using Spire.Pdf;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Pdf2Tiff
{
    public class SpirePdf2TiffConverter
    {
        public string OutDir { get; private set; }
        public SpirePdf2TiffConverter(string outDir)
        {
            OutDir = outDir;
        }
        public void Process(string filePath)
        {
            var fileBaseName = Path.GetFileNameWithoutExtension(filePath);
            var outTiff = Path.Combine(OutDir, $"{fileBaseName}.tiff");
            var pdfDoc = new PdfDocument();
            pdfDoc.LoadFromFile(filePath);
            CreatePdfToTiff(outTiff, pdfDoc);
        }

        private void CreatePdfToTiff(string outTiff, PdfDocument pdfDoc)
        {
            CreateTiffFromImages(SaveAsImage(pdfDoc), outTiff);
        }
        private Image[] SaveAsImage(PdfDocument pdfDoc)
        {
            Console.WriteLine("Save As Image ...");
            var images = new Image[pdfDoc.Pages.Count];
            for (int i = 0; i <= pdfDoc.Pages.Count - 1; i++)
            {
                //use the document.SaveAsImage() method save the pdf as image
                images[i] = pdfDoc.SaveAsImage(i);
            }
            return images;
        }

        /// <summary>
        /// Create Tiff from image Collection
        /// </summary>
        /// <param name="images"></param>
        /// <param name="outFile"></param>
        /// 
        public static void CreateTiffFromImages(Image[] images, string outFile)
        {
            //use the save encoder
            var enc = Encoder.SaveFlag;
            var ep = new EncoderParameters(2);
            ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.MultiFrame);
            ep.Param[1] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
            var pages = images[0];
            var frame = 0;
            var info = GetEncoderInfo("image/tiff");
            foreach (var img in images)
            {
                if (frame == 0)
                {
                    pages = img;
                    //save the first frame
                    pages.Save(outFile, info, ep);
                }

                else
                {
                    //save the intermediate frames
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.FrameDimensionPage);

                    pages.SaveAdd(img, ep);
                }
                if (frame == images.Length - 1)
                {
                    //flush and close.
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.Flush);
                    pages.SaveAdd(ep);
                }
                frame++;
            }
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            throw new Exception(mimeType + " mime type not found in ImageCodecInfo");
        }
    }
}
