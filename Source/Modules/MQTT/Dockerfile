FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

COPY ./NuGet.Config ./
COPY ./Infrastructure ./Infrastructure
COPY ./Modules/MQTT ./Modules/MQTT
WORKDIR /app/Modules/MQTT/Entry
RUN dotnet restore --ignore-failed-sources
RUN dotnet publish -c Release -o out --no-restore

FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/Modules/MQTT/Entry/out ./

RUN useradd -ms /bin/bash moduleuser
USER moduleuser

EXPOSE 1883/tcp

ENTRYPOINT ["dotnet", "Entry.dll"]