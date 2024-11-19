using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class UserTimelineRepository(TimelineDbContext dbContext)
        : MongoDbGenericRepository<UserTimeline, Guid>(dbContext), IUserTimelineRepository
    {
        public async Task<BCursorPagedResult<TimelinePost>> GetUserTimelineAsync(Guid profileId, string? cursor, int pageSize)
        {
            var filter = Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId);

            var projection = Builders<UserTimeline>.Projection.Expression(userTimeline =>
                new
                {
                    Timeline = cursor == null
                        ? userTimeline.Posts.Take(pageSize)
                        : userTimeline.Posts
                            .Where(post => string.Compare(post.PostId.ToString(), cursor) < 0)
                            .Take(pageSize)
                });

            var result = await _collection
                .Find(filter)
                .Project(projection)
                .FirstOrDefaultAsync();

            if (result == null || !result.Timeline.Any())
                return new BCursorPagedResult<TimelinePost>();

            var posts = result.Timeline.ToList();
            var nextCursor = posts.Count < pageSize ? null : posts.Last().PostId.ToString();

            return new BCursorPagedResult<TimelinePost>
            {
                Items = posts,
                NextCursor = nextCursor
            };
        }

        public async Task UpdateUserTimelineAsync(Guid profileId, TimelinePost post)
        {
            // Filter to find the user timeline by profileId
            var filter = Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId);

            // Update to add the new post and keep only the latest 100 posts
            var update = Builders<UserTimeline>.Update
                .PushEach(ut => ut.Posts,[ post ], slice: -100) // Push new post, keep last 100
                .Set(ut => ut.UpdatedAt, DateTime.UtcNow);

            // Perform the update
            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task<bool> IsExists(Guid profileId)
        {
            var filter = Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId);
            return await _collection.Find(filter).AnyAsync();
        }

    }
}
