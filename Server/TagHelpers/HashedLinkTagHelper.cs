namespace NgSnippets.TagHelpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class HashedLinkTagHelper : TagHelper
    {
        public string LinkName { get; set; }

        private IHostingEnvironment HostingEnvironment { get; }

        private const string Rel = "stylesheet";

        private const string Type = "text/css";

        public HashedLinkTagHelper(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var directoryRoot = new DirectoryInfo("wwwroot");
            var hashedFile = directoryRoot
                                  .GetFiles("*.css", SearchOption.TopDirectoryOnly)
                                  .FirstOrDefault(f => GetMainFileName(f.Name) == LinkName);

            output.TagName = "link";

            output.Attributes.SetAttribute("rel", Rel);
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
    }
}
