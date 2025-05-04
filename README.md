Project Name
Todo API

Library
## SQL
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

## Validate DTOs
dotnet add package FluentValidation.AspNetCore

## Auto Mapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

## OpenAPI Documentation (Swagger)
dotnet add package Swashbuckle.AspNetCore

## Memory cache (Use Memory Cache or Redis)
builder.Services.AddMemoryCache();

## API versioning
dotnet add package Microsoft.AspNetCore.Mvc.Versioning

##Integrate with Swagger
dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
## Support sorting
dotnet add package System.Linq.Dynamic.Core

##Migration
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
To do enhancement
How to run load test cac dung ProducesResponseType

Learning
ProjectTo và Mapper: projecTo dùng cho iqueryable, và nó tìm những field cần map trong bộ config của automapper
còn map thì sẽ map trên dữ liệu đã tải về bộ nhớ
var ordersDto = dbContext.Orders
.Where(o => o.Status == "Pending")
.ProjectTo(_mapper.ConfigurationProvider)
.ToList();
ToPagedResponseAsync => helper cho Paging
dùng CancellationToken để dừng,
Là một tín hiệu (signal) được gửi đi cho async code hoặc long-running operation biết rằng:
👉 "Tao muốn mày dừng công việc lại càng sớm càng tốt."
Nó không ép thread dừng lại ngay lập tức như Thread.Abort() (vốn đã bị deprecated vì nguy hiểm).
FUll text search
Nếu bạn Dùng gì Tìm kiếm chính xác 1-2 field LIKE hoặc .Contains() đủ Tìm kiếm toàn cục nhanh EF.Functions.FreeText + Full-Text Index Tìm kiếm dynamic trên nhiều field Full-Text hoặc custom mapping thay vì reflection Dữ liệu <1000 records nào cũng được, chưa thấy rõ chênh lệch Dữ liệu >10k records nên dùng Full-Text hoặc có Index

LIKE '%text%': mất ~500ms đến vài giây trên table >10k rows.

FREETEXT(text, 'bug fix'): chỉ ~30ms với full-text index.

Reflection: phải .GetProperties() + .Contains() từng field + không translate được sang SQL → load toàn bộ dữ liệu về RAM, cực chậm.

Với Full-Text Index (Inverted Index): SQL tạo một bảng riêng gọi là "inverted index", nó giống như:

Word Row IDs bug 1, 3 urgent 1 resolved 3

Khi bạn tìm "bug", SQL không cần đọc từng dòng nữa → nó tra thẳng vào index:

sql

"bug" => [1, 3]

→ Trả về dòng 1 và 3 ngay lập tức.

Tại sao tốn dung lượng? Vì SQL Server lưu thêm một bảng riêng (gọi là Full-Text Catalog) chứa:

Từng từ tách ra từ cột Title

ID của dòng chứa từ đó

Vị trí từ trong chuỗi (để hỗ trợ NEAR, RANK, ...)

Khi bạn tạo Full-Text Index trên cột Title, SQL Server sẽ phân tích từng dòng như sau: 🔎 Bước 1: Tách từ (Tokenization) Dòng 1: "Fix urgent bug" → ["Fix", "urgent", "bug"]

Dòng 2: "Feature released" → ["Feature", "released"]

Dòng 3: "Resolved bug" → ["Resolved", "bug"]

🔎 Bước 2: Tạo Inverted Index (ngược lại: từ → dòng) Word Row IDs bug 1, 3 urgent 1 fix 1 feature 2 released 2 resolved 3

SQL Server có 2 chế độ cập nhật Full-Text Index chính:

Automatic (Tự động – mặc định) Khi bạn INSERT, UPDATE, DELETE dữ liệu
→ SQL sẽ ngầm cập nhật index

✅ Không cần làm gì thêm

⛔ Nhưng nếu dữ liệu thêm nhiều, sẽ có độ trễ nhẹ

📌 Mặc định nếu bạn không chỉnh gì, SQL chọn chế độ này.

Nếu bạn không cập nhật Full-Text Index (hoặc để nó ở chế độ Manual) thì sao? 👉 Khi có data mới (INSERT) hoặc sửa (UPDATE), mà bạn không cập nhật index: ✅ Dữ liệu vẫn nằm trong bảng → truy vấn bình thường vẫn có

❌ Nhưng Full-Text Search sẽ không tìm thấy dữ liệu mới đó → Vì nó chỉ tìm trên index cũ mà chưa được cập nhật

Manual (Thủ công) Bạn phải gọi thủ công:
sql Sao chép Chỉnh sửa ALTER FULLTEXT INDEX ON TodoItems START FULL POPULATION; hoặc

sql Sao chép Chỉnh sửa ALTER FULLTEXT INDEX ON TodoItems START INCREMENTAL POPULATION; Dùng khi bạn muốn tự kiểm soát thời điểm cập nhật, thường trong hệ thống cực lớn (hàng triệu dòng)

🧠 Tip: Kiểm tra Catalog đang ở chế độ nào? sql Sao chép Chỉnh sửa SELECT OBJECT_NAME(i.object_id) AS TableName, i.is_enabled, c.is_auto_population AS IsAuto FROM sys.fulltext_indexes i JOIN sys.fulltext_catalogs c ON i.fulltext_catalog_id = c.fulltext_catalog_id ✅ Kết luận Câu hỏi Trả lời Thêm data rồi, index có tự cập nhật không? ✅ Có, nếu đang ở chế độ Auto (mặc định là vậy) Có cần quan tâm chuyện đó không? ❌ Bình thường không cần, chỉ quan tâm nếu bạn làm hệ thống dữ liệu lớn Muốn tự kiểm soát thì sao? 👉 Chuyển sang Manual + gọi cập nhật index khi cần

Với data lớn thì sao? Nếu bạn không cập nhật:

FTS giống như đang nhìn vào một bức ảnh chụp cũ

Search ra kết quả không đầy đủ, gây sai lệch cho người dùng

Nếu cập nhật quá thường xuyên:

Có thể ảnh hưởng hiệu năng hệ thống, vì build index tốn CPU/IO

✅ Giải pháp thường dùng trong production: Cách dùng FTS Gợi ý Dữ liệu ít thay đổi (mostly read) ✅ Dùng AUTO là hợp lý Dữ liệu thay đổi liên tục 🟡 Cân nhắc MANUAL + Job chạy định kỳ (VD: mỗi 5 phút) Dữ liệu cực lớn (triệu bản ghi) ✅ MANUAL + INCREMENTAL index Yêu cầu kết quả realtime ❌ Full-Text không phù hợp, nên kết hợp ElasticSearch

Gợi ý thực chiến 🛠 Nếu bạn đang test dev/local: Dùng AUTO

🕒 Nếu bạn thấy dữ liệu search bị chậm/cũ: chạy lệnh

ALTER FULLTEXT INDEX ON TodoItems START INCREMENTAL POPULATION;

Cách | Cần virtual | Query control | Hiệu suất | Khuyên dùng Lazy Loading | ✅ Có | ❌ Tự động | ❌ Có thể gây n+1 | ❌ Không nên nếu team không rành Eager Loading (Include) | ❌ Không | ✅ Có | ✅ Tốt | ✅ Rất nên dùng Explicit Loading | ❌ Không | ✅ Có | ✅ Tốt | ✅ Với các quan hệ phức tạp

Cách tạo thư mục project
mkdir TodoApp && cd TodoApp dotnet new sln -n TodoAPI

dotnet new webapi -n TodoAPI dotnet new classlib -n TodoAPI.Infrastructure

dotnet sln add TodoAPI/TodoAPI.csproj dotnet sln add TodoAPI.Infrastructure/TodoAPI.Infrastructure.csproj dotnet sln add TodoAPI.Domain/TodoAPI.Domain.csproj

dotnet add TodoApp.API reference TodoApp.Infrastructure

TodoApp/ ├── TodoAPI/ # Web API ├── TodoAPI.Infrastructure/ # Class Library ├── TodoAPI.Domain/ # Class Library └── TodoAPI.sln # Solution file

🎯 Senior-Level Checklist (backend .NET):
Yếu tố Có chưa? Ghi chú
✅ Clean Architecture
⬜ Layered: API → App → Domain → Infra 
✅ CQRS + MediatR 
⬜ Commands & Queries tách biệt rõ
✅ Unit Tests 
⬜ Dùng xUnit + Moq test Service & Handler 
✅ Authentication 
⬜ JWT + policy-based authorization
✅ Logging
⬜ Serilog + Console/File/Seq 
✅ Swagger Docs 
⬜ Mô tả rõ toàn bộ endpoint
✅ Docker Support 
⬜ Dockerfile + docker-compose.yml 
✅ EF Core Migrations
⬜ DB version control
✅ DTOs + Validation 
⬜ FluentValidation & ModelState handling 
✅ Caching (bonus) 
⬜ MemoryCache cho danh sách task
✅ CI/CD (bonus) 
⬜ GitHub Actions hoặc Azure Pipeline

TodoApp/ │ ├── src/ │ 
├── TodoApp.API/ # Entry point (Controllers, Swagger, DI config) │
├── TodoApp.Application/ # UseCases, DTOs, CQRS (Commands/Queries) │ 
├── TodoApp.Domain/ # Entities, Interfaces, Enums │ 
├── TodoApp.Infrastructure/ # EF Core, DB context, Repositories, Logging │ 
├── tests/ 
    │
    ├── TodoApp.UnitTests/ │ 
    ├── TodoApp.IntegrationTests/

Auth Example (JWT)

Muốn “nâng cấp” to-do list thì thêm:

Soft delete (IsDeleted flag)

✅ Paging, filtering, sorting

✅ Background jobs (nhắc nhở task chưa hoàn thành)

✅ Logging + exception tracking (Serilog)

✅ API versioning

Data migration
dotnet tool install --global dotnet-ef

dotnet add TodoApp.Infrastructure package Microsoft.EntityFrameworkCore.Design --version 8.0.0 dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

cd src/TodoApp.Infrastructure dotnet ef migrations add InitDb dotnet ef database update ASPNETCORE_ENVIRONMENT=Development dotnet ef migrations add InitDb --project TodoApp.Infrastructure --startup-project TodoApp.API

DI
AddScoped: 1 instance cho mỗi http request: Repository, Unit of work, Db context, User Context AddSingleton: 1 instance duy nhất trong app: Config, Logger, Cache shared AddTransient: Instance cho mỗi lần inject( resolved): Service stateless( send email)

How DI works? .NET dùng built-in IOC ( Inversion of control) container

Khi run project ( dotnet run), app đọc Program.cs

Gặp builder.Services.AddScoped<ITodoRepository, TodoRepository>(); nó lưu vào container

Khi gọi 1 controller, nó thấy controller cần ITodoRepository

Container resolve -> tạo instance của TodoRepository -> inject vào controller

->Nếu k dùng DI thì k dùng dc AOP( Logging, Caching, Validation middleware) -DI cho phép dùng: + Middleware + Decorator + Interceptor(AOP)

Không quản lý vòng đời: phải tự manage dispose, leak memory -- DEEP WORK OF DI DI Container là 1 object kiểu IServiceProvider
Chứa 1 dictionary kiểu Type( interface) -> Func<IserviceProvider, object>(factory) quản lý cách khởi tạo, lifecycle và các dependency con
AddScoped: + Request 1: Controller inject TodoService (Instance A) Logger inject TodoService (Instance A) -> cùng bản + Request 2: Todo service Instance B

AddTransient: + Request 1: Controller inject TodoService (Instance A) Logger inject TodoService(Instance B) -> khác bản + Request 2: Todo service Instance C

Config env
macOS / Linux
export ASPNETCORE_ENVIRONMENT=Development

Windows CMD
set ASPNETCORE_ENVIRONMENT=Development //hoặc config trong launchSettings.json

##Mỗi lần add new project hay nuget, reference project dùng dotnet clean dotnet restore dotnet nuget locals all --clear

để remapping

Windows
attrib -r README.md

Linux/Mac
chmod u+w README.md

## POSTGRES
check db:  psql -U postgres -W -l
psql postgres 

## CQRS and MediatR
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection

🔍 CQRS là gì?
CQRS = Command Query Responsibility Segregation

Command: thao tác gây thay đổi dữ liệu (Create, Update, Delete)

Query: thao tác chỉ đọc dữ liệu (GetAll, GetById...)

💡 Tách biệt hoàn toàn "đọc" và "ghi" để:

Dễ scale (chia hai hướng)

Dễ test
Dễ maintain

🧠 Mediator pattern là gì?
Mediator là 1 design pattern trung gian, giúp các object không gọi nhau trực tiếp, mà thông qua "người trung gian" (Mediator).

→ Trong .NET, MediatR là một thư viện implement pattern này.

⚙️ Cơ chế hoạt động của MediatR
🧩 Thành phần chính:

Thành phần	Mô tả
IMediator	Interface chính để gửi Command hoặc Query
IRequest<T>	Đại diện cho một request (Command hoặc Query), và trả về kiểu T
IRequestHandler<TRequest, TResponse>	Xử lý logic tương ứng cho request
ServiceCollection.AddMediatR(...)	Đăng ký các handler vào DI container

## MediatR hoạt động như sau:
Bạn gọi Send() → truyền vào CreateTodoCommand

MediatR scan các class đã đăng ký từ AddMediatR(...) và tìm handler phù hợp:

CreateTodoHandler implements IRequestHandler<CreateTodoCommand, int>

MediatR gọi phương thức Handle() trong handler đó

Handler thực thi logic, trả kết quả về lại cho Send()


// Phải tìm chuẩn format code, sau này làm nhiều member, trước khi commit code phải , thì sau này lúc đọc code đỡ nhằn


## Record
✅ Ví dụ record đơn giản:
csharp
Sao chép
Chỉnh sửa
public record TodoDto(int Id, string Title, bool IsCompleted);
Nó tương đương với:

csharp
Sao chép
Chỉnh sửa
public class TodoDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public bool IsCompleted { get; init; }

    public TodoDto(int id, string title, bool isCompleted)
    {
        Id = id;
        Title = title;
        IsCompleted = isCompleted;
    }

    public override bool Equals(object? obj) => ...
    public override int GetHashCode() => ...
}


## mai làm 
1. Khái niệm về Unit of Work
   Unit of Work là một thiết kế giúp quản lý tất cả các thao tác database trong một "transaction" duy nhất. Điều này giúp đảm bảo rằng tất cả các thay đổi được commit cùng nhau (hoặc rollback nếu có lỗi).

Unit of Work sẽ chịu trách nhiệm quản lý các repository, giúp bạn thực hiện các thao tác trên nhiều entity mà không cần gọi SaveChanges nhiều lần.

