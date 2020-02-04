using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class Build
{
    [MenuItem("MyBuildMenu/Build All")]
    public static void BuildAll(){
        // TODO logging ins't working in headless....
        BuildServer();
        BuildClient();
        if (Application.isBatchMode){
            EditorApplication.Exit(0);
        }
    }

    static void BuildGame(BuildPlayerOptions buildPlayerOptions){
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
    }

    [MenuItem("MyBuildMenu/Set Server Directives")]
    public static void SetPlayerSettingsServer(){
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            "SERVER_BUILD"
        );
    }

    public static void BuildServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "Builds/Server/main.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/GameScene.unity"
        };
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
        SetPlayerSettingsServer();
        BuildGame(buildPlayerOptions);
    }

    [MenuItem("MyBuildMenu/Set Client Directives")]
    public static void SetPlayerSettingsClient(){
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            "CLIENT_BUILD"
        );
    }

    public static void BuildClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "Builds/Client/main.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/GameScene.unity"
        };
        buildPlayerOptions.options = BuildOptions.None;
        SetPlayerSettingsClient();
        BuildGame(buildPlayerOptions);
    }

    [MenuItem("MyBuildMenu/Set Editor Directives")]
    public static void SetPlayerSettingsEditor(){
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            ""
        );
    }
}
