FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . /src
RUN dotnet restore "DoppelgangstersOnline.csproj"
RUN dotnet build "DoppelgangstersOnline.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DoppelgangstersOnline.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
RUN apt-get update
EXPOSE 5000
EXPOSE 3306
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DoppelgangstersOnline.dll"]
