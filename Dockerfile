FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ./CSharpAspCoreAdoPg/*.csproj ./
RUN dotnet restore CSharpAspCoreAdoPg.csproj

COPY ./CSharpAspCoreAdoPg .
RUN dotnet publish CSharpAspCoreAdoPg.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

RUN addgroup -S appgroup && adduser -S appuser -G appgroup
RUN chown -R appuser:appgroup /app
USER appuser

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CSharpAspCoreAdoPg.dll"]

EXPOSE 8001
