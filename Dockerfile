FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY ["DemoProject/DemoProject.csproj", "./DemoProject/"]
RUN dotnet restore "./DemoProject/"

# copy everything and build
COPY . .
RUN dotnet publish "./DemoProject/" -c Release -o out

# build runtime image
FROM microsoft/aspnetcore:2.0 as web
WORKDIR /app
COPY --from=build-env ["/app/DemoProject/out", "."]
CMD ["dotnet", "DemoProject.dll"]