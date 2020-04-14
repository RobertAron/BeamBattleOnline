using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CustomNetworkManager : NetworkManager
{
    GameManager gameManager;


    private void Start() {
        #if WEB
            useWebSockets = true;
        #else
            useWebSockets = false;
        #endif
        
        #if SERVER_BUILD
            Debug.Log("Server Directives! Hosting Game.");
            string serverType = useWebSockets?"Web Sockets":"TLC Connection";
            Debug.Log($"Using Connection type {serverType}");
            StartServer();
        #elif CLIENT_BUILD
            Debug.Log("Client Directives! Connecting to Game.");
            StartClient();
        #else
            GetComponent<UnityEngine.Networking.NetworkManagerHUD>().showGUI = true;
        #endif
        gameManager = GameManager.instance;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        gameManager.AddPlayer(conn,playerControllerId);
    }

    public override void OnServerDisconnect(NetworkConnection conn){
        gameManager.RemovePlayer(conn);
    }

}
