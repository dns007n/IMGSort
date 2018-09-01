using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        internal ICollection<DeltaItem> _fileItems;
        public IEnumerable<DeltaItem> FileItems { get { return _fileItems; } }
        public IEnumerable<string> SourcePaths { get; set; }
        public string TargetPath { get; set; }
        private int _Duplicates;
        public int Duplicates { get { return _Duplicates; } }

        public Task GetSourceFilesAsync()
        {
            return Task.Run(() => GetSourceFiles());
        }
        public Task CalcTargetAsync()
        {
            return Task.Run(() => CalcTarget());
        }
        public Task RemoveDuplicatesAsync()
        {
            return Task.Run(() => RemoveDuplicates());
        }
        public Task CopyFilesToTargetAsync()
        {
            return Task.Run(() => CopyFilesToTarget());
        }

        internal void GetSourceFiles()
        {
            _fileItems = new HashSet<DeltaItem>();
            int idCounter = 0;

            foreach (string sourcePath in SourcePaths)
            {
                Parallel.ForEach(FastDirectoryEnumerator.FastDirectoryEnumerator.EnumerateFiles(sourcePath, "*.jpg", SearchOption.AllDirectories), item =>
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
                    _fileItems.Add(new DeltaItem() { ID = idCounter, SourceFile = item, FileHash = fileHash });
                });

            }
        }

        internal void CalcTarget()
        {
            foreach (DeltaItem item in _fileItems)
            {
                DateTime lwt = item.SourceFile.LastWriteTime;
                DateTime imageTaken;
                try
                {
                    lwt = GetDateTakenFromImage(item.SourceFullPath);

                }
                catch (Exception ex)
                {


                }
                item.TargetPath = string.Format(@"{0}\{1}\{2}\{1}-{2}-{3}",
                                 TargetPath,
                                 lwt.Year,
                                 lwt.Month.ToString("00"),
                                 lwt.Day.ToString("00"));
                string[] split = item.SourceFile.Name.Split('.');
                string fileEnding = split.Count() > 1 ? split[split.Count() - 1] : string.Empty;
                item.TargetFileName = string.Format(@"IMG_{0}{1}{2}_{3}{4}{5}-00.{6}",
                                                     lwt.Year,
                                                     lwt.Month.ToString("00"),
                                                     lwt.Day.ToString("00"),
                                                     lwt.Hour.ToString("00"),
                                                     lwt.Minute.ToString("00"),
                                                     lwt.Second.ToString("00"),
                                                     fileEnding);
            }
        }

        internal void RemoveDuplicates()
        {
            DeltaItem[] duplicates = _fileItems.GroupBy(s => s.FileHash).SelectMany(grp => grp.Skip(1)).ToArray();
            _Duplicates = duplicates.Count();
            foreach (DeltaItem item in duplicates)
            {
                _fileItems.Remove(item);

            }
            var xTimesItems = _fileItems.GroupBy(x => x.TargetFullPath).Where(x => x.Count() > 1);
            foreach (var grouping in xTimesItems)
            {
                int counter = 0;
                foreach (var item in grouping)
                {
                    item.TargetFileName = item.TargetFileName.Replace("-00", "-" + counter.ToString("00"));
                    counter++;
                }
            }
        }

        internal void CopyFilesToTarget()
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
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        //retrieves the datetime WITHOUT loading the whole image
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
    }
}
