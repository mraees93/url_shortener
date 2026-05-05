# Stage 1: Build the application using .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy the csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build the release
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Run using .NET 10 Runtime
FROM ://microsoft.com AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set Render port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "url_shortener.dll"]
