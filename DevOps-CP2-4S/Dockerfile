#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DevOps-CP2-4S/DevOps-CP2-4S.csproj", "DevOps-CP2-4S/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["Services/Services.csproj", "Services/"]

RUN dotnet restore "DevOps-CP2-4S/DevOps-CP2-4S.csproj"
COPY . .
WORKDIR "/src/DevOps-CP2-4S"
RUN dotnet build "DevOps-CP2-4S.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevOps-CP2-4S.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevOps-CP2-4S.dll"]