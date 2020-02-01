unity-build:
	"C:\Program Files\Unity\Hub\Editor\2019.2.17f1\Editor\Unity.exe" -batchmode -nographics -quit -executeMethod Build.BuildAll
build-server-container:
	docker build . -t light-bike/game-server
docker-clean-server:
	docker stop my-test
	docker rm my-test
docker-start-server: docker-clean-server
	docker run -it -p 7777:7777/udp --name=my-test light-bike/game-server
