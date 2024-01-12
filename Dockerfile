FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

# Expose port 5000 for the ASP.NET Core app
ENV ASPNETCORE_URLS=http://+:5000

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["CaptchaTest.csproj", "./"]
# Restore dependencies and build app
RUN dotnet restore "CaptchaTest.csproj"# Build stage

# Compile & publish release version of app
COPY . .
WORKDIR "/src/."
RUN dotnet build "CaptchaTest.csproj" -c $configuration -o /app/build

# Publish stage  
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "CaptchaTest.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Copy the fonts folder from the root application to the /app directory inside the container
COPY ./fonts /app/fonts

# Install the fonts from the fonts folder into the system fonts directory within the container
RUN mkdir -p /usr/share/fonts/truetype/ && \
    find /app/fonts -name "*.ttf" -exec install -m644 {} /usr/share/fonts/truetype/ \;

# Clean up - remove the fonts folder from the /app directory inside the container
RUN rm -rf /app/fonts

# Copy published app and run it
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaptchaTest.dll"]


