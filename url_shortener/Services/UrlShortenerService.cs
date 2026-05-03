using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace url_shortener.Services
{
    public class UrlShortenerService
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly Random Random = new();

    public string GenerateCode()
    {
        // Generates a 6-character random string (Base62)
        var chars = new char[6];
        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = Alphabet[Random.Next(Alphabet.Length)];
        }
        return new string(chars);
    }
    }
}