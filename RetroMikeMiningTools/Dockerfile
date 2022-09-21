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

RUN apt-get update -y
RUN apt install -y wget unzip
RUN apt-get install -y tzdata
RUN wget -q https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN dpkg -i google-chrome-stable_current_amd64.deb; exit 0
RUN apt -f install -y

RUN CHROME_MAJOR_VERSION=$(google-chrome --product-version --no-sandbox | sed -E 's/\.[^.]*$//') && CHROME_DRIVER_VERSION=$(wget -O - "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_$CHROME_MAJOR_VERSION") && wget -O chromedriver_linux64.zip https://chromedriver.storage.googleapis.com/$CHROME_DRIVER_VERSION/chromedriver_linux64.zip
RUN unzip chromedriver_linux64.zip
RUN rm chromedriver_linux64.zip
RUN rm google-chrome-stable_current_amd64.deb

ENTRYPOINT ["dotnet", "RetroMikeMiningTools.dll","--platform_name=Docker"]