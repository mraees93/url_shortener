using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using url_shortener.Services;
using Xunit;

namespace url_shortener.Tests
{
    public class UrlShortenerTests
    {
        private readonly UrlShortenerService _service;

        public UrlShortenerTests()
        {
            // Setup: Create a fresh service for every test
            _service = new UrlShortenerService();
        }

        [Fact]
        public void GenerateCode_ReturnsCorrectLength()
        {
            // Act
            var code = _service.GenerateCode();

            // Assert: Proves it meets the 6-character requirement
            Assert.Equal(6, code.Length);
        }

        [Fact]
        public void GenerateCode_IsRandom()
        {
            // Act
            var code1 = _service.GenerateCode();
            var code2 = _service.GenerateCode();

            // Assert: Ensures back-to-back codes aren't identical
            Assert.NotEqual(code1, code2);
        }

        [Theory]
        [InlineData(100)]
        public void GenerateCode_OnlyContainsAllowedCharacters(int iterations)
        {
            // Arrange
            var allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Act & Assert: Stress test the character set 100 times
            for (int i = 0; i < iterations; i++)
            {
                var code = _service.GenerateCode();
                Assert.All(code, c => Assert.Contains(c, allowedChars));
            }
        }
    }
}