using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsIndexerNET
{
    internal class PdfUtils
    {
        // Descomprimir uno de los streams del PDF
        private static string decompress(byte[] input)
        {
            var stream = new MemoryStream();

            using (var compressStream = new MemoryStream(input))
            using (var decompressor = new DeflateStream(compressStream, CompressionMode.Decompress))
                decompressor.CopyTo(stream);

            return Encoding.Default.GetString(stream.ToArray());
        }
    }
}
