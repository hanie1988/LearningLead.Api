### OAuth2 Packages
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
```

### Sql Server Packages
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet remove package Microsoft.EntityFrameworkCore
dotnet remove package Microsoft.EntityFrameworkCore.SqlServer
dotnet remove package Microsoft.EntityFrameworkCore.Tools
```

```
dotnet list . package

dotnet list reference
```

```Csharp
cd Src/Api

dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
```