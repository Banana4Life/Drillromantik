* Docker image: unityci/editor:ubuntu-2020.3.5f1-webgl-0.12.0
* password in .pass
* BUILD_PATH env for output dir
* code in readable+writable /src
* Unity_Lic.ulf /root/.local/share/unity3d/Unity/Unity_lic.ulf
* command: xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' unity-editor -projectPath /src -quit -batchmode -nographics -buildTarget WebGL -executeMethod Editor.BuildScript.PerformBuild -logFile /dev/stdout -username phillip@schich.tel -password "$(< .pass)"
  * improve with https://docs.unity3d.com/Manual/CommandLineArguments.html via "Create a license activation file and import license file by command"

