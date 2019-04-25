FROM microsoft/aspnetcore-build
WORKDIR /app

COPY src .
WORKDIR /app/Logo.Api
RUN dotnet restore
RUN dotnet build --configuration Production
ENTRYPOINT ["dotnet", "run", "-c Production"]
