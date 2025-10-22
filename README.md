# UTC2OJ - Online Judge for Contests

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-ISC-blue.svg)](LICENSE)

> Hệ thống quản lý contest lập trình trực tuyến cho trường đại học, xây dựng theo kiến trúc Microservices với .NET 9.0 và .NET Aspire.

## 📋 Mục lục

- [Giới thiệu](#giới-thiệu)
- [Kiến trúc hệ thống](#kiến-trúc-hệ-thống)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cài đặt](#cài-đặt)
- [Chạy ứng dụng](#chạy-ứng-dụng)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [API Endpoints](#api-endpoints)
- [Đóng góp](#đóng-góp)
- [License](#license)

## 🎯 Giới thiệu

UTC2OJ là hệ thống Online Judge được thiết kế để hỗ trợ việc tổ chức các kỳ thi lập trình, contest và bài tập coding cho sinh viên. Hệ thống cung cấp các tính năng:

- ✅ Quản lý người dùng (đăng ký, đăng nhập, phân quyền)
- ✅ Quản lý bài tập lập trình (problems)
- ✅ Nộp và chấm bài tự động (submission & execution)
- ✅ Tổ chức contest (cuộc thi lập trình)
- ✅ Bảng xếp hạng (leaderboard)
- ✅ Hỗ trợ nhiều ngôn ngữ lập trình

## 🏗️ Kiến trúc hệ thống

Hệ thống được xây dựng theo kiến trúc **Microservices** với các service độc lập:

```
┌─────────────────┐
│   API Gateway   │ ← Nginx Reverse Proxy
│   (Port 5000)   │
└────────┬────────┘
         │
    ┌────┴────┬──────────┬──────────┬──────────┐
    │         │          │          │          │
┌───▼───┐ ┌──▼──┐  ┌────▼────┐ ┌───▼───┐ ┌───▼────┐
│ User  │ │Problem│ │Submission│ │Execution│ │Contest │
│Service│ │Service│ │ Service  │ │Service│ │Service │
│:5001  │ │:5002 │ │  :5003   │ │ :5004 │ │ :5005  │
└───┬───┘ └──┬──┘  └────┬─────┘ └───┬───┘ └───┬────┘
    │        │          │           │         │
┌───▼───┐ ┌──▼──┐  ┌────▼────┐ ┌───▼───┐ ┌───▼────┐
│UserDB │ │ProbDB│ │SubmitDB │ │ExecDB │ │ContestDB│
│:5432  │ │:5433│ │  :5434  │ │:5435  │ │ :5436  │
└───────┘ └─────┘  └─────────┘ └───────┘ └────────┘
                        │           │
                        └─────┬─────┘
                          ┌───▼────┐
                          │RabbitMQ│
                          │:5672   │
                          └────────┘
```

### Các Service chính:

- **API Gateway**: Điểm truy cập duy nhất, routing requests đến các service
- **UserService**: Quản lý người dùng, authentication, authorization
- **ProblemService**: Quản lý bài tập lập trình (CRUD problems, test cases)
- **SubmissionService**: Nhận và quản lý bài nộp từ user
- **ExecutionService**: Thực thi và chấm điểm code submissions
- **ContestService**: Tổ chức và quản lý các contest

### Message Queue:

- **RabbitMQ**: Xử lý async giữa SubmissionService ↔ ExecutionService

## 🛠️ Công nghệ sử dụng

### Backend
- **.NET 9.0** - Framework chính
- **.NET Aspire** - Orchestration và cloud-native development
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core** - ORM
- **PostgreSQL 15** - Database cho mỗi service
- **RabbitMQ** - Message broker
- **Docker & Docker Compose** - Containerization

### Tools & Libraries
- **Swagger/OpenAPI** - API Documentation
- **Nginx** - Reverse Proxy & Load Balancer
- **Husky** - Git hooks
- **Just** - Task runner

## 💻 Yêu cầu hệ thống

Để phát triển và chạy ứng dụng, bạn cần:

- **.NET SDK 9.0** hoặc cao hơn
- **Docker Desktop** (để chạy với Docker Compose)
- **PostgreSQL 15** (nếu chạy local không dùng Docker)
- **RabbitMQ** (nếu chạy local không dùng Docker)
- **Node.js & npm** (để chạy các scripts setup)
- **Git** (để clone repository)
- **Visual Studio 2022** hoặc **VS Code** + C# Extension

## 📦 Cài đặt

### 1. Clone repository

```bash
git clone https://github.com/bientranngoc/utc2oj.git
cd utc2oj
```

### 2. Cài đặt dependencies

```bash
# Cài đặt npm packages (bao gồm Husky)
npm install

# Restore .NET packages
dotnet restore
```

### 3. Cấu hình môi trường

Tạo file `appsettings.Development.json` trong mỗi service (nếu chưa có) với nội dung:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=yourdb;Username=postgres;Password=yourpass"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest"
  }
}
```

## 🚀 Chạy ứng dụng

### Option 1: Chạy với Docker Compose (Khuyến nghị)

```bash
# Build và chạy tất cả services
cd deploy
docker-compose up --build

# Hoặc chạy ở background
docker-compose up -d --build
```

Sau khi chạy, các service sẽ có sẵn tại:
- API Gateway: http://localhost:5000
- User Service: http://localhost:5001
- Problem Service: http://localhost:5002
- Submission Service: http://localhost:5003
- Execution Service: http://localhost:5004
- Contest Service: http://localhost:5005
- RabbitMQ Management: http://localhost:15672 (guest/guest)

### Option 2: Chạy với .NET Aspire (Development)

```bash
# Chạy AppHost với Aspire
cd src/Aspire/UTC2OJ.AppHost
dotnet run
```

Aspire Dashboard sẽ mở tại: https://localhost:17148

### Option 3: Chạy từng service riêng lẻ (Local Development)

```bash
# Terminal 1 - User Service
cd src/UserService
dotnet run

# Terminal 2 - Problem Service
cd src/ProblemService
dotnet run

# Terminal 3 - Submission Service
cd src/SubmissionService
dotnet run

# Terminal 4 - Execution Service
cd src/ExecutionService
dotnet run

# Terminal 5 - Contest Service
cd src/ContestService
dotnet run
```

### Option 4: Chạy với Docker Scripts

```bash
# Development mode
bash scripts/docker/dev-run.sh

# Production mode
bash scripts/docker/prod-run.sh

# Debug mode
bash scripts/docker/debug-run.sh
```

## 📁 Cấu trúc dự án

```
utc2oj/
├── src/                          # Source code
│   ├── ApiGateway/              # Nginx reverse proxy
│   │   ├── Dockerfile
│   │   └── nginx.conf
│   ├── Aspire/                  # .NET Aspire orchestration
│   │   ├── UTC2OJ.AppHost/      # Aspire host application
│   │   └── UTC2OJ.ServiceDefaults/ # Shared service configurations
│   ├── UserService/             # Service quản lý người dùng
│   │   ├── Controllers/
│   │   ├── Data/
│   │   ├── Models/
│   │   └── Program.cs
│   ├── ProblemService/          # Service quản lý bài tập
│   ├── SubmissionService/       # Service nhận bài nộp
│   ├── ExecutionService/        # Service chấm bài
│   ├── ContestService/          # Service quản lý contest
│   └── Shared/                  # Shared libraries
├── deploy/                      # Deployment configs
│   └── docker-compose.yml       # Docker Compose configuration
├── scripts/                     # Utility scripts
│   └── docker/                  # Docker run scripts
├── docs/                        # Documentation
├── assets/                      # Static assets
├── global.json                  # .NET SDK version
├── utc2oj.sln                  # Solution file
├── package.json                 # NPM configuration
└── README.md                    # This file
```

## 🔌 API Endpoints

### User Service (Port 5001)
```
POST   /api/users/register       # Đăng ký user mới
POST   /api/users/login          # Đăng nhập
GET    /api/users/{id}           # Lấy thông tin user
PUT    /api/users/{id}           # Cập nhật user
DELETE /api/users/{id}           # Xóa user
```

### Problem Service (Port 5002)
```
GET    /api/problems             # Danh sách problems
GET    /api/problems/{id}        # Chi tiết problem
POST   /api/problems             # Tạo problem mới
PUT    /api/problems/{id}        # Cập nhật problem
DELETE /api/problems/{id}        # Xóa problem
```

### Submission Service (Port 5003)
```
GET    /api/submissions          # Danh sách submissions
GET    /api/submissions/{id}     # Chi tiết submission
POST   /api/submissions          # Nộp bài
GET    /api/submissions/user/{userId} # Submissions của user
```

### Execution Service (Port 5004)
```
GET    /api/executions           # Danh sách executions
GET    /api/executions/{id}      # Chi tiết execution
POST   /api/executions/execute   # Thực thi code
```

### Contest Service (Port 5005)
```
GET    /api/contests             # Danh sách contests
GET    /api/contests/{id}        # Chi tiết contest
POST   /api/contests             # Tạo contest mới
PUT    /api/contests/{id}        # Cập nhật contest
DELETE /api/contests/{id}        # Xóa contest
GET    /api/contests/{id}/leaderboard # Bảng xếp hạng
```

## 🧪 Testing

```bash
# Chạy tất cả tests
dotnet test

# Chạy tests với coverage
dotnet test /p:CollectCoverage=true
```

## 📝 Development

### Build solution

```bash
# Build tất cả projects
dotnet build

# Build specific project
dotnet build src/UserService/UserService.csproj
```

### Database Migrations

```bash
# Add migration
cd src/UserService
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Code Style

Project sử dụng Husky để enforce code quality:
- Automatic linting khi commit
- Pre-commit hooks

## 🤝 Đóng góp

Chúng tôi hoan nghênh mọi đóng góp! Vui lòng:

1. Fork repository
2. Tạo branch cho feature (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push lên branch (`git push origin feature/AmazingFeature`)
5. Mở Pull Request

## 📄 License

Dự án được phân phối dưới giấy phép ISC. Xem file `LICENSE` để biết thêm chi tiết.

## 👥 Tác giả

- GitHub: [@bientranngoc](https://github.com/bientranngoc)

## 🔗 Links

- Repository: https://github.com/bientranngoc/utc2oj
- Issues: https://github.com/bientranngoc/utc2oj/issues

## 📧 Liên hệ

Nếu bạn có câu hỏi hoặc đề xuất, vui lòng tạo issue trên GitHub repository.

---

**Made with ❤️ for UTC2**