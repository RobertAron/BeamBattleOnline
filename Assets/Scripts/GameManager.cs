using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class GameManager : NetworkBehaviour
{
    Dictionary<NetworkConnection,short> playerConnections = new Dictionary<NetworkConnection,short>(); 
    bool hasSpawned = false;
    [SerializeField] float timeTillPlayersSpawn = 15;
    [SerializeField] GameObject playerPrefab;
    float nextCountdownToLog;

    void Awake(){
        nextCountdownToLog = timeTillPlayersSpawn;
    }


    void FixedUpdate() {
        if(isServer) ServerManagerUpdate();
    }

    public void AddPlayer(NetworkConnection playerConnection,short playerId){
        playerConnections.Add(playerConnection,playerId);
    }

    public void RemovePlayer(NetworkConnection connection){
        playerConnections.Remove(connection);
    }

    [ServerCallback]
    void ServerManagerUpdate(){
        timeTillPlayersSpawn -= Time.deltaTime;
        if(timeTillPlayersSpawn<=nextCountdownToLog && nextCountdownToLog>=0){
            Debug.Log($"{nextCountdownToLog} seconds to spawn");
            nextCountdownToLog-=1;
        }
        if(timeTillPlayersSpawn<0 && !hasSpawned) SpawnLoggedInPlayers();
    }

    [ServerCallback]
    void SpawnLoggedInPlayers(){
        hasSpawned = !hasSpawned;
        foreach(var ele in playerConnections){
            
            Vector3 position = new Vector3(UnityEngine.Random.RandomRange(-450f,450f),1,UnityEngine.Random.RandomRange(-450,450));
            Vector3 rotation = new Vector3(0,UnityEngine.Random.RandomRange(0,3)*90,0);
            var player = (GameObject)Instantiate(playerPrefab, position, Quaternion.Euler(rotation));
            NetworkServer.AddPlayerForConnection(ele.Key, player, ele.Value);
        }
    }
}
