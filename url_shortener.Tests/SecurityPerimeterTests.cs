using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using url_shortener.Services;
using Xunit;

namespace url_shortener.Tests
{
    public class SecurityPerimeterTests
    {
        private readonly SecurityPerimeterService _service;

        public SecurityPerimeterTests()
        {
            var mockLogger = new Mock<ILogger<SecurityPerimeterService>>();
            _service = new SecurityPerimeterService(mockLogger.Object);
        }

        [Theory]
        [InlineData("https://google.com")]
        [InlineData("https://github.com")]
        [InlineData("https://stackoverflow.com")]
        public async Task IsUrlSafeAsync_StandardValidDomains_ReturnsTrue(string safeUrl)
        {
            var result = await _service.IsUrlSafeAsync(safeUrl);
            Assert.True(result);
        }

        [Theory]
        [InlineData("http://paypal-security-update-login-verify.com")]
        [InlineData("https://secure-bank-signin-spoof.ru")]
        [InlineData("http://claim-reward-free-crypto.tk")]
        [InlineData("https://malicious-payload-drop.zip")]
        public async Task IsUrlSafeAsync_HighRiskThreatVectors_ReturnsFalse(string maliciousUrl)
        {
            var result = await _service.IsUrlSafeAsync(maliciousUrl);
            Assert.False(result);
        }
    }
}
