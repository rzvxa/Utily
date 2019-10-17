using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils.IO
{
    public static class BinaryHelper
    {
        public static byte[] ToBinary(this object obj)
        {
            if(obj is null)
                return null;
            using(var memStream = new MemoryStream())
            {
                var bFormatter = new BinaryFormatter();
                bFormatter.Serialize(memStream, obj);
                return Compress(memStream.ToArray());
            }
        }

        public static object ToObject(this byte[] bytes)
        {
            using (var memStream = new MemoryStream())
            {
                var bFormatter = new BinaryFormatter();
                bytes = Decompress(bytes);
                memStream.Write(bytes, 0, bytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                return bFormatter.Deserialize(memStream);
            }
        }

        public static byte[] Compress(byte[] bytes)
        {
            byte[] @return;
            using(var memStream = new MemoryStream())
            {
                using(var zip = new GZipStream(memStream, CompressionMode.Compress))
                {
                    zip.Write(bytes, 0, bytes.Length);
                }
                @return = memStream.ToArray();
            }
            return @return;
        }

        public static byte[] Decompress(byte[] bytes)
        {
            byte[] @return;
            using (var outMemStream = new MemoryStream())
            {
                using (var inMemStream = new MemoryStream(bytes))
                {
                    using (var zip = new GZipStream(inMemStream, CompressionMode.Decompress))
                    {
                        zip.CopyTo(outMemStream);
                    }
                }
                @return = outMemStream.ToArray();
            }
            return @return;
        }
    }
}
