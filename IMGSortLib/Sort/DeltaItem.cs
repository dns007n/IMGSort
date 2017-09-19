using FastDirectoryEnumerator;
using IMGSortLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGSortLib.Sort
{
    internal class DeltaItem
    {
        public int ID { get; set; }
        public IFileData SourceFile { get; set; }
        public string FileHash { get; set; }
        public string TargetPath { get; set; }
        public string TargetFileName { get; set; }
    }
}
