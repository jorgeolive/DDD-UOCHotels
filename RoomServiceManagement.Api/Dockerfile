FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY RoomServiceManagement.Api/RoomServiceManagement.Api.csproj RoomServiceManagement.Api/
COPY UOCHotels.RoomServiceManagement/UOCHotels.RoomServiceManagement.Application/UOCHotels.RoomServiceManagement.Application.csproj UOCHotels.RoomServiceManagement/UOCHotels.RoomServiceManagement.Application/
COPY UOCHotels.RoomServiceManagement.Domain/UOCHotels.RoomServiceManagement.Domain.csproj UOCHotels.RoomServiceManagement.Domain/
COPY UOCHotels.RoomServiceManagement.Persistence/UOCHotels.RoomServiceManagement.Persistence.csproj UOCHotels.RoomServiceManagement.Persistence/
RUN dotnet restore "RoomServiceManagement.Api/RoomServiceManagement.Api.csproj"
COPY . .
WORKDIR "/src/RoomServiceManagement.Api"
RUN dotnet build "RoomServiceManagement.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RoomServiceManagement.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RoomServiceManagement.Api.dll"]
