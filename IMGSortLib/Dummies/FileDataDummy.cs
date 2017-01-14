using IMGSortLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IMGSortLib.Dummies
{
    class FileDataDummy : IFileData
    {
        public FileAttributes Attributes { get; }

        public DateTime CreationTime { get; }

        public DateTime LastAccessTime { get; }

        public DateTime LastWriteTime { get; }

        public string Name { get; }

        public string Path { get; }

        public long Size { get; }

        public FileDataDummy(string fileName, string path)
        {
            Name = fileName;
            Path = path;
        }
        public FileDataDummy(string fileName, string path,DateTime lastWriteTime, DateTime lastAccessTime)
        {
            Name = fileName;
            Path = path;
            LastAccessTime = lastAccessTime;
            LastWriteTime = lastWriteTime;
        }

    }
}
