using FastDirectoryEnumerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMGSortLib.Sort
{
    public class SortLogic
    {
        public static SortLogic Current { get; set; }
        internal List<DeltaItem> _fileItems;
        internal IEnumerable<DeltaItem> FileItems { get; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }


        public void GetSourceFiles()
        {
            _fileItems = new List<DeltaItem>();
            int idCounter = 0;
            foreach (FileData item in FastDirectoryEnumerator.FastDirectoryEnumerator.EnumerateFiles(SourcePath, "*", SearchOption.AllDirectories))
            {
                idCounter++;
                string fileHash;
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(item.Path))
                    {
                        fileHash = Encoding.Default.GetString(md5.ComputeHash(stream));
                    }
                }
                _fileItems.Add(new DeltaItem() { ID=idCounter, SourceFile = item, FileHash=fileHash });
            }
        }
        public void CalcTarget()
        {
            foreach (DeltaItem item in _fileItems)
            {
                DateTime lwt = item.SourceFile.LastWriteTime;
                item.TargetPath = string.Format(@"{0}\{1}\{2}\{1}-{2}-{3}",
                                                  TargetPath,
                                                  lwt.Year,
                                                  lwt.Month.ToString("00"),
                                                  lwt.Day.ToString("00"));
                string[] split = item.SourceFile.Name.Split('.');
                string fileEnding = split.Count() > 1 ? split[split.Count()-1] : string.Empty;
                item.TargetFileName = String.Format(@"IMG {0}-{1}-{2}.{3}",
                                                     lwt.Hour.ToString("00"),
                                                     lwt.Minute.ToString("00"),
                                                     lwt.Second.ToString("00"),
                                                     fileEnding);
            }
        }

        public void RemoveDuplicates()
        {
            DeltaItem[] duplicates = _fileItems.GroupBy(s => s.FileHash).SelectMany(grp => grp.Skip(1)).ToArray();
            foreach (DeltaItem item in duplicates)
            {
                _fileItems.Remove(item);

            }
        }

        public void CopyFilesToTarget()
        {
            foreach (DeltaItem item in _fileItems)
            {
                if (!Directory.Exists(item.TargetPath))
                {
                    Directory.CreateDirectory(item.TargetPath);
                }
                try
                {
                    File.Copy(item.SourceFile.Path, Path.Combine(item.TargetPath, item.TargetFileName), true);

                }
                catch (Exception)
                {
                    //TODO Log
                }
            }
        }
    }
}
