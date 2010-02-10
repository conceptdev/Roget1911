using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Roget
{
	/// <summary>
	/// First view that users see - lists the top level of the hierarchy xml
	/// </summary>
	/// <remarks>
	/// LOADS data from the xml files into public properties (deserialization)
	/// then we pass around references to the MainViewController so other
	/// ViewControllers can access the data.
	/// 
	/// Added the new UITableViewSource</remarks>
    [Register]
    public class MainViewController : UITableViewController
    {
        private UITableView tableView;
		
		/// <summary>'Class'es are the root list of items in the hierarchy</summary>
		public List<RogetClass> Classes = new List<RogetClass>();
		/// <summary>The main data-set of words by part-of-speech</summary>
		public RogetCategories Categories;
		
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

			#region load data from XML
			RogetHierarchy hierarchy;
			using (TextReader reader = new StreamReader("roget15aHierarchy.xml"))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(RogetHierarchy));
				hierarchy = (RogetHierarchy)serializer.Deserialize(reader);
				// HACK: makes Divisions synonymous with Sections, makes navigation easier
				foreach (var h in hierarchy.Classes)
				{
					foreach (RogetDivision d in h.Divisions)
					{
						h.Sections.Add(new RogetSection{Name=d.Name, Sections = d.Sections});
					}
				}
			} 
			Classes = hierarchy.Classes;
			
			using (TextReader reader = new StreamReader("roget15aCategories.xml"))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(RogetCategories));
				Categories = (RogetCategories)serializer.Deserialize(reader);
			}
			#endregion
			
			// no XIB !
			tableView = new UITableView()
			{
				Source = new TableViewSource (Classes, this),
			    //Delegate = new TableViewDelegate(Classes, this),
			    //DataSource = new TableViewDataSource(Classes, this),
			    AutoresizingMask = UIViewAutoresizing.FlexibleHeight|
			                       UIViewAutoresizing.FlexibleWidth,
			    BackgroundColor = UIColor.White,
			};
			
			// Set the table view to fit the width of the app.
			tableView.SizeToFit();
			
			// Reposition and resize the receiver
			tableView.Frame = new RectangleF (
			    0, 0, this.View.Frame.Width, this.View.Frame.Height);
			
			// Add the table view as a subview
			this.View.AddSubview(tableView);
			
            Console.WriteLine("Is you're using the simulator, switch to it now.");
        }

		/// <summary>
		/// Extends the new UITableViewSource in MonoTouch 1.2 (4-Nov-09)
		/// </summary>
		private class TableViewSource : UITableViewSource
		{
            static NSString kCellIdentifier = new NSString ("MyIdentifier");
			private List<RogetClass> list;
			private MainViewController mvc;
			
            public TableViewSource (List<RogetClass> list, MainViewController controller)
            {
                this.list = list;
				mvc = controller;
            }
			
			public override int RowsInSection (UITableView tableview, int section)
            {
                return list.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
                }
                cell.TextLabel.Text = list[indexPath.Row].Name;
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                return cell;
            }
			
			/// <summary>
			/// If there are subsections in the hierarchy, navigate to those
			/// ASSUMES there are _never_ Categories hanging off the root in the hierarchy
			/// </summary>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("MAIN TableViewDelegate.RowSelected: Label={0}", list[indexPath.Row].Name);
				
				SectionViewController uivc = new SectionViewController(mvc);
				uivc.Title = list[indexPath.Row].Name;
				uivc.Sections = mvc.Classes[indexPath.Row].Sections;
				if (uivc.Sections.Count == 0)
				{
					Console.WriteLine("Doesn't support 'words' hanging off the root RogetClass elements");
				}
				else
				{
					Console.WriteLine("  thesaurus count: " + uivc.Sections.Count);
					mvc.NavigationController.PushViewController(uivc,true);
            		}
			}
		}
		
		#region Obsolete Delegates
		[Obsolete("Replaced by UITableViewSource")]
        private class TableViewDelegate : UITableViewDelegate
        {
            private List<RogetClass> list;
			private MainViewController mvc;
			
            public TableViewDelegate(List<RogetClass> list, MainViewController controller)
            {
                this.list = list;
				mvc = controller;
            }

			/// <summary>
			/// If there are subsections in the hierarchy, navigate to those
			/// ASSUMES there are _never_ Categories hanging off the root in the hierarchy
			/// </summary>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("MAIN TableViewDelegate.RowSelected: Label={0}", list[indexPath.Row].Name);
				
				SectionViewController uivc = new SectionViewController(mvc);
				uivc.Title = list[indexPath.Row].Name;
				uivc.Sections = mvc.Classes[indexPath.Row].Sections;
				if (uivc.Sections.Count == 0)
				{
					Console.WriteLine("Doesn't support 'words' hanging off the root RogetClass elements");
				}
				else
				{
					Console.WriteLine("  thesaurus count: " + uivc.Sections.Count);
					mvc.NavigationController.PushViewController(uivc,true);
            		}
			}
        }

		[Obsolete("Replaced by UITableViewSource")]
        private class TableViewDataSource : UITableViewDataSource
        {
            static NSString kCellIdentifier = new NSString ("MyIdentifier");
            private List<RogetClass> list;
			private MainViewController mvc;
            public TableViewDataSource (List<RogetClass> list, MainViewController controller)
            {
                this.list = list;
				mvc = controller;
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                return list.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (kCellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
                }
                cell.TextLabel.Text = list[indexPath.Row].Name;
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                return cell;
            }
        }
		#endregion
    }
}