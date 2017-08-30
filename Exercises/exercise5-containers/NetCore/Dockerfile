FROM microsoft/dotnet:2-sdk AS build-env
WORKDIR /app

COPY *.csproj ./
COPY *.cs ./
RUN dotnet restore && dotnet publish -c Release -r linux-x64 -o out

FROM microsoft/aspnetcore:2
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["./NetCore"]