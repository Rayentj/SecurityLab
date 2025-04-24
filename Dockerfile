# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ✅ Copy csproj files
COPY ["DentalApp/DentalApp.Api.csproj", "DentalApp/"]
COPY ["DentalApp.Application/DentalApp.Application.csproj", "DentalApp.Application/"]
COPY ["DentalApp.Data/DentalApp.Data.csproj", "DentalApp.Data/"]
COPY ["DentalApp.Domain/DentalApp.Domain.csproj", "DentalApp.Domain/"]
COPY ["DentalApp.Infra/DentalApp.Infra.csproj", "DentalApp.Infra/"]

# ✅ Restore
RUN dotnet restore "DentalApp/DentalApp.Api.csproj"

# ✅ Copy the rest of the files
COPY . .

# ✅ Set the working directory to where Program.cs is
WORKDIR /src/DentalApp

# ✅ Build & publish the app
RUN dotnet publish "DentalApp.Api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose default port
EXPOSE 80

# ✅ Run the app
ENTRYPOINT ["dotnet", "DentalApp.Api.dll"]
