# Development Dockerfile with hot reload support
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Install dotnet-ef tools globally for development
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Set environment variables for development and hot reload
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:8080

# Install file watcher dependencies
RUN apt-get update && apt-get install -y inotify-tools && rm -rf /var/lib/apt/lists/*

# Copy csproj and restore dependencies
COPY ["GerenciamentoDeTarefas.csproj", "./"]
RUN dotnet restore "GerenciamentoDeTarefas.csproj"

# Copy source code
COPY . .

# Expose ports
EXPOSE 8080
EXPOSE 5134

# Use dotnet watch for hot reload in development
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]