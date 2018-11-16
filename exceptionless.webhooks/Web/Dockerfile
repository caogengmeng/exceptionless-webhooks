FROM microsoft/dotnet:2.1-aspnetcore-runtime
RUN cp /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "Web.dll"]