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
using url_shortener.Services;
using Microsoft.AspNetCore.RateLimiting;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UrlShortenerService _service;

        public UrlController(AppDbContext context, UrlShortenerService service)
        {
            _context = context;
            _service = service;
        }

        [HttpPost("shorten")]
        [EnableRateLimiting("fixed")] // The policy i named in Program.cs
        public async Task<IActionResult> Shorten([FromBody] string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _)) return BadRequest("Invalid URL");

            var code = _service.GenerateCode();
            var newUrl = new ShortUrl { LongUrl = url, ShortCode = code };

            _context.ShortUrls.Add(newUrl);
            await _context.SaveChangesAsync();

            return Ok(new { shortCode = code });
        }

        [HttpGet("/{code}")]
        public async Task<IActionResult> RedirectTo(string code)
        {
            var entry = await _context.ShortUrls
                .FirstOrDefaultAsync(u => u.ShortCode == code);

            if (entry == null)
            {
                return NotFound("Short URL not found.");
            }

            // Log the click
            var click = new ClickEvent
            {
                ShortCode = code,
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
            _context.ClickEvents.Add(click);
            await _context.SaveChangesAsync();

            return Redirect(entry.LongUrl);
        }

        [HttpGet("stats/{code}")]
        public async Task<IActionResult> GetStats(string code)
        {
            var count = await _context.ClickEvents.CountAsync(c => c.ShortCode == code);
            return Ok(new { clickCount = count });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            // Fetch last 10 links, ordered by newest first
            var history = await _context.ShortUrls
                .OrderByDescending(u => u.Id)
                .Take(10)
                .ToListAsync();

            return Ok(history);
        }
    }
}