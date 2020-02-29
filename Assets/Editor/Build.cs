using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;

public class Build
{
  [MenuItem("MyBuildMenu/Build Native")]
  public static void BuildNative()
  {
    BuildServer(false);
    BuildDesktopClient();
    if (Application.isBatchMode)
    {
      EditorApplication.Exit(0);
    }
    else SetDirectives(false, false, false);
  }
  [MenuItem("MyBuildMenu/Web/Build Web")]
  public static void BuildWebGL()
  {
    BuildServer(true);
    BuildClientWeb();
    if (Application.isBatchMode)
    {
      EditorApplication.Exit(0);
    }
    else SetDirectives(false, false, false);
  }

  static void SetDirectives(bool isClient, bool isServer, bool isWeb)
  {
    List<string> allDefines = new List<string>();
    if(isClient) allDefines.Add("CLIENT_BUILD");
    if(isServer) allDefines.Add("SERVER_BUILD");
    if(isWeb) allDefines.Add("WEB");
    string directives = string.Join(";",allDefines.ToArray());
    PlayerSettings.SetScriptingDefineSymbolsForGroup(
        EditorUserBuildSettings.selectedBuildTargetGroup,
        directives
    );
    Debug.Log($"set directives : {directives}");
    AssetDatabase.Refresh();
  }
  [MenuItem("MyBuildMenu/Directives/Set Server")] static void a() { SetDirectives(false, true, false); }
  [MenuItem("MyBuildMenu/Directives/Set Client")] static void b() { SetDirectives(true, false, false); }
  [MenuItem("MyBuildMenu/Directives/Set Editor")] static void c() { SetDirectives(false, false, false); }
  [MenuItem("MyBuildMenu/Web/Build Web Server")] static void d() { BuildServer(true); }
  [MenuItem("MyBuildMenu/Web/Build Web Client")] static void e() { BuildWebGL(); }
  [MenuItem("MyBuildMenu/Build Desktop Server")] static void f() { BuildServer(false); }
  [MenuItem("MyBuildMenu/Build Desktop Client")] static void g() { BuildDesktopClient(); }


  static void BuildGame(BuildPlayerOptions buildPlayerOptions)
  {
    BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
  }

  public static void BuildServer(bool isWeb)
  {
    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    string pathNameOption = isWeb ? "Web" : "Local";
    buildPlayerOptions.locationPathName = $"Builds/Server{pathNameOption}/main.exe";
    buildPlayerOptions.target = BuildTarget.StandaloneWindows;
    buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/GameScene.unity"
        };
    buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
    SetDirectives(false, true, isWeb);
    BuildGame(buildPlayerOptions);
  }

  public static void BuildDesktopClient()
  {
    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    buildPlayerOptions.locationPathName = "Builds/ClientDesktop/main.exe";
    buildPlayerOptions.target = BuildTarget.StandaloneWindows;
    buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/GameScene.unity"
        };
    buildPlayerOptions.options = BuildOptions.None;
    SetDirectives(true, false, false);
    BuildGame(buildPlayerOptions);
  }

  public static void BuildClientWeb()
  {
    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    buildPlayerOptions.locationPathName = "Builds/ClientWeb/";
    buildPlayerOptions.target = BuildTarget.WebGL;
    buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/GameScene.unity"
        };
    buildPlayerOptions.options = BuildOptions.None;
    SetDirectives(true, false, true);
    BuildGame(buildPlayerOptions);
  }
}

