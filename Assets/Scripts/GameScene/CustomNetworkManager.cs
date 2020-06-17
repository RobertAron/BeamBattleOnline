using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CustomNetworkManager : NetworkManager
{
    GameManager gameManager;

    public override void Start() {
        base.Start();
        GetComponent<Mirror.NetworkManagerHUD>().showGUI = true;
        gameManager = GameManager.instance;
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        gameManager.AddPlayer(conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, NetworkIdentity player){
        gameManager.RemovePlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn){
        gameManager.RemovePlayer(conn);
    }

}
