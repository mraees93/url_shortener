using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace url_shortener.Services
{
    public class SecurityPerimeterService
    {
        private readonly ILogger<SecurityPerimeterService> _logger;

        public SecurityPerimeterService(ILogger<SecurityPerimeterService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> IsUrlSafeAsync(string url)
        {
            await Task.Delay(40); 

            var lowerUrl = url.ToLower();
            int threatWeight = 0;
            string dynamicIntent = "Unclassified / Safe";

            string[] highRiskTokens = { "paypal", "login", "verify", "secure", "bank", "account", "update", "signin", "crypto", "free", "gift" };
            foreach (var token in highRiskTokens)
            {
                if (lowerUrl.Contains(token)) threatWeight += 3;
            }

            if (lowerUrl.Contains("-") && lowerUrl.Contains("login")) { threatWeight += 2; dynamicIntent = "Login Spoofing Variant"; }
            if (url.Contains("@")) { threatWeight += 4; dynamicIntent = "Credential Hijacking Pattern"; }
            if (lowerUrl.StartsWith("http://")) { threatWeight += 2; dynamicIntent = "Insecure Data Transit Protocol"; }

            string[] dangerousTlds = { ".zip", ".mov", ".ru", ".tk" };
            foreach (var tld in dangerousTlds)
            {
                if (lowerUrl.Contains(tld + "/") || lowerUrl.EndsWith(tld)) 
                { 
                    threatWeight += 4; 
                    dynamicIntent = "Malicious Top-Level Domain Redirect";
                }
            }

            double structuralRiskProbability = Math.Min(((double)threatWeight / 12.0) * 100, 100.0);
            
            string safetyVerdict = structuralRiskProbability >= 30.0 ? "Malicious" : "Safe";

            _logger.LogInformation($"[SECURITY PERIMETER] Local Token Classification Scan Completed for URL: {url}");
            _logger.LogInformation($"[SECURITY PERIMETER] Metrics -> Classification Weight: {threatWeight}, Threat Probability: {structuralRiskProbability:F1}%, Assigned Intent: {dynamicIntent}, Verdict: {safetyVerdict}");

            if (safetyVerdict == "Malicious")
            {
                _logger.LogWarning($"[SECURITY PERIMETER] FIREWALL SECURITY TRIGGERED: Blocking link based on high threat probability ({structuralRiskProbability:F1}%). Vector: {dynamicIntent}");
                return false;
            }

            return true;
        }
    }
}
