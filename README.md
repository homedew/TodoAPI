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

    Integrate with Swagger
    dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer 
    
# To do enhancement
How to run load test
cac dung ProducesResponseType