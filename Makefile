do-docker:
	- docker stop my-test
	- docker rm my-test
	- docker build . -t light-bike/game-server
	docker run -it -p 80:8080 --name=my-test light-bike/game-server


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