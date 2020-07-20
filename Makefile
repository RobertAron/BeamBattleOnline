do-docker:
	- docker stop my-test
	- docker rm my-test
	- docker build . -t light-bike/game-server
	docker run -it -p 80:8080 --name=my-test light-bike/game-server