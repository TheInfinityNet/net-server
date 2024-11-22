using Microsoft.AspNetCore.Mvc;
using InfinityNetServer.Application.Post.Presentation.DTOs.Requests;
using InfinityNetServer.Application.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace InfinityNetServer.Presentation.Controller
{
    [ApiController]
    [Route("post-reaction")]
    public class PostReactionController(IPostReactionService service) : ControllerBase
    {
        private readonly IPostReactionService _service = service;
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddPostReactionRequest request)
        {
            try
            {
                var model = await _service.CreatePostReaction(request);
                return Ok(model);
            }catch(Exception ex) {
                Console.WriteLine("Đã có lỗi xảy ra" + ex.Message.ToString());
            }
            return BadRequest(new {Message = "Lỗi không xác định" });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string lstPostId)
        {
            var request = await _service.GetPostReactions(lstPostId);
            return Ok(request);
        }
    }
}