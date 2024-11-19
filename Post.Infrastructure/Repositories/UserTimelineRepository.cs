using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Repositories
{
    public class UserTimelineRepository(TimelineDbContext dbContext)
        : MongoDbGenericRepository<UserTimeline, Guid>(dbContext), IUserTimelineRepository
    {
        public async Task<CursorPagedResult<TimelinePost>> GetUserTimelineAsync(Guid profileId, string cursor, int pageSize)
        {
            var filter = Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId);

            var projection = Builders<UserTimeline>.Projection.Expression(userTimeline =>
                new
                {
                    Timeline = cursor == null
                        ? userTimeline.Posts.Take(pageSize)
                        : userTimeline.Posts
                            .Where(post => string.Compare(post.Id.ToString(), cursor) < 0)
                            .Take(pageSize)
                });

            var result = await _collection
                .Find(filter)
                .Project(projection)
                .FirstOrDefaultAsync();

            if (result == null || !result.Timeline.Any())
                return new CursorPagedResult<TimelinePost>();

            var posts = result.Timeline.ToList();
            var nextCursor = posts.Count < pageSize ? null : posts.Last().Id.ToString();

            return new CursorPagedResult<TimelinePost>
            {
                Items = posts,
                NextCursor = nextCursor
            };
        }

        public async Task PushPostToTimelineAsync(Guid profileId, TimelinePost post)
        {
            // Lấy document hiện tại theo profileId
            var filter = Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId);
            var userTimeline = await _collection.Find(filter).FirstOrDefaultAsync();

            // Nếu không tồn tại thì khởi tạo
            var posts = userTimeline?.Posts ?? [];

            // Thêm post mới và sắp xếp theo created_at
            posts.Add(post);
            posts = posts.OrderBy(p => p.CreatedAt).Take(100).ToList(); // Giữ tối đa 100 bài mới nhất

            // Update document
            var update = Builders<UserTimeline>.Update
                .Set(ut => ut.Posts, posts) // Cập nhật danh sách bài viết đã sắp xếp
                .Set(ut => ut.UpdatedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task UpdateParentIdAsync(Guid profileId, Guid postId, Guid? parentId)
        {
            // Tìm bài đăng theo postId và cập nhật parentId
            var filter = Builders<UserTimeline>.Filter.And(
                Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId),
                Builders<UserTimeline>.Filter.ElemMatch(ut => ut.Posts, p => p.Id == postId)
            );

            var update = Builders<UserTimeline>.Update.Set(p => p.Posts[-1].ParentId, parentId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeletePostFromTimelineAsync(Guid profileId, Guid postId)
        {
            // Tìm document theo profileId và xóa bài đăng
            var filter = Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId);
            var update = Builders<UserTimeline>.Update.PullFilter(
                ut => ut.Posts,
                Builders<TimelinePost>.Filter.Eq(p => p.Id, postId)
            );

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<bool> IsExists(Guid profileId)
        {
            var filter = Builders<UserTimeline>.Filter.Eq(ut => ut.ProfileId, profileId);
            return await _collection.Find(filter).AnyAsync();
        }

    }
}
