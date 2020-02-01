FROM mcr.microsoft.com/windows:1809-amd64
EXPOSE 7777
COPY ./Builds/Server/ ./Server
ENTRYPOINT [ "/Server/Main.exe" ]