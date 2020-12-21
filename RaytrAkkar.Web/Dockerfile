#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RaytrAkkar.Web/RaytrAkkar.Web.csproj", "RaytrAkkar.Web/"]
COPY ["RaytrAkkar.Common/RaytrAkkar.Common.csproj", "RaytrAkkar.Common/"]
COPY ["RaytrAkkar.Raytracer/RaytrAkkar.Raytracer.csproj", "RaytrAkkar.Raytracer/"]
RUN dotnet restore "RaytrAkkar.Web/RaytrAkkar.Web.csproj"
COPY . .
WORKDIR "/src/RaytrAkkar.Web"
RUN dotnet build "RaytrAkkar.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaytrAkkar.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaytrAkkar.Web.dll"]