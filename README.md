# BuildingBlocks/: 
- folder chứa 4 project base (common) dùng chung cho tất cả service.

# Services/: 
- Folder chứa các microservices.
- Mỗi microservice có 4 project:
  - ## |MicroserviceName|.Application:
    - DTOs/: tạo dto requests/responses/... cho microservice hiện tại.
    - Services/: tạo interface cho bussiness service của microservice hiện tại.
    - GrpcServices/: tạo class implement các grpc server method của microservice hiện tại trong các file .proto.
    - GrpcClient/: tạo grpc client gọi đến các grpc server khác.
    - Consumers/: tạo consumer cho microservice hiện tại để lắng nghe (consume) message từ các microservice khác qua RabbitMQ.
    - Exceptions/: tạo error code và exception cho service hiện tại.
    - Resources/: tạo file .resx chứa các message string đa ngôn ngữ của microservice hiện tại.
    - Helpers/: tạo class static method dùng chung nội bộ trong microservice hiện tại.

  - ## |MicroserviceName|.Domain:
    - Entities/: tạo entities (tables) ánh xạ xuống db của microservice hiện tại.
    - Enums/: tạo enums cần dùng trong microservice hiện tại.
    - Repositories/: tạo interface cho các db repositories của microservice hiện tại.

  - ## |MicroserviceName|.Infrastructure:
    - Data/: chứa db context và các db migrations của microservice hiện tại.
    - Repositories/: tạo class implement lại các interface repo ở tầng Domain.
    - DependencyInjection/: tạo class static method để inject các db context, repositories của microservice hiện tại. 

  - ## |MicroserviceName|.Presentation:
    - Configurations/: chứa các class, method gọi đến BuildingBlocks hoặc tầng Infrastructure để inject các cấu hình cần thiết cho microservice hiện tại.
    - Services/: tạo class implement lại các interface service ở tầng Application.
    - Exceptions/: tạo middleware để handle các exception đã định nghĩa ở tầng Application của microservice hiện tại.
    - Mappers/: tạo các ánh xạ nhanh giữa dto và proto message, giữa dto và entities,...
    - Controllers/: tạo class chứa rest api theo chức năng (bussiness) của microservice hiện tại.

# Lưu ý: 
  - Những class hay method nào trong các microservices mà cần dùng chung cho nhiều hơn 1 microservices khác nhau thì chuyển vào BuildingBlocks theo cấu trúc thư mục tương ứng.
  - Những file protobuf .proto thì chỉ tạo trong BuildingBlocks.Application/Protos/.
  - Run những project |MicroserviceName|.Presentation cần cho chức năng mình làm, không cần run hết sau đó run Ocelot.Presentation
  - http://localhost:60000/swagger/index.html

# Cách start backend:
  - Ở thư mục root, mở terminal chạy "docker-compose -f docker-compose.yml up -d" (lần đầu).
  - Mở file .sln trong thư mục root rồi run các |MicroserviceName|.Presentation cần thiết.
