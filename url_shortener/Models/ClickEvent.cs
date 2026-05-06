using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace url_shortener.Models
{
    public class ClickEvent
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ShortCode { get; set; } = string.Empty;

        [Column(TypeName = "timestamp with time zone")]
        public DateTime ClickedAt { get; set; } = DateTime.UtcNow;
        public string? UserAgent { get; set; }
    }
}