namespace NgSnippets.TagHelpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class HashedScriptTagHelper : TagHelper
    {
        public string ScriptName { get; set; }

        private IHostingEnvironment HostingEnvironment { get; }

        private const string Type = "text/javascript";

        public HashedScriptTagHelper(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var directoryRoot = new DirectoryInfo("wwwroot");
            var hashedFile = directoryRoot
                                  .GetFiles("*.js", SearchOption.TopDirectoryOnly)
                                  .FirstOrDefault(f => GetMainFileName(f.Name) == ScriptName);

            output.TagName = "script";

            output.Attributes.SetAttribute("type", Type);
            output.Attributes.SetAttribute("src", hashedFile.FullName.Replace(directoryRoot.FullName, string.Empty));

            // https://stackoverflow.com/questions/10520048/calculate-md5-checksum-for-a-file#answer-10520086
            // https://stackoverflow.com/questions/38356083/sri-hash-not-what-is-expected
            using (var sha512 = SHA512.Create())
            {
                using (var stream = File.OpenRead(hashedFile.FullName))
                {
                    var hash = sha512.ComputeHash(stream);
                    output.Attributes.SetAttribute("integrity", $"sha512-{Convert.ToBase64String(hash)}");
                }
            }
        }

        private static string GetMainFileName(string file)
        {
            return file.Substring(0, file.IndexOf('.'));
        }

        // public static MvcHtmlString VersionedStyle(string file, string institutionAbbreviation)
        // {
        //     return VersionedStyleContent("<link href=\"assets\\{0}\\{1}\" rel=\"stylesheet\" type=\"text/css\">", file, institutionAbbreviation);
        // }

        // private static MvcHtmlString VersionedStyleContent(string template, string file, string institutionAbbreviation)
        // {
        //     var rootServerDir = HttpContext.Current.Server.MapPath("~");
        //     var rootDir = Path.Combine(rootServerDir, "assets", institutionAbbreviation);
        //     var files = new DirectoryInfo(rootDir).GetFiles("*.css", SearchOption.TopDirectoryOnly);
        //     var hashedFile = files.FirstOrDefault(f => GetMainFileName(f.Name) == file);

        //     return MvcHtmlString.Create(hashedFile == null ? "" : string.Format(template, institutionAbbreviation, hashedFile));
        // }
    }
}
