unity-build:
	"C:\Program Files\Unity\Hub\Editor\2019.2.17f1\Editor\Unity.exe" -batchmode -nographics -quit -executeMethod Build.BuildAll
build-build:
	docker build . -t light-bike/game-server
docker-clean:
	docker stop my-test
	docker rm my-test
docker-start: docker-stop
	docker run -it --name=my-test mcr.microsoft.com/windows/nanoserver:1803-amd64
