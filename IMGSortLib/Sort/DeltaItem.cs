using FastDirectoryEnumerator;
using IMGSortLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGSortLib.Sort
{
    public class DeltaItem
    {

        public int ID { get; set; }
        public IFileData SourceFile { get; set; }
        public string FileHash { get; set; }
        public string TargetPath { get; set; }
        public string TargetFileName { get; set; }
        public string FullName => SourceFile.Path;
    }

}
