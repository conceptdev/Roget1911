using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Roget
{
	/// <summary>
	/// Show the 'categories' of word in a scrolling table
	/// </summary>
	/// <remarks>
	/// The data for this view comes from the Categories.xml and is
	/// parsed in MainViewController (so it's only done once)
	/// </remarks>
	public class CategoryViewController : UITableViewController
	{
		private UITableView tableView;
		/// <summary>
		/// MainViewController is required by most of the other ViewControllers 
		/// in the app so we can Push more views on for navigation
		/// </summary>
		private MainViewController rootmvc;
		/// <summary>Start of category 'range'</summary>
		private string start;
		/// <summary>End of category 'range'</summary>
		private string end;
		/// <summary>Collection(subset) of the Categories we're showing on this view</summary>
		private List<RogetCategory> showCategories = new List<RogetCategory>();
		
		public CategoryViewController (MainViewController mvc, string startCategory, string endCategory) : base() 
		{
			rootmvc = mvc;
			start = startCategory;
			end = endCategory;
		}
		
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
			Console.Write("Total categories {0} ", rootmvc.Categories.Categories.Count);
			showCategories = rootmvc.Categories.GetRange (start, end);
			Console.WriteLine (" range ({0},{1}) items {2}", start,end,showCategories.Count);
			
			// no XIB !
            tableView = new UITableView()
            {
                Delegate = new TableViewDelegate(showCategories, rootmvc),
                DataSource = new TableViewDataSource(showCategories, rootmvc),
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
            private List<RogetCategory> list;
			private MainViewController mvc;
			
            public TableViewDelegate(List<RogetCategory> list, MainViewController controller)
            {
                this.list = list;
				mvc = controller;
            }

			/// <summary>
			/// A category was clicked. Show all the parts-of-speech results for it
			/// (including all the actual words :) in a new view (uses UIWebView)
			/// </summary>
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {	// show detail
				var selectedCategory = list[indexPath.Row];
                Console.WriteLine("CATEGORY TableViewDelegate.RowSelected: Label={0}", selectedCategory.Name);
				
				PartsOfSpeechViewController posvc = new PartsOfSpeechViewController(mvc, selectedCategory);
				posvc.Title = selectedCategory.Name;
				Console.WriteLine("  category count: " + indexPath.Row);
				mvc.NavigationController.PushViewController (posvc, true);
				
            }
        }

        private class TableViewDataSource : UITableViewDataSource
        {
            static NSString kCellIdentifier = new NSString ("MyIdentifier");
            private List<RogetCategory> list;
			private MainViewController mvc;
			
            public TableViewDataSource (List<RogetCategory> list, MainViewController controller)
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
				cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
                return cell;
            }
        }
	}
}
