using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Roget
{
	/// <summary>
	/// Parts of speech include Noun, Adjective, Phrase. This view
	/// shows all the related works in these groupings.
	/// </summary>
	/// <remarks>
	/// Uses UIWebView since we want to format the text display (with HTML)
	/// </remarks>
	public class PartsOfSpeechViewController : UIViewController
	{
		/// <summary>
		/// MainViewController is required by most of the other ViewControllers 
		/// in the app so we can Push more views on for navigation
		/// </summary>
		MainViewController rootmvc;
		List<RogetPartOfSpeech> partsOfSpeech;
		
		public PartsOfSpeechViewController (MainViewController mvc, RogetCategory displayCategory) : base()
		{
			rootmvc = mvc;
			partsOfSpeech = displayCategory.PartsOfSpeech;
		}
		
		public UITextView textView;
		public UIWebView webView;
		
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			// no XIB !
			webView = new UIWebView()
			{
				ScalesPageToFit = false
			};
			webView.LoadHtmlString(FormatText(), new NSUrl());
			
			// Set the web view to fit the width of the app.
            webView.SizeToFit();

            // Reposition and resize the receiver
            webView.Frame = new RectangleF (
                0, 0, this.View.Frame.Width, this.View.Frame.Height);

            // Add the table view as a subview
            this.View.AddSubview(webView);
			
		}		
		/// <summary>
		/// Format the parts-of-speech text for UIWebView
		/// </summary>
		private string FormatText()
		{
			StringBuilder sb = new StringBuilder();
			
			foreach (var part in partsOfSpeech)
			{
				sb.Append("<style>body,b,p{font-family:Helvetica;}</style>");
				sb.Append("<b>"+part.PartOfSpeech+"</b><br/>"+ Environment.NewLine);
				foreach (var line in part.Lines)
				{
					sb.Append(line + "<br/>" + Environment.NewLine);
				}
				sb.Append("<br/>");
			}
			return sb.ToString();
		}
	}
}
