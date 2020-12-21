#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RaytrAkkar.SceneRenderService/RaytrAkkar.SceneRenderService.csproj", "RaytrAkkar.SceneRenderService/"]
COPY ["RaytrAkkar.Common/RaytrAkkar.Common.csproj", "RaytrAkkar.Common/"]
COPY ["RaytrAkkar.Raytracer/RaytrAkkar.Raytracer.csproj", "RaytrAkkar.Raytracer/"]
RUN dotnet restore "RaytrAkkar.SceneRenderService/RaytrAkkar.SceneRenderService.csproj"
COPY . .
WORKDIR "/src/RaytrAkkar.SceneRenderService"
RUN dotnet build "RaytrAkkar.SceneRenderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaytrAkkar.SceneRenderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaytrAkkar.SceneRenderService.dll"]