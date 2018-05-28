# Build the .NET part
FROM microsoft/dotnet:2.0.5-sdk-2.1.4 AS dotnet-build
WORKDIR /src
COPY ./README.md /Packages
COPY ./Build ./Build
COPY ./Source/Visualization/NuGet.Config ./
COPY ./Source/Visualization/. ./Source/Visualization
COPY ./Source/Infrastructure/. ./Source/Infrastructure
WORKDIR /src/Source/Visualization/Web
RUN dotnet restore --ignore-failed-sources
RUN dotnet publish -c Release -o out

# Build the static content
FROM node:latest AS node-build
WORKDIR /src
COPY ./Source/Visualization/Web/. ./Source/Visualization/Web
RUN find . -name "*.cs" -type f -delete
RUN find . -name "*.csproj" -type f -delete
RUN find . -name "yarn.lock" -type f -delete
WORKDIR /src/Source/Visualization/Web
RUN yarn global add webpack
RUN yarn global add webpack-cli
RUN yarn add babel-loader
RUN yarn
RUN webpack -p --env.production

# Build runtime image
FROM microsoft/aspnetcore:2.0.5
WORKDIR /app
COPY --from=dotnet-build /src/Source/Visualization/Web/out ./
COPY --from=node-build /src/Source/Visualization/Web/wwwroot ./wwwroot
EXPOSE 80

#ENTRYPOINT ["dotnet", "Web.dll"]
ENTRYPOINT ["/bin/sh", "-c", "echo 104.45.19.232   kafka-service >> /etc/hosts && dotnet Web.dll"]