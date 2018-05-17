using IMGSortLib.Interfaces;
using System.IO;

namespace IMGSortLib.Sort
{
    public class DeltaItem
    {

        public int ID { get; set; }
        public IFileData SourceFile { get; set; }
        public string FileHash { get; set; }
        public string TargetPath { get; set; }
        public string TargetFileName { get; set; }
        public string SourceFullPath => SourceFile?.Path;
        public string TargetFullPath => TargetPath != null && TargetFileName != null ? Path.Combine(TargetPath, TargetFileName) : null;
    }

}
