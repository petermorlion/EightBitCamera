﻿using NUnit.Framework;
using EightBitCamera;

namespace UnitTests
{
    [TestFixture]
    public class PixelatorTests
    {
        private Pixelator _pixelator;

        [Test]
        public void GivenAPixelatorWithPixelSizeEight()
        {
            _pixelator = new Pixelator(4);

            var buffer = new[]
                             {
                                 0, 1, 2, 3, 4, 5, 6, 7, 
                                 8, 9, 10, 11, 12, 13, 14, 15, 
                                 16, 17, 18, 19, 20, 21, 22, 23, 
                                 24, 25, 26, 27, 28, 29, 30, 31, 
                                 32, 33, 34, 35, 36, 37, 38, 39, 
                                 40, 41, 42, 43, 44, 45, 46, 47, 
                                 48, 49, 50, 51, 52, 53, 54, 55, 
                                 56, 57, 58, 59, 60, 61, 62, 63
                             };

            var result = _pixelator.Pixelate(buffer, 8, 8);

            var expectedBuffer = new[]
                             {
                                 18, 18, 18, 18, 22, 22, 22, 22,
                                 18, 18, 18, 18, 22, 22, 22, 22,
                                 18, 18, 18, 18, 22, 22, 22, 22, 
                                 18, 18, 18, 18, 22, 22, 22, 22, 
                                 50, 50, 50, 50, 54, 54, 54, 54,
                                 50, 50, 50, 50, 54, 54, 54, 54,
                                 50, 50, 50, 50, 54, 54, 54, 54,
                                 50, 50, 50, 50, 54, 54, 54, 54
                             };

            Assert.AreEqual(expectedBuffer, result);
        }
    }
}
