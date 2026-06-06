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
            // Simulate a brief async machine-learning classification delay
            await Task.Delay(40); 

            var lowerUrl = url.ToLower();
            int threatWeight = 0;
            string dynamicIntent = "Unclassified / Safe";

            // 1. Semantic Token Evaluation Matrix (+3 Weight per Match)
            string[] highRiskTokens = { "paypal", "login", "verify", "secure", "bank", "account", "update", "signin", "crypto", "free", "gift" };
            foreach (var token in highRiskTokens)
            {
                if (lowerUrl.Contains(token)) threatWeight += 3;
            }

            // 2. Syntax Manipulation Scoring (+2 Weight per Pattern)
            if (lowerUrl.Contains("-") && lowerUrl.Contains("login")) { threatWeight += 2; dynamicIntent = "Login Spoofing Variant"; }
            if (url.Contains("@")) { threatWeight += 4; dynamicIntent = "Credential Hijacking Pattern"; }
            if (lowerUrl.StartsWith("http://")) { threatWeight += 2; dynamicIntent = "Insecure Data Transit Protocol"; }

            // 3. High-Risk TLD Vectors (+4 Weight)
            string[] dangerousTlds = { ".zip", ".mov", ".ru", ".tk" };
            foreach (var tld in dangerousTlds)
            {
                if (lowerUrl.Contains(tld + "/") || lowerUrl.EndsWith(tld)) 
                { 
                    threatWeight += 4; 
                    dynamicIntent = "Malicious Top-Level Domain Redirect";
                }
            }

            // Calculate algorithmic probability threshold values
            double structuralRiskProbability = Math.Min((threatWeight / 12.0) * 100, 100.0);
            string safetyVerdict = structuralRiskProbability >= 50.0 ? "Malicious" : "Safe";

            _logger.LogInformation($"[AI GUARD] Local Token Classification Scan Completed for URL: {url}");
            _logger.LogInformation($"[AI GUARD] Metrics -> Classification Weight: {threatWeight}, Threat Probability: {structuralRiskProbability:F1}%, Assigned Intent: {dynamicIntent}, Verdict: {safetyVerdict}");

            // The Perimeter Boundary Block Execution Node
            if (safetyVerdict == "Malicious")
            {
                _logger.LogWarning($"[AI GUARD] FIREWALL SECURITY TRIGGERED: Blocking link based on high threat probability ({structuralRiskProbability:F1}%). Vector: {dynamicIntent}");
                return false;
            }

            return true;
        }
    }
}
