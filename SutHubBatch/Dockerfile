FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SutHubBatch/SutHubBatch.csproj", "SutHubBatch/"]
RUN dotnet restore "SutHubBatch/SutHubBatch.csproj"
COPY . .
WORKDIR "/src/SutHubBatch"
RUN dotnet build "SutHubBatch.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SutHubBatch.csproj" -c Release -o /app

FROM base AS final
#WORKDIR /app

# Configure timezone
RUN unlink /etc/localtime
RUN ln -s /usr/share/zoneinfo/Brazil/East /etc/localtime

# Update aptget and Install cron
RUN apt-get update && apt-get -y install cron

# Add crontab file in the cron directory
RUN printf "* * * * * cd /app; dotnet SutHubBatch.dll\n" >> /etc/cron.d/crontab
RUN printf "* * * * * cd /app; sleep 15; dotnet SutHubBatch.dll\n" >> /etc/cron.d/crontab
RUN printf "* * * * * cd /app; sleep 30; dotnet SutHubBatch.dll\n" >> /etc/cron.d/crontab
RUN printf "* * * * * cd /app; sleep 45; dotnet SutHubBatch.dll\n" >> /etc/cron.d/crontab

# Give execution rights on the cron job
RUN chmod 0644 /etc/cron.d/crontab

# Apply cron job
RUN crontab /etc/cron.d/crontab

# Create the log file to be able to run tail
RUN touch /var/log/cron.log

# Run the command on container startup
CMD cron && tail -f /var/log/cron.log

HEALTHCHECK --interval=5s --timeout=30s --retries=3 CMD cat /app/log || exit 1

WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "SutHubBatch.dll"]

