FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ToDoList/ToDoList.csproj", "ToDoList/"]
RUN dotnet restore "ToDoList/ToDoList.csproj"

COPY ["ToDoList/", "ToDoList/"]
WORKDIR "/src/ToDoList"
RUN dotnet build "ToDoList.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ToDoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install SQLite (helpful for development mode)
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

# Environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Create volume for database persistence
VOLUME /app/Data

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ToDoList.dll"]