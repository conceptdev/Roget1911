using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Roget
{
	/// <summary>
	/// Provide a TableView to navigate through the hierarchy of Sections
	/// </summary>
	[Register]
	public class SectionViewController : UITableViewController
	{
		private UITableView tableView;
		/// <summary>
		/// MainViewController is required by most of the other ViewControllers 
		/// in the app so we can Push more views on for navigation
		/// </summary>
		private MainViewController rootmvc;
		public List<RogetSection> Sections = new List<RogetSection>();
		
		public SectionViewController (MainViewController mvc) : base() 
		{
			rootmvc = mvc;
		}
		
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			// no XIB !
            tableView = new UITableView()
            {
                Delegate = new TableViewDelegate(Sections, rootmvc, this),
                DataSource = new TableViewDataSource(Sections, rootmvc, this),
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
			
		}		
		
		private class TableViewDelegate : UITableViewDelegate
        {
            private List<RogetSection> list;
			private MainViewController mvc;
			private SectionViewController svc;
			
            public TableViewDelegate(List<RogetSection> list, MainViewController controller, SectionViewController sectionController)
            {
                this.list = list;
				mvc = controller;
				svc = sectionController;
            }

			/// <summary>
			/// A section was touched. Either show the next level of of Sections
			/// or load a different view for Categories
			/// </summary>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                Console.WriteLine("SECTION TableViewDelegate.RowSelected: Label={0}", list[indexPath.Row].Name);
				
				if (svc.Sections[indexPath.Row].Sections.Count == 0)
				{	// show words
					Console.WriteLine("  show words {0} to {1}", list[indexPath.Row].StartCategory, list[indexPath.Row].EndCategory);
					CategoryViewController secvc = new CategoryViewController(mvc, list[indexPath.Row].StartCategory, list[indexPath.Row].EndCategory);
					secvc.Title = list[indexPath.Row].Name;
					mvc.NavigationController.PushViewController(secvc,true);
				}
				else
				{	// show more sections (more hierarchy)
					SectionViewController uivc = new SectionViewController(mvc);
					uivc.Title = list[indexPath.Row].Name;
					uivc.Sections = svc.Sections[indexPath.Row].Sections;
					Console.WriteLine("  thesaurus count: " + uivc.Sections.Count);
					mvc.NavigationController.PushViewController(uivc,true);
            		}
            }
        }

        private class TableViewDataSource : UITableViewDataSource
        {
            static NSString kCellIdentifier = new NSString ("MyIdentifier");
            private List<RogetSection> list;
			private MainViewController mvc;
			private SectionViewController svc;
            public TableViewDataSource (List<RogetSection> list, MainViewController controller, SectionViewController sectionController)
            {
                this.list = list;
				mvc = controller;
				svc = sectionController;
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
	}
}
