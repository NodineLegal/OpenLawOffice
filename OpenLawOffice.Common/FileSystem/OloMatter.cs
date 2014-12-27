using System;
using System.IO;

namespace OpenLawOffice.Common.FileSystem
{
    public class OloMatter
    {
        public string Title { get; private set; }
        public string CaseNumber { get; private set; }

        public OloMatter()
        {
        }

        public static OloMatter Parse(string fileSystemPath)
        {
            string wholeName;
            OloMatter matter = new OloMatter();

            string afterRoot = fileSystemPath.Replace(OpenLawOffice.Common.Settings.Manager.Instance.FileStorage.MattersPath, "");

            if (string.IsNullOrEmpty(afterRoot))
                throw new ArgumentException("Invalid path.");

            if (afterRoot.IndexOf(Path.DirectorySeparatorChar) < 1)
            { // Just a matter folder
                wholeName = afterRoot;
            }
            else
            { // Filer or folder within a matter
                wholeName = afterRoot.Substring(0, afterRoot.IndexOf(Path.DirectorySeparatorChar));
            }

            if (wholeName.LastIndexOf("(") > 0)
            { // Has case number
                matter.Title = wholeName.Substring(0, wholeName.LastIndexOf("(")).Trim();
                matter.CaseNumber = wholeName.Substring(wholeName.LastIndexOf("(") + 1);
                matter.CaseNumber = matter.CaseNumber.Substring(0, matter.CaseNumber.LastIndexOf(")"));
            }
            else
            {
                matter.Title = wholeName.Trim();
            }

            return matter;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(CaseNumber))
                return Title;
            else
                return Title + "(" + CaseNumber + ")";
        }
    }
}
