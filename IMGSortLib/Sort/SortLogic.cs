using FastDirectoryEnumerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGSortLib.Sort
{
    class SortLogic
    {
        private string _path;
        public static SortLogic Current { get; set; }
        private List<DeltaItem> _fileItems;
        public IEnumerable<DeltaItem> FileItems { get; }
        public string TargetPath { get; set; }

        public SortLogic(string sourcePath)
        {
            _path = sourcePath;
        }

        public void GetSourceFiles()
        {
            _fileItems = new List<DeltaItem>();
            foreach (FileData item in FastDirectoryEnumerator.FastDirectoryEnumerator.EnumerateFiles(_path, "*", SearchOption.AllDirectories))
            {
                _fileItems.Add(new DeltaItem() { SourceFile = item });
            }
        }
        public void CalcTarget()
        {
            foreach (DeltaItem item in _fileItems)
            {
                DateTime lwt = item.SourceFile.LastWriteTime;
                item.TargetPath = string.Format("{0}/{1}/{0}-{1}-{2}",
                                                  lwt.Year,
                                                  lwt.Month,
                                                  lwt.Day);
                item.TargetFileName = String.Format("IMG {0}-{1}-{2}",
                                                     lwt.Hour,
                                                     lwt.Minute,
                                                     lwt.Second);
                
            }
        }
    }
}
