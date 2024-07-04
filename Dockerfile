
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NMS.Assistant.Api/NMS.Assistant.Api.csproj", "NMS.Assistant.Api/"]
COPY ["NMS.Assistant.Data/NMS.Assistant.Data.csproj", "NMS.Assistant.Data/"]
COPY ["NMS.Assistant.Domain/NMS.Assistant.Domain.csproj", "NMS.Assistant.Domain/"]
COPY ["NMS.Assistant.Integration/NMS.Assistant.Integration.csproj", "NMS.Assistant.Integration/"]
COPY ["NMS.Assistant.Persistence/NMS.Assistant.Persistence.csproj", "NMS.Assistant.Persistence/"]
RUN dotnet restore "NMS.Assistant.Api/NMS.Assistant.Api.csproj"
COPY . .
WORKDIR "/src/NMS.Assistant.Api"
RUN dotnet build "NMS.Assistant.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NMS.Assistant.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NMS.Assistant.Api.dll"]