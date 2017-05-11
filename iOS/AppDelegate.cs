using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;

namespace sub100DemoApp.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

			global::Xamarin.Forms.Forms.Init();
			CachedImageRenderer.Init();
			FormsPlugin.Iconize.iOS.IconControls.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
