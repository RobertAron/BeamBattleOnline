using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CustomNetworkManager : NetworkManager
{
    
    List<Tuple<short,NetworkConnection>> playerConnections = new List<Tuple<short,NetworkConnection>>(); 
    [SerializeField] float timeTillPlayersSpawn = 15;
    [SerializeField] bool isServer;
    bool hasSpawned = false;

    private void Start() {
        #if SERVER_BUILD
            Debug.Log("SERVER!");
            StartServer();
        #elif CLIENT_BUILD
            Debug.Log("Client!");
            StartClient();
        #else
            GetComponent<UnityEngine.Networking.NetworkManagerHUD>().showGUI = true;
        #endif
    }

    void FixedUpdate() {
        timeTillPlayersSpawn -= Time.deltaTime;
        if(timeTillPlayersSpawn<0 && !hasSpawned) SpawnLoggedInPlayers();
    }

    void SpawnLoggedInPlayers(){
        hasSpawned = !hasSpawned;
        foreach(var ele in playerConnections){
            
            Vector3 position = new Vector3(UnityEngine.Random.RandomRange(-450f,450f),1,UnityEngine.Random.RandomRange(-450,450));
            Vector3 rotation = new Vector3(0,UnityEngine.Random.RandomRange(0,3)*90,0);
            var player = (GameObject)Instantiate(playerPrefab, position, Quaternion.Euler(rotation));
            NetworkServer.AddPlayerForConnection(ele.Item2, player, ele.Item1);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        playerConnections.Add(Tuple.Create(playerControllerId,conn));
    }
}
