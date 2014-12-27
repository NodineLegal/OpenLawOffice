using System;
using System.IO;

namespace OpenLawOffice.Common.FileSystem
{
    public class OloMatterDirectory
    {
        public OloMatter Matter { get; private set; }
        public string MatterRelativePath { get; private set; }

        public OloMatterDirectory()
        {
        }

        public static OloMatterDirectory Parse(string fileSystemPath)
        {
            OloMatter matter = OloMatter.Parse(fileSystemPath);
            
            // Trim down to just the directory
            if (Utilities.IsFile(fileSystemPath).Value)
            {
                fileSystemPath = Path.GetDirectoryName(fileSystemPath);
            }

            string afterMatter = fileSystemPath.Replace(matter.ToString(), "");

            while (afterMatter.EndsWith("\\") || afterMatter.EndsWith("/"))
                afterMatter = afterMatter.TrimEnd('\\').TrimEnd('/');

            if (afterMatter.IndexOf(Path.DirectorySeparatorChar) > 0)
            { // Has relative path
                return new OloMatterDirectory()
                {
                    Matter = matter,
                    MatterRelativePath = Path.DirectorySeparatorChar + afterMatter.Substring(0, afterMatter.LastIndexOf(Path.DirectorySeparatorChar))
                };
            }
            else
            {
                return new OloMatterDirectory()
                {
                    Matter = matter,
                    MatterRelativePath = ""
                };
            }
        }

        public override string ToString()
        {
            return Matter.ToString() + MatterRelativePath;
        }
    }
}
