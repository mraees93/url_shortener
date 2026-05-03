using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace url_shortener.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}