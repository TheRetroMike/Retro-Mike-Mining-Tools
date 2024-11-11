#FROM ubuntu:focal as build
FROM mcr.microsoft.com/dotnet/sdk as build

WORKDIR /app
#ARG DEBIAN_FRONTEND=noninteractive
#RUN apt-get update -y
#RUN apt-get -y install wget
#RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
#RUN dpkg -i packages-microsoft-prod.deb
#RUN rm packages-microsoft-prod.deb
#RUN apt-get update -y
#RUN apt-get -y install dotnet-sdk

COPY . ./



#FROM mcr.microsoft.com/dotnet/sdk as build


#COPY *.csproj ./
#COPY NuGet.Config ./
#COPY *.nupkg ./

#RUN mkdir -p /nuget
#copy *.nupkg /nuget/
#RUN ls /app/nuget
RUN dotnet restore -s "https://api.nuget.org/v3/index.json" -s "/app/nuget/"

#RUN nuget install nuget\Telerik.UI.for.AspNet.Core.2022.3.913.nupkg -OutputDirectory packages


#RUN dotnet restore -s ""

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
#ENTRYPOINT dotnet RetroMikeMiningTools.dll --platform_name=Docker-${DOCKER_PLATFORM} --multi_user_mode=true

# SingleUser: 
ENTRYPOINT dotnet RetroMikeMiningTools.dll --platform_name=Docker-${DOCKER_PLATFORM}



#UAT: docker buildx build -t theretromike/miningtools:uat . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#QA: docker buildx build -t theretromike/miningtools:qa . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#Flux Multi User Release: docker buildx build -t theretromike/miningtools:flux . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#Single User Release: docker buildx build -t theretromike/miningtools:single_user . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push
#Multi User Release: docker buildx build -t theretromike/miningtools:multi_user . --progress plain --platform linux/amd64,linux/arm64,linux/amd64/v2,linux/amd64/v3 --push