using UnityEditor;

// Build.BuildWindows
// "C:\Program Files\Unity\Hub\Editor\2019.3.11f1\Editor\Unity.exe" -batchmode -nographics -quit -executeMethod Build.BuildWindows
public class Build	
{
    [MenuItem("My Build/Windows")]
    public static void BuildWindows(){
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = $"build/Windows/main.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.scenes = new[] {	
            "Assets/Scenes/MenuScene.unity",
            "Assets/Scenes/GameScene.unity"
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        
    }
    [MenuItem("My Build/Server")]
    public static void BuildServer(){
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = $"build/Server/StandaloneLinux64.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/GameScene.unity"	
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
    [MenuItem("My Build/WebGL")]
    public static void BuildWebGL(){
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = $"build/WebGL";
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.scenes = new[] {	
            "Assets/Scenes/MenuScene.unity",
            "Assets/Scenes/GameScene.unity"	
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}