namespace EightBitCamera
{
    public class Pixelator
    {
        /// <summary>
        /// Creates a new Pixelator instance.
        /// </summary>
        /// <param name="pixelateSize">The size of the pixels.</param>
        /// <param name="sourceIsCameraPreview">
        /// Set to true if the source will be the camera preview. When using the camera preview, the given buffer is used, because otherwise we get
        /// strange lines in our image. For an existing image, we need a new buffer so we can 'cache' the original image bytes.
        /// </param>
        public Pixelator(int pixelateSize, bool sourceIsCameraPreview)
        {
            PixelateSize = pixelateSize;
            SourceIsCameraPreview = sourceIsCameraPreview;
        }

        public int PixelateSize { get; set; }

        public bool SourceIsCameraPreview { get; set; }

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

            var newBuffer = SourceIsCameraPreview ? buffer : new int[buffer.Length];

            for (int i = 0; i < width * height; i++)
            {
                var y = (int)((double)i / width);
                var offsetX = 0;
                var referencePixel = 0;

                var x = 0;

                var isNewPixelatedLine = y % PixelateSize == 0;

                if (isNewPixelatedLine)
                {
                    x = i + (PixelateSize / 2) - (i % PixelateSize);
                    var offsetY = y % PixelateSize;
                    offsetX = offsetY + (PixelateSize / 2) - (offsetY % PixelateSize);
                    referencePixel = x + (offsetX * width);
                }
                else
                {
                    referencePixel = i - ((y % PixelateSize) * width);
                }
                
                // TODO: do something with last line(s)
                if (referencePixel < buffer.Length)
                {
                    newBuffer[i] = buffer[referencePixel];
                }
            }

            return newBuffer;
        }
    }
}