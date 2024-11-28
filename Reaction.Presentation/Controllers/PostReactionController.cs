using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Presentation.Controllers
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
                var model = await _service.Create(request);
                return Ok(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã có lỗi xảy ra" + ex.Message.ToString());
            }
            return BadRequest(new { Message = "Lỗi không xác định" });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string lstPostId)
        {
            var request = await _service.GetPostReactions(lstPostId);
            return Ok(request);
        }
    }
}