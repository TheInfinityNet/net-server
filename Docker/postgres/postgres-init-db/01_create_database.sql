-- Tạo các cơ sở dữ liệu nếu chưa tồn tại
CREATE DATABASE identity_service_db;
CREATE DATABASE profile_service_db;
CREATE DATABASE relationship_service_db;
CREATE DATABASE post_service_db;
CREATE DATABASE comment_service_db;
CREATE DATABASE reaction_service_db;
CREATE DATABASE tag_service_db;
CREATE DATABASE group_service_db;

-- Kết nối và tạo extension cho từng cơ sở dữ liệu
\c identity_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c profile_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c relationship_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c post_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c comment_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c reaction_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c tag_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

\c group_service_db
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


