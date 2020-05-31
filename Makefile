unity-build:
	"C:\Program Files\Unity\Hub\Editor\2019.2.17f1\Editor\Unity.exe" -batchmode -nographics -quit -executeMethod Build.BuildAll
build-server-container:
	docker build -f dockerfile.webserver . -t light-bike/game-server
docker-clean-server:
	- docker stop my-test
	- docker rm my-test
docker-start-server: docker-clean-server build-server-container
	docker run -it -p 8080:8080 --name=my-test light-bike/game-server
gcloud-push: build-server-container
	docker tag light-bike/game-server gcr.io/light-bike-278304/light-bike-game-sever
	docker push gcr.io/light-bike-278304/light-bike-game-sever