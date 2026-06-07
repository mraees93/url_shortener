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
        private readonly SecurityPerimeterService _aiGuard;

        public UrlController(AppDbContext context, UrlShortenerService service, SecurityPerimeterService aiGuard)
        {
            _context = context;
            _service = service;
            _aiGuard = aiGuard;
        }

        [HttpPost("shorten")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Shorten([FromBody] string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                return BadRequest("Invalid URL");

            // AI FIREWALL CHECK: Scans the target link before database execution
            var isSafe = await _aiGuard.IsUrlSafeAsync(url);
            if (!isSafe)
            {
                return BadRequest(new { message = "Security Perimeter: This destination URL has been blocked as a high-risk security threat." });
            }

            var code = _service.GenerateCode();
            var newUrl = new ShortUrl { LongUrl = url, ShortCode = code };

            _context.ShortUrls.Add(newUrl);
            await _context.SaveChangesAsync();

            var host = Request.Host.Value;
            var protocol = Request.Scheme;

            var response = new
            {
                id = newUrl.Id,
                shortCode = code,
                longUrl = url,
                shortUrl = $"{protocol}://{host}/{code}"
            };

            return Ok(response);
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
            var host = Request.Host.Value;
            var protocol = Request.Scheme;

            var history = await _context.ShortUrls
                .OrderByDescending(u => u.Id)
                .Take(10)
                .Select(u => new
                {
                    id = u.Id,
                    u.ShortCode,
                    u.LongUrl,
                    // This ensures the frontend has the full clickable link
                    shortUrl = $"{protocol}://{host}/{u.ShortCode}"
                })
                .ToListAsync();

            return Ok(history);
        }

        [HttpDelete("history/{id}")]
        public async Task<IActionResult> DeleteUrlById(string id)
        {
            var ShortUrl = await _context.ShortUrls.FindAsync(id);
            if (ShortUrl == null)
            {
                return NotFound(new { message = "Link not found or already deleted" });
            }

            _context.ShortUrls.Remove(ShortUrl);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}