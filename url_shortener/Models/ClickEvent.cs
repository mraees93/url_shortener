using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace url_shortener.Models
{
    public class ClickEvent
    {
        public int Id { get; set; }
        public string ShortCode { get; set; } = string.Empty;
        public DateTime ClickedAt { get; set; } = DateTime.UtcNow;
        public string? UserAgent { get; set; }
    }
}