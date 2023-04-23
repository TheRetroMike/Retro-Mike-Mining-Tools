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
EXPOSE 7001
COPY --from=build /app/out ./

RUN apt-get install -y tzdata

ARG TARGETPLATFORM
ENV DOCKER_PLATFORM=${TARGETPLATFORM}
#RUN if [ "${TARGETPLATFORM}" = "linux/amd64" ]; then apt-get update -y && apt-get install -y wget unzip chromium && CHROME_MAJOR_VERSION=$(chromium --product-version --no-sandbox | sed -E 's/\.[^.]*$//') && CHROME_DRIVER_VERSION=$(wget -O - "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_$CHROME_MAJOR_VERSION") && wget -O chromedriver_linux64.zip https://chromedriver.storage.googleapis.com/$CHROME_DRIVER_VERSION/chromedriver_linux64.zip && unzip chromedriver_linux64.zip && rm chromedriver_linux64.zip; fi

# MultiUser Flux: 
#ENTRYPOINT dotnet RetroMikeMiningTools.dll --platform_name=Docker-${DOCKER_PLATFORM} --multi_user_mode=true --flux_mode=true

# MultiUser: 
ENTRYPOINT dotnet RetroMikeMiningTools.dll --platform_name=Docker-${DOCKER_PLATFORM} --multi_user_mode=true

# SingleUser: 
#ENTRYPOINT dotnet RetroMikeMiningTools.dll --platform_name=Docker-${DOCKER_PLATFORM}



#UAT: docker buildx build -t theretromike/miningtools:uat . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#QA: docker buildx build -t theretromike/miningtools:qa . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#Flux Multi User Release: docker buildx build -t theretromike/miningtools:flux . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#Single User Release: docker buildx build -t theretromike/miningtools:single_user . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#Multi User Release: docker buildx build -t theretromike/miningtools:multi_user . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push