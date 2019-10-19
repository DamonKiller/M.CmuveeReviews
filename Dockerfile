FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["CmuveeReviews.NetCore/CmuveeReviews.NetCore.csproj", "CmuveeReviews.NetCore/"]
RUN dotnet restore "CmuveeReviews.NetCore/CmuveeReviews.NetCore.csproj"
COPY . .
WORKDIR "/src/CmuveeReviews.NetCore"
RUN dotnet build "CmuveeReviews.NetCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CmuveeReviews.NetCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CmuveeReviews.NetCore.dll"]