FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

COPY ./NuGet.Config ./
COPY ./Infrastructure ./Infrastructure
COPY ./Modules/MQTT ./Modules/MQTT
RUN dotnet restore --ignore-failed-sources ./Modules/MQTT/Entry
RUN dotnet publish ./Modules/MQTT/Entry -c Release -o out --no-restore

FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/Modules/MQTT/Entry/out ./

#RUN useradd -ms /bin/bash moduleuser
#USER moduleuser

EXPOSE 1883/tcp

ENTRYPOINT ["/bin/sh", "-c", "echo 137.117.175.54   kafka-service >> /etc/hosts && dotnet Entry.dll"]