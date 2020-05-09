namespace SEI.Web.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Optimization;

    public static class BundleExtensions
    {
        /// <summary>
        /// Applies the CssRewriteUrlTransform to every path in the array.
        /// </summary>      
        public static Bundle IncludeWithCssRewriteUrlTransform(this Bundle bundle, params string[] virtualPaths)
        {
            if ((virtualPaths != null) && virtualPaths.Any())
            {
                foreach (var virtualPath in virtualPaths)
                {
                    bundle.Include(virtualPath, new CssRewriteUrlTransformFixed());
                }
            }

            return bundle;
        }

        /// <summary>
        /// Fixes for the standard System.Web.Optimization.CssRewriteUrlTransform. 
        /// </summary>
        public class CssRewriteUrlTransformFixed : IItemTransform
        {
            public string Process(string includedVirtualPath, string input)
            {
                if (includedVirtualPath == null)
                {
                    throw new ArgumentNullException(nameof(includedVirtualPath));
                }

                if (includedVirtualPath.Length < 1 || includedVirtualPath[0] != '~')
                {
                    throw new ArgumentException("includedVirtualPath must be valid ( i.e. have a length and start with ~ )");
                }

                var directory = VirtualPathUtility.GetDirectory(includedVirtualPath);
                return ConvertUrlsToAbsolute(directory, input);
            }

            private static string ConvertUrlsToAbsolute(string baseUrl, string content)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return content;
                }

                var matchEvaluator = MatchEvaluator(baseUrl);
                var regex = new Regex("url\\((?<prefix>['\"]?)(?<url>[^)]+?)(?<suffix>['\"]?)\\)");
                return regex.Replace(content, matchEvaluator);
            }

            private static MatchEvaluator MatchEvaluator(string baseUrl)
            {
                return
                    match =>
                        "url("
                        + RebaseUrlToAbsolute(
                            baseUrl,
                            match.Groups["url"].Value,
                            match.Groups["prefix"].Value,
                            match.Groups["suffix"].Value) + ")";
            }

            private static string RebaseUrlToAbsolute(string baseUrl, string url, string prefix, string suffix)
            {
                var isRootUrl = string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(baseUrl) ||
                                url.StartsWith("/", StringComparison.OrdinalIgnoreCase)
                                || url.StartsWith("http://") || url.StartsWith("https://");

                if (isRootUrl)
                {
                    return url;
                }

                if (url.StartsWith("data:"))
                {
                    // Keep the prefix and suffix quotation chars as is in case they are needed (e.g. non-base64 encoded svg)
                    return prefix + url + suffix;
                }

                if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl += "/";
                }

                return VirtualPathUtility.ToAbsolute(baseUrl + url);
            }
        }
    }
}