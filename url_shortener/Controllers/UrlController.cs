using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using url_shortener.Database;
using url_shortener.Models;
using NanoidDotNet; 

namespace url_shortener.Controllers
{
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UrlController(AppDbContext context) => _context = context;

        [HttpPost("api/shorten")]
        public async Task<IActionResult> Shorten([FromBody] string longUrl)
        {
            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out _)) return BadRequest("Invalid URL");

            var code = Nanoid.Generate(size: 6);
            var shortUrl = new ShortUrl { LongUrl = longUrl, ShortCode = code };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return Ok(new { ShortCode = code });
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectTo(string code)
        {
            var entry = await _context.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == code);
            if (entry == null) return NotFound();

            return Redirect(entry.LongUrl);
        }
    }
}