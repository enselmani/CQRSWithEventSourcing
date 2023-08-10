using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using SharedKernel.Infrastructure;
using WebAPI.Queries;
using WebAPI.DTOs;
using SharedKernel.DTOs;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/")]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<Post> _queryDispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<Post> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
                List<Post> posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all posts!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("[action]/{postId:guid}")]
        public async Task<ActionResult> GetByPostIdAsync([FromRoute] Guid postId)
        {
            try
            {
                List<Post> posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });

                if (posts is null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = "Successfully returned post!"
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find post by ID!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetPostsByAuthorAsync([FromQuery] string author)
        {
            try
            {
                List<Post> posts = await _queryDispatcher.SendAsync(new FindPostsByAuthorQuery { Author = author });

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts by author!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
                List<Post> posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts with comments!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetPostsWithLikesAsync([FromQuery] int numberOfLikes)
        {
            try
            {
                List<Post> posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberOfLikes });

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts with likes!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        #region PRIVATE

        private ActionResult NormalResponse(List<Post> posts)
        {
            if (posts is null || !posts.Any())
            {
                return NoContent();
            }

            int count = posts.Count;

            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Successfully returned {count} post{(count > 1 ? "s" : string.Empty)}!"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.LogError(ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }

        #endregion PRIVATE
    }
}