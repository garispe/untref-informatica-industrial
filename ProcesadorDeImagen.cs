using System;
using AForge.Imaging.Filters;
using System.Drawing;
using AForge.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using AForge;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Microsoft.Samples.Kinect.DepthBasics
{
    class ProcesadorDeImagen
    {
        // Canny
        public BitmapImage aplicarFiltroCanny(Bitmap bitmap, string misFotos, string hora)
        {
            Rectangle rectangulo = new Rectangle(0, 0, 640, 480);
            var foto = bitmap.Clone(rectangulo, PixelFormat.Format8bppIndexed);

            UnmanagedImage uI = UnmanagedImage.FromManagedImage(foto);

            CannyEdgeDetector filtroBordes = new CannyEdgeDetector();
            UnmanagedImage fotoConFiltro = filtroBordes.Apply(uI);
            Bitmap bitmapConFiltro = fotoConFiltro.ToManagedImage();

            string urlFotoFiltro = Path.Combine(misFotos, "Canny.bmp");

            bitmapConFiltro.Save(urlFotoFiltro);

            return new BitmapImage(new Uri(urlFotoFiltro));
        }

        // Sobel
        public BitmapImage aplicarFiltroSobel(Bitmap bitmap, string misFotos, string hora)
        {
            Rectangle rectangulo = new Rectangle(0, 0, 640, 480);
            var foto = bitmap.Clone(rectangulo, PixelFormat.Format8bppIndexed);

            UnmanagedImage uI = UnmanagedImage.FromManagedImage(foto);

            SobelEdgeDetector filtroBordes = new SobelEdgeDetector();
            UnmanagedImage fotoConFiltro = filtroBordes.Apply(uI);
            Bitmap bitmapConFiltro = fotoConFiltro.ToManagedImage();

            string urlFotoFiltro = Path.Combine(misFotos, "Sobel.bmp");

            bitmapConFiltro.Save(urlFotoFiltro);

            return new BitmapImage(new Uri(urlFotoFiltro));
        }

        // Detector de cuadrilateros
        public BitmapImage detectarCuadrilateros(Bitmap bitmap, string misFotos, string hora)
        {
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(bitmap);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            Graphics g = Graphics.FromImage(bitmap);
            Pen bluePen = new Pen(Color.Blue, 2);
            Pen redPen = new Pen(Color.Red, 2);
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners = PointsCloud.FindQuadrilateralCorners(edgePoints);

                SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    g.DrawPolygon(redPen, toPointsArray(corners));
                }
                else
                {
                    g.DrawPolygon(bluePen, toPointsArray(corners));

                }
            }

            bluePen.Dispose();
            g.Dispose();

            string urlConFormas = Path.Combine(misFotos, "ImagenConFormas.bmp");

            bitmap.Save(urlConFormas);

            return new BitmapImage(new Uri(urlConFormas));
        }

        private System.Drawing.Point[] toPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }

        // Transformar bitmap en BitmapImage
        public Bitmap bitmapFromBitmapImage(BitmapImage bitmapImage)
        {
            using (MemoryStream outStraeam = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(outStraeam);
                Bitmap bitmap = new Bitmap(outStraeam);

                return new Bitmap(bitmap);
            }
        }

        // Contraste
        public BitmapImage aplicarContraste(string path, string misFotos, string hora, int threshold)
        {
            Bitmap sourceBitmap = new Bitmap(path);
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);

            double azul = 0;
            double verde = 0;
            double rojo = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4) {
                azul = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;
                verde = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;
                rojo = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;

                if (azul > 255) {
                    azul = 255;
                } else if (azul < 0) {
                    azul = 0;
                }
                if (verde > 255) {
                    verde = 255;
                } else if (verde < 0) {
                    verde = 0;
                }
                if (rojo > 255) {
                    rojo = 255;
                } else if (rojo < 0) {
                    rojo = 0;
                }
                pixelBuffer[k] = (byte)azul;
                pixelBuffer[k + 1] = (byte)verde;
                pixelBuffer[k + 2] = (byte)rojo;
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            string urlConFormas = Path.Combine(misFotos, "Contraste.bmp");

            resultBitmap.Save(urlConFormas);

            return new BitmapImage(new Uri(urlConFormas));
        }

        // Bitonal
        public BitmapImage Bitonal(string path, string misFotos, string hora, 
            Color darkColor, Color lightColor, int threshold) {

            Bitmap sourceBitmap = new Bitmap(path);
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, 
                sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            
            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4) {

                if (pixelBuffer[k] + pixelBuffer[k + 1] + pixelBuffer[k + 2] <= threshold) {
                    pixelBuffer[k] = darkColor.B;
                    pixelBuffer[k + 1] = darkColor.G;
                    pixelBuffer[k + 2] = darkColor.R;
                } else {
                    pixelBuffer[k] = lightColor.B;
                    pixelBuffer[k + 1] = lightColor.G;
                    pixelBuffer[k + 2] = lightColor.R;
                }
            }
            
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            string urlConFormas = Path.Combine(misFotos, "Bitonal.bmp");

            resultBitmap.Save(urlConFormas);

            return new BitmapImage(new Uri(urlConFormas));
        }

        // Gamma
        private byte[] CreateGammaArray(double color)
        {
            byte[] gammaArray = new byte[256];
            for (int i = 0; i < 256; ++i) {
                gammaArray[i] = (byte)Math.Min(255,
                    (int)((255.0 * Math.Pow(i / 255.0, 1.0 / color)) + 0.5));
            }
            return gammaArray;
        }

        public BitmapImage gamma(string path, string misFotos, string hora
            , double red, double green, double blue)
        {
            Bitmap bitmap = new Bitmap(path);
            Bitmap temp = (Bitmap)bitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            byte[] redGamma = CreateGammaArray(red);
            byte[] greenGamma = CreateGammaArray(green);
            byte[] blueGamma = CreateGammaArray(blue);
            for (int i = 0; i < bmap.Width; i++) {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    bmap.SetPixel(i, j, Color.FromArgb(redGamma[c.R],
                       greenGamma[c.G], blueGamma[c.B]));
                }
            }
            bitmap = (Bitmap)bmap.Clone();

            string urlConFormas = Path.Combine(misFotos, "Gamma.bmp");

            bitmap.Save(urlConFormas);

            return new BitmapImage(new Uri(urlConFormas));
        }

        // Escala gris
        public BitmapImage aplicarGrises(string path, string misFotos, string hora)
        {

            Bitmap bitmap = new Bitmap(path);
            Bitmap temp = bitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            bitmap = (Bitmap)bmap.Clone();

            string urlConFormas = Path.Combine(misFotos, "Grises.bmp");

            bitmap.Save(urlConFormas);

            return new BitmapImage(new Uri(urlConFormas));
        }

        // Hough
        public BitmapImage aplicarHough(string path, string misFotos)
        {
            Bitmap bitmap = new Bitmap(path);
            HoughLineTransformation lineTransform = new HoughLineTransformation();
            // apply Hough line transofrm
            lineTransform.ProcessImage(bitmap);
            Bitmap houghLineImage = lineTransform.ToBitmap();
            // get lines using relative intensity
            HoughLine[] lines = lineTransform.GetLinesByRelativeIntensity(0.6);

            foreach (HoughLine line in lines)
            {
                // get line's radius and theta values
                int r = line.Radius;
                double t = line.Theta;
                
                // check if line is in lower part of the image
                if (r < 0)
                {
                    t += 180;
                    r = -r;
                }

                // convert degrees to radians
                t = (t / 180) * Math.PI;

                // get image centers (all coordinate are measured relative
                // to center)
                int w2 = bitmap.Width / 2;
                int h2 = bitmap.Height / 2;

                double x0 = 0, x1 = 0, y0 = 0, y1 = 0;

                if (line.Theta != 0)
                {
                    // none-vertical line
                    x0 = -w2; // most left point
                    x1 = w2;  // most right point

                    // calculate corresponding y values
                    y0 = (-Math.Cos(t) * x0 + r) / Math.Sin(t);
                    y1 = (-Math.Cos(t) * x1 + r) / Math.Sin(t);
                }
                else
                {
                    // vertical line
                    x0 = line.Radius;
                    x1 = line.Radius;

                    y0 = h2;
                    y1 = -h2;
                }

                // draw line on the image
                BitmapData bmp = new BitmapData();

                Rectangle rec = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rec, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                bitmap.UnlockBits(bmpData);

                Drawing.Line(bmpData,
                    new IntPoint((int)x0 + w2, h2 - (int)y0),
                    new IntPoint((int)x1 + w2, h2 - (int)y1),
                    Color.White);
            }

            string urlConFormas = Path.Combine(misFotos, "Hough.bmp");

            bitmap.Save(urlConFormas);

            return new BitmapImage(new Uri(urlConFormas));
        }

    }
}
