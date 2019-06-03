using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public static class Unzipper
    {
        public static byte[] Unzip(byte[] zipped)
        {
            using (var ms = new MemoryStream(zipped))
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Read))
            using (var entryStream = archive.Entries[0].Open())
            using (var outputStream = new MemoryStream())
            {
                entryStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
