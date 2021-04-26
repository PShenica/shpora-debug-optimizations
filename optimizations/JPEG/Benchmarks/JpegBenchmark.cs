using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace JPEG.Benchmarks
{
    [MemoryDiagnoser]
    public class JpegBenchmark
    {
        private Bitmap bitmap;

        [GlobalSetup]
        public void Setup()
        {
            var width = 1000;
            var height = 1000;
            bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Red);
            };
        }

        [Benchmark]
        public void PrimitiveBitmap()
        {
            for(var j = 0; j < bitmap.Height; j++)
            {
                for(var i = 0; i < bitmap.Width; i++)
                {
                    var color = bitmap.GetPixel(i, j);

                    if (color == Color.Red)
                    {
                        bitmap.SetPixel(i, j, Color.Blue);
                    }
                }
            }
        }

        [Benchmark]
        public unsafe void ImageProcess()
        {
            var bData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            var bitsPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat);
            var scan0 = (byte*)bData.Scan0.ToPointer();

            for (var i = 0; i < bData.Height; ++i)
            {
                for (var j = 0; j < bData.Width; ++j)
                {
                    var data = scan0 + i * bData.Stride + j * bitsPerPixel / 8;

                    data[2] = 0;
                    data[0] = 255;
                    //data is a pointer to the first byte of the 3-byte color data
                    //data[0] = blueComponent;
                    //data[1] = greenComponent;
                    //data[2] = redComponent;
                }
            }

            bitmap.UnlockBits(bData);
        }
    }
}