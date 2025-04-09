# Project Name
Todo API
# Library
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

# To do enhancement
How to run load test
cac dung ProducesResponseType


# Learning
 ## ProjectTo và Mapper: projecTo dùng cho iqueryable, và nó tìm những field cần map trong bộ config của automapper
 ## còn map thì sẽ map trên dữ liệu đã tải về bộ nhớ

## var ordersDto = dbContext.Orders
   ##  .Where(o => o.Status == "Pending")
   ##  .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
   ##  .ToList();


   ## ToPagedResponseAsync => helper cho Paging
   ## dùng CancellationToken để dừng, 
   ## Là một tín hiệu (signal) được gửi đi cho async code hoặc long-running operation biết rằng:
   ## 👉 "Tao muốn mày dừng công việc lại càng sớm càng tốt."

#### Nó không ép thread dừng lại ngay lập tức như Thread.Abort() (vốn đã bị deprecated vì nguy hiểm).