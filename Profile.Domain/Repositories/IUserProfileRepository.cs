using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IUserProfileRepository : ISqlRepository<UserProfile, Guid>
    {

        //Định nghĩa hàm repository
        // lý do mình ko gọi repository trức tiếp trong controller:
        // tách biệt logic vì trong service ngoài gọi truy vấn có thể sẽ làm những việc khác
        // ông hình dung:
        // tầng DAL <==> repository
        // tầng BLL(BUS) <==> service
        // tầng GUI <==> controller (ở đây là api thay vì view)
        Task<UserProfile> GetGetUserProfileByAccountIdAsync(Guid accountId);

    }
}
