using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Comment.Presentation.Services
{
    public static class ServiceExtentions
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
        }

    }

}
