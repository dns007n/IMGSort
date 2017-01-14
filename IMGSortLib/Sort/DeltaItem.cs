using FastDirectoryEnumerator;
using IMGSortLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGSortLib.Sort
{
    class DeltaItem
    {
        public int ID { get; set; }
        public IFileData SourceFile { get; set; }
        public string TargetPath { get; set; }
        public string TargetFileName { get; set; }
    }
}
