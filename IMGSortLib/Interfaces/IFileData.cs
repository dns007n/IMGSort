using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGSortLib.Interfaces
{
    interface IFileData
    {
        FileAttributes Attributes { get; }
        DateTime CreationTime { get; }
        DateTime LastAccessTime { get; }
        DateTime LastWriteTime { get; }
        long Size { get; }
        string Name { get; }
        string Path { get; }
    }
}
