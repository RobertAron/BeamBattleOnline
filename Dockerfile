FROM ubuntu:22.04
EXPOSE 8080 8090

RUN apt-get update && apt-get install -y python3
RUN mkdir /game
WORKDIR /game
COPY build/StandaloneLinux64/ .
RUN chmod +x ./StandaloneLinux64.x86_64

CMD python3 -m http.server 8090 & \
    ./StandaloneLinux64.x86_64 -batchmode -nographics