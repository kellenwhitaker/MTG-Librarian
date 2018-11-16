using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image FromByteArray(byte[] bytes)
        {
            if (bytes == null) return null;
            MemoryStream ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }
    }    
}
