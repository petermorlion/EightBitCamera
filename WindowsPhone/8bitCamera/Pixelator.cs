using System;

namespace EightBitCamera
{
    public class Pixelator
    {
        private readonly int _pixelateSize;

        public Pixelator(int pixelateSize)
        {
            _pixelateSize = pixelateSize;
        }

        public int[] Pixelate(int[] buffer, int width, int height)
        {
            // 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8
            // 9 , 10, 11, 12, 13, 14, 15, 16
            // 17, 18, 19, 20, 21, 22, 23, 24
            // 25, 26, 27, 28, 29, 30, 31, 32
            // 33, 34, 35, 36, 37 ,38, 39, 40
            // 41, 42, 43, 44, 45, 46, 47, 48
            // 49, 50, 51, 52, 53, 54, 55, 56
            // 57, 58, 59, 60, 61, 62, 63, 64

            // 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8, 9 , 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 ,38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64

            // 19, 19, 19, 19, 23, 23, 23, 23
            // 19, 19, 19, 19, 23, 23, 23, 23
            // 19, 19, 19, 19, 23, 23, 23, 23
            // 19, 19, 19, 19, 23, 23, 23, 23
            // 51, 51, 51, 51, 55, 55, 55, 55
            // 51, 51, 51, 51, 55, 55, 55, 55
            // 51, 51, 51, 51, 55, 55, 55, 55
            // 51, 51, 51, 51, 55, 55, 55, 55

            // 19, 19, 19, 19, 23, 23, 23, 23, 19, 19, 19, 19, 23, 23, 23, 23, 19, 19, 19, 19, 23, 23, 23, 23, 19, 19, 19, 19, 23, 23, 23, 23, 51, 51, 51, 51, 55, 55, 55, 55, 51, 51, 51, 51, 55, 55, 55, 55, 51, 51, 51, 51, 55, 55, 55, 55, 51, 51, 51, 51, 55, 55, 55, 55

            // 12 -> 22
            // 13 -> 22 
            // 22 -> 22
            // 23 -> 22
            // 28 -> 22
            for (int i = 0; i < width * height; i++)
            {
                var y = (int)((double)i / width);
                var offsetX = 0;
                var referencePixel = 0;

                var x = 0;

                var isNewPixelatedLine = y % _pixelateSize == 0;

                if (isNewPixelatedLine)
                {
                    x = i + (_pixelateSize / 2) - (i % _pixelateSize);
                    var offsetY = y % _pixelateSize;
                    offsetX = offsetY + (_pixelateSize / 2) - (offsetY % _pixelateSize);
                    referencePixel = x + (offsetX * width);
                }
                else
                {
                    referencePixel = i - ((y % _pixelateSize) * width);
                }
                
                // TODO: do something with last line(s)
                if (referencePixel < buffer.Length)
                {
                    buffer[i] = buffer[referencePixel];
                }
            }

            return buffer;
        }
    }
}