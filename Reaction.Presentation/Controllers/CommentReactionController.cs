using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using InfinityNetServer.Services.Reaction.Application.Services;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;

namespace InfinityNetServer.Services.Reaction.Presentation.Controllers
{
    [ApiController]
    [Route("comment-reaction")]
    public class CommentReactionController(ICommentReactionService service) : ControllerBase
    {
        private readonly ICommentReactionService _service = service;
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddCommentReactionRequest request)
        {
            try
            {
                var model = await _service.CreateAsync(request);
                return Ok(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã có lỗi xảy ra" + ex.Message.ToString());
            }
            return BadRequest(new { Message = "Lỗi không xác định" });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string lstCommentId)
        {
            var request = await _service.GetCommandReaction(lstCommentId);
            return Ok(request);
        }
    }
}