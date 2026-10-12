FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/ToeicReadingApp.Web.Host/ToeicReadingApp.Web.Host.csproj", "src/ToeicReadingApp.Web.Host/"]
COPY ["src/ToeicReadingApp.Web.Core/ToeicReadingApp.Web.Core.csproj", "src/ToeicReadingApp.Web.Core/"]
COPY ["src/ToeicReadingApp.Application/ToeicReadingApp.Application.csproj", "src/ToeicReadingApp.Application/"]
COPY ["src/ToeicReadingApp.Core/ToeicReadingApp.Core.csproj", "src/ToeicReadingApp.Core/"]
COPY ["src/ToeicReadingApp.EntityFrameworkCore/ToeicReadingApp.EntityFrameworkCore.csproj", "src/ToeicReadingApp.EntityFrameworkCore/"]
WORKDIR "/src/src/ToeicReadingApp.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/ToeicReadingApp.Web.Host", "src/ToeicReadingApp.Web.Host"]
COPY ["src/ToeicReadingApp.Web.Core", "src/ToeicReadingApp.Web.Core"]
COPY ["src/ToeicReadingApp.Application", "src/ToeicReadingApp.Application"]
COPY ["src/ToeicReadingApp.Core", "src/ToeicReadingApp.Core"]
COPY ["src/ToeicReadingApp.EntityFrameworkCore", "src/ToeicReadingApp.EntityFrameworkCore"]
WORKDIR "/src/src/ToeicReadingApp.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "ToeicReadingApp.Web.Host.dll"]
