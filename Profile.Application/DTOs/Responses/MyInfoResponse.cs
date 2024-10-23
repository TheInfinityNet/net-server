namespace InfinityNetServer.Services.Profile.Application.DTOs.Responses
{
    // các dto này tạo tùy theo bên frontend cần gì
    // khoan cai ong giờ ông push lên thử xem
    //oker
    public class MyInfoResponse
    {
        // giả sử cần nhiêu thôi
        public string ProfileId { get; set; }

        // nếu sai tên nó ra null
        // giả bộ tui muốn custom 1 vài thuộc tính khác tên
        public string Firstnameee { get; set; }

        public string Lastname { get; set; }

        // sẽ có cách dùng thư viện cho nó tự set cácacshuoojc tính giữa dto và entity
        // hay nói đubng hơn là nó map các thuohc tính giữa 2 class khác nhau miên sao cấc thuộc tinh sphair trùng tên

    }
}
