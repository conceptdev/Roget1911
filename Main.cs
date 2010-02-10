using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Roget
{
	/// <summary>
	/// UITableView sample originally from
	/// http://sabonrai.wordpress.com/2009/08/28/monotouch-sample-code-uitableview/
	/// 
	/// Roget's data from Charles Petzold
	/// http://www.charlespetzold.com/blog/2009/08/Rogets-Hierarchical-Thesaurus-in-a-Silverlight-App.html
	/// </summary>
	public class Application
    {
        static void Main (string[] args)
        {
			try
			{
            		UIApplication.Main (args, null, "AppController");
			}
			catch (Exception ex)
			{	// HACK: this is just here for debugging
				Console.WriteLine(ex);
			}
        }
    }
}
