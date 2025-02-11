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