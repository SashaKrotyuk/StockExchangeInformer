namespace SEI.Web
{
	using System;
	using System.Web.Optimization;

	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
#if DEBUG
			BundleTable.EnableOptimizations = false;
			bundles.UseCdn = false;
#else
			BundleTable.EnableOptimizations = true;
			bundles.UseCdn = true;
#endif

			bundles.DirectoryFilter.Clear();
			AddDefaultIgnoresToBundles(bundles.DirectoryFilter);
			RegisterStyleBundles(bundles);
			RegisterScriptBundles(bundles);
		}

		public static void RegisterStyleBundles(BundleCollection bundles)
		{
			// bundles.Add(new StyleBundle("~/bundles/styles/main")
			//   .IncludeWithCssRewriteUrlTransform("~/Content/site.css"));
		}

		public static void RegisterScriptBundles(BundleCollection bundles)
		{
			// bundles.Add(new ScriptBundle("~/bundles/scripts/lodash")
			//    .Include("~/Scripts/_import/lodash/lodash.js"));
		}

		private static void AddDefaultIgnoresToBundles(IgnoreList ignoreList)
		{
			if (ignoreList == null)
			{
				throw new ArgumentNullException(nameof(ignoreList));
			}

			ignoreList.Ignore("*.min.js", OptimizationMode.Always);
			ignoreList.Ignore("*.min.css", OptimizationMode.Always);
		}
	}
}