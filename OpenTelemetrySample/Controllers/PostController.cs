using Microsoft.AspNetCore.Mvc;
using OpenTelemetrySample.Entities;
using OpenTelemetrySample.Services.Abstractions;
using System.Diagnostics;

namespace OpenTelemetrySample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPlaceholderService _placeholderService;

        public PostController(
            AppDbContext dbContext,
            IPlaceholderService placeholderService
        )
        {
            _dbContext = dbContext;
            _placeholderService = placeholderService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(Post post)
        {
            await Task.CompletedTask;

            return Ok(post);
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
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