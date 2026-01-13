# Use the official .NET 5.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["AuthAPI.csproj", "./"]
RUN dotnet restore "./AuthAPI.csproj"

# Copy the rest of the code and build
COPY . .
RUN dotnet build "AuthAPI.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "AuthAPI.csproj" -c Release -o /app/publish

# Use the official .NET 5.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000
ENTRYPOINT ["dotnet", "AuthAPI_New.dll"]
