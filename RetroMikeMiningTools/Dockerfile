FROM mcr.microsoft.com/dotnet/sdk as build

WORKDIR /app

COPY *.csproj ./
COPY NuGet.Config ./
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
EXPOSE 7000
COPY --from=build /app/out ./

ARG TARGETPLATFORM
ENV DOCKER_PLATFORM=${TARGETPLATFORM}
RUN if [ "${TARGETPLATFORM}" = "linux/amd64" ]; then apt-get update -y && apt-get install -y wget unzip chromium && CHROME_MAJOR_VERSION=$(chromium --product-version --no-sandbox | sed -E 's/\.[^.]*$//') && CHROME_DRIVER_VERSION=$(wget -O - "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_$CHROME_MAJOR_VERSION") && wget -O chromedriver_linux64.zip https://chromedriver.storage.googleapis.com/$CHROME_DRIVER_VERSION/chromedriver_linux64.zip && unzip chromedriver_linux64.zip && rm chromedriver_linux64.zip; fi

ENTRYPOINT dotnet RetroMikeMiningTools.dll --platform_name=Docker-${DOCKER_PLATFORM}



#docker buildx build -t theretromike/miningtools:dev . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push