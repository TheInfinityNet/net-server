using System;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Responses
{
    // các dto này tạo tùy theo bên frontend cần gì
    // khoan cai ong giờ ông push lên thử xem
    //oker
    public class MyInfoResponse
    {
        public string ProfileId { get; set; }
        public string AccountId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string CoverPhoto { get; set; }
        public string AvatarPhoto { get; set; }
        public string Status { get; set; }
    } 
}
