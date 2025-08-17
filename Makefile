IMAGE_NAME = light-bike/game-server
TAG = $(shell git rev-parse --short HEAD)
REGISTRY = registry.digitalocean.com/my-regsitry

do-docker:
	- docker stop my-test
	- docker rm my-test
	docker buildx build --platform=linux/amd64 -t $(IMAGE_NAME):$(TAG) .
	docker run -it -p 8080:8080 -p 8090:8090 --name=my-test $(IMAGE_NAME):$(TAG)

push-docker:
	docker tag $(IMAGE_NAME):$(TAG) $(REGISTRY)/$(IMAGE_NAME):$(TAG)
	docker push $(REGISTRY)/$(IMAGE_NAME):$(TAG)
# "C:\Program Files\Unity\Hub\Editor\2019.3.11f1\Editor\Unity.exe" \
# -batchmode \
# -logfile \
# .\log.txt \
# -quit \
# -customBuildName \
# StandaloneWindows64 \
# -buildTarget \
# StandaloneWindows64 \
# -customBuildTarget \
# StandaloneWindows64 \
# -customBuildPath \
# \github\workspace\build\StandaloneWindows64\StandaloneWindows64.exe \
# -executeMethod \
# Build.BuildWebGL 