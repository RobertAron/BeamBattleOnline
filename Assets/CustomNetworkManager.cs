using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CustomNetworkManager : NetworkManager
{
    Dictionary<short,NetworkConnection> playerConnections = new Dictionary<short,NetworkConnection>(); 
    public float countDown = 15;
    public bool hasSpawned = false;
    private void FixedUpdate() {
        countDown -= Time.deltaTime;
        if(countDown<0 && !hasSpawned){
            Debug.Log("Spawning in...");
            hasSpawned = !hasSpawned;
            foreach(KeyValuePair<short,NetworkConnection> ele in playerConnections){
                var player = (GameObject)Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
                NetworkServer.AddPlayerForConnection(ele.Value, player, ele.Key);
            }

        }

    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        playerConnections.Add(playerControllerId,conn);
    }
}
