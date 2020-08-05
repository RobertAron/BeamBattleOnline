using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

[System.Obsolete]
public class CustomNetworkManager : NetworkManager
{
    GameManager gameManager;
    bool isHeadless => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;

    private void Start() {
        if(
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer
        ) useWebSockets = false;
        else useWebSockets = true;
        if(Application.platform == RuntimePlatform.WebGLPlayer){
            var hud = GetComponent<NetworkManagerHUD>();
            hud.showGUI = false;
            if(GetURL.GetURLFromPage().Contains("https")) networkPort = 443;
            else networkPort = 80;
            Debug.Log($"Auto starting client on port {networkPort}");
            StartClient();
        }
        if(Application.platform == RuntimePlatform.LinuxPlayer){
            networkPort = 8080;
        }
        if(isHeadless){
            Debug.Log($"Auto starting server on port {networkPort}");
            StartServer();
        }
        gameManager = GameManager.instance;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log(GameManager.instance);
        GameManager.instance.AddPlayer(conn,playerControllerId);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player){
        GameManager.instance.RemovePlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn){
        GameManager.instance.RemovePlayer(conn);
    }

}
