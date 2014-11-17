using System;
using System.IO;

namespace OpenLawOffice.Common.FileSystem
{
    public class OloFile
    {
        public OloMatter Matter { get; private set; }
        public string MatterRelativePath { get; private set; }
        public DateTime? TimeStamp { get; private set; }
        public string Title { get; private set; }
        public string Extension { get; private set; }

        public OloFile()
        {
        }

        public static OloFile Parse(string fileSystemPath)
        {
            OloFile file = new OloFile();
            OloMatter Matter = OloMatter.Parse(fileSystemPath);

            string afterMatter = fileSystemPath.Replace(Matter.ToString(), "");

            while (afterMatter.EndsWith("\\") || afterMatter.EndsWith("/"))
                afterMatter = afterMatter.TrimEnd('\\').TrimEnd('/');

            if (afterMatter.IndexOf(Path.DirectorySeparatorChar) > 0)
            { // Has relative path
                file.MatterRelativePath = afterMatter.Substring(0, afterMatter.LastIndexOf(Path.DirectorySeparatorChar));
                file.Title = afterMatter.Substring(afterMatter.LastIndexOf(Path.DirectorySeparatorChar));
                file.Extension = file.Title.Substring(file.Title.LastIndexOf("."));
                file.Title = file.Title.Substring(0, file.Title.LastIndexOf(".") - 1);
            }
            else
            {
                file.Extension = afterMatter.Substring(afterMatter.LastIndexOf("."));
                file.Title = afterMatter.Substring(0, afterMatter.LastIndexOf(".") - 1);
            }

            return file;
        }

        public override string ToString()
        {
            string output = Matter.ToString() + Path.DirectorySeparatorChar + MatterRelativePath;

            if (!output.EndsWith(Path.DirectorySeparatorChar.ToString()))
                output += Path.DirectorySeparatorChar;

            if (TimeStamp.HasValue)
                output += TimeStamp.Value.ToString("yyyy-MM-dd") + " ";

            output += Title;

            if (!string.IsNullOrEmpty(Extension))
                output += "." + Extension;

            return output;
        }
    }
}
