# Build the .NET part
FROM microsoft/dotnet:2.2-sdk-bionic AS dotnet-build
WORKDIR /src
COPY ./README.md /Packages
COPY ./Build ./Build
COPY ./Source/Visualization/NuGet.Config ./
COPY ./Source/Visualization/. ./Source/Visualization
WORKDIR /src/Source/Visualization/Core
RUN dotnet restore --ignore-failed-sources
RUN dotnet publish -c Release -o out

# Build the static content
FROM node:latest AS node-build
WORKDIR /src
COPY ./Source/Visualization/Web/. ./Source/Visualization/Web
RUN find . -name "yarn.lock" -type f -delete
WORKDIR /src/Source/Visualization/Web
RUN yarn global add webpack
RUN yarn global add webpack-cli
RUN yarn add babel-loader@7
RUN yarn
RUN webpack -p --env.production

# Build runtime image
FROM microsoft/dotnet:2.2-sdk-bionic
WORKDIR /app
COPY --from=dotnet-build /src/Source/Visualization/Core/out ./
COPY --from=node-build /src/Source/Visualization/Web/wwwroot ./wwwroot
EXPOSE 80

ENTRYPOINT ["dotnet", "Web.dll"]