using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetrySample.Entities;
using OpenTelemetrySample.Services.Abstractions;
using System.Diagnostics.Metrics;

namespace OpenTelemetrySample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPlaceholderService _placeholderService;
        private readonly IConfiguration _configuration;
        private readonly Counter<long> _sampleCounter = Meters
            .SampleMeter.CreateCounter<long>("SampleCounter");

        public PostController(
            AppDbContext dbContext,
            IPlaceholderService placeholderService,
            IConfiguration configuration
        )
        {
            _dbContext = dbContext;
            _placeholderService = placeholderService;
            _configuration = configuration;
        }

        [HttpGet("env")]
        public async Task<IActionResult> GetEnv(CancellationToken ct)
        {
            _sampleCounter.Add(1, new("name", "apple"), new("color", "red"));

            await Task.CompletedTask;

            return Ok(Environment.GetEnvironmentVariables());
        }

        [HttpPost]
        public async Task<IActionResult> Send(CancellationToken ct)
        {
            _sampleCounter.Add(1);

            var rs = await _dbContext.Posts.ToListAsync(ct);

            return Ok(new { rs.Count, Item = rs });
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            _sampleCounter.Add(1);
            var rs = await _placeholderService.GetPosts(ct);

            var posts = rs.Select(r => new Post
            {
                UserId = r.UserId,
                Title = r.Title,
                Body = r.Body,
            });

            await _dbContext.AddRangeAsync(posts, ct);
            await _dbContext.SaveChangesAsync(ct);

            return Ok(rs);
        }
    }
}