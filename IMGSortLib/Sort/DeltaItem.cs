using FastDirectoryEnumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGSortLib.Sort
{
    class DeltaItem
    {
        public FileData SourceFile { get; set; }
        public string TargetPath { get; set; }
        public string TargetFileName { get; set; }
        
    }
}
