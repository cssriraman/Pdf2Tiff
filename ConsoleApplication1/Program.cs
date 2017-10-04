using Pdf2Tiff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spire Investor Presentation.pdf");
            var outDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "out");
            Directory.CreateDirectory(outDir);
            var spirePdf2TiffConverter = new SpirePdf2TiffConverter(outDir);
            spirePdf2TiffConverter.Process(filePath);
        }
    }
}
