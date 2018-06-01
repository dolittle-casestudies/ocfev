FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

ADD ./NuGet.Config ./
ADD ./Modules/Adapter ./Modules/Adapter
ADD ./Infrastructure ./Infrastructure
RUN dotnet restore --ignore-failed-sources ./Modules/Adapter/Ingestion/
RUN dotnet publish ./Modules/Adapter/Ingestion/ --no-restore -c Release -o /app/out

FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out ./


#RUN useradd -ms /bin/bash moduleuser
#USER moduleuser


ENTRYPOINT ["/bin/sh", "-c", "echo 104.45.19.232   kafka-service >> /etc/hosts && dotnet Ingestion.dll"]