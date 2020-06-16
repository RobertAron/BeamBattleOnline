FROM ubuntu:20.10
EXPOSE 8080
COPY /Builds/Web/Server /Server
ENTRYPOINT [ "//Server/main.exe" ]