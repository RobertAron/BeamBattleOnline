using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class Build
{
    [MenuItem("MyBuildMenu/Build Server")]
    public static void BuildServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "Builds/Server/main.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/SampleScene.unity"
        };
        buildPlayerOptions.options = BuildOptions.None;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            "UNITY_POST_PROCESSING_STACK_V2;SERVER_BUILD"
        );
        BuildGame(buildPlayerOptions);
    }

    [MenuItem("MyBuildMenu/Build Client")]
    public static void BuildClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "Builds/Client/main.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/SampleScene.unity"
        };
        buildPlayerOptions.options = BuildOptions.None;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            "UNITY_POST_PROCESSING_STACK_V2;CLIENT_BUILD"
        );
        BuildGame(buildPlayerOptions);
    }


    static void BuildGame(BuildPlayerOptions buildPlayerOptions){
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
