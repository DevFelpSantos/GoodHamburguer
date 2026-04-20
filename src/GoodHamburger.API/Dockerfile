FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/GoodHamburger.Domain/GoodHamburger.Domain.csproj", "src/GoodHamburger.Domain/"]
COPY ["src/GoodHamburger.Application/GoodHamburger.Application.csproj", "src/GoodHamburger.Application/"]
COPY ["src/GoodHamburger.Infrastructure/GoodHamburger.Infrastructure.csproj", "src/GoodHamburger.Infrastructure/"]
COPY ["src/GoodHamburger.API/GoodHamburger.API.csproj", "src/GoodHamburger.API/"]

RUN dotnet restore "src/GoodHamburger.API/GoodHamburger.API.csproj"

COPY . .

RUN dotnet build "src/GoodHamburger.API/GoodHamburger.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/GoodHamburger.API/GoodHamburger.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GoodHamburger.API.dll"]