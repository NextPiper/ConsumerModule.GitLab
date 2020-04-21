# Define node-builder stage
FROM node:10-alpine AS build-node
WORKDIR /ClientApp
COPY ConsumerModule.GitLab/ClientApp/package.json .
COPY ConsumerModule.GitLab/ClientApp/package-lock.json .
RUN npm install
COPY ConsumerModule.GitLab/ClientApp/ .
RUN npm run build

# Define .Net-builder stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-api
ENV BuildingDocker true
WORKDIR /app

# copy csproj and restore respective projects
COPY *.sln .
COPY ConsumerModule.GitLab/*.csproj ./ConsumerModule.GitLab/

# Restore each projects
RUN dotnet restore --packages ./.nuget/packages

# After restore copy all the code and build the App
COPY ConsumerModule.GitLab/. ./ConsumerModule.GitLab/

# Change workdir to NextPipe and build from the .csproj file
WORKDIR /app/ConsumerModule.GitLab
RUN dotnet publish -c Release -o out

# Define runtime stage. Create /app workdir and copy the build
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build-api /app/ConsumerModule.GitLab/out ./
COPY --from=build-node /ClientApp/build ./ClientApp/build
ENTRYPOINT [ "dotnet", "ConsumerModule.GitLab.dll" ]