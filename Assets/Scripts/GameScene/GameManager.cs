using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[System.Obsolete]
public class GameManager : NetworkBehaviour
{
    [SerializeField] int timeTillPlayersSpawn = 4;
    [SerializeField] GameObject bikePrefab = default;
    [SerializeField] GameObject playerControllerPrefab = default;
    [SerializeField] GameObject dangerSpherePrefab = default;
    [SerializeField] int targetPlayerCount = 30;
    [SerializeField] RemainingPlayerTextSetter remainingPlayerTextSetter = default;
    Dictionary<NetworkConnection, GameObject> playerConnections = new Dictionary<NetworkConnection, GameObject>();
    List<GameObject> bikesAlive = new List<GameObject>();
    bool isGameRunning;

    #region  Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    void FixedUpdate()
    {
        if (playerConnections.Count > 0 && !isGameRunning)
        {
            StartCoroutine(StartGameSequence());
        }
    }

    IEnumerator StartGameSequence()
    {
        isGameRunning = true;
        for (int timeRemaining = timeTillPlayersSpawn; timeRemaining > 0; timeRemaining--)
        {
            Debug.Log($"Time Till Reset {timeRemaining}");
            yield return new WaitForSeconds(1);
        }
        SpawnGameObjects();
        while (bikesAlive.Count > 1)
        {
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(VictorySequence());
    }

    IEnumerator VictorySequence()
    {
        foreach (var ele in playerConnections)
        {
            var camGrabber = ele.Value.GetComponent<CamGrabber>();
            camGrabber.TargetSetVictoryCam(ele.Key);
        }
        if(bikesAlive.Count>0){
            var winningPlayer = playerConnections.ToList().Where((KeyValuePair<NetworkConnection, GameObject> kv)=>{
                var pic = kv.Value.GetComponent<PlayerInputCommunicator>();
                if(pic==null) return false; 
                return pic.HasBike();
            });
            var bikeThatWon = bikesAlive[0];
            var bikeVictoryStuff = bikeThatWon.GetComponentInChildren<BikeVictoryStuff>();
            if(winningPlayer.Any()) bikeVictoryStuff.TargetSetWinningPlayer(winningPlayer.First().Key);
            bikeVictoryStuff.RpcWinAnimationStuff();
        }
        yield return new WaitForSeconds(4);
        var gosToClear = GameObject.FindGameObjectsWithTag("ClearAfterGame").ToList();
        gosToClear.AddRange(GameObject.FindGameObjectsWithTag("Player").ToList());
        foreach (var go in gosToClear)
        {
            NetworkServer.Destroy(go);
        }
        isGameRunning = false;
    }

    public void AddPlayer(NetworkConnection playerConnection, short playerId)
    {
        var playerController = (GameObject)Instantiate(playerControllerPrefab);
        NetworkServer.AddPlayerForConnection(playerConnection, playerController, playerId);
        playerConnections.Add(playerConnection, playerController);
    }

    public void RemovePlayer(NetworkConnection connection)
    {
        var go = playerConnections[connection];
        PlayerInputCommunicator pic = go.GetComponent<PlayerInputCommunicator>();
        var bikeGo = pic.GetBike();
        NetworkManager.Destroy(bikeGo);
        playerConnections.Remove(connection);
        NetworkManager.Destroy(go);
        NetworkServer.DestroyPlayersForConnection(connection);
    }


    [ServerCallback]
    void SpawnGameObjects()
    {
        var dangerSphere = (GameObject)Instantiate(dangerSpherePrefab);
        NetworkServer.Spawn(dangerSphere);
        foreach (var ele in playerConnections)
        {
            // Set player to link to that bike
            var pic = ele.Value.GetComponent<PlayerInputCommunicator>();
            var playerBike = SpawnBike(pic.GetPlayerName(), pic.accentColor);
            var bikeMovement = playerBike.GetComponent<BikeMovement>();
            var camGrabber = ele.Value.GetComponent<CamGrabber>();
            pic.SetBike(bikeMovement);
            camGrabber.TargetSetPlayersBike(ele.Key, playerBike);
        }
        int computersToSpawn = targetPlayerCount - playerConnections.Count;
        for (var i = 0; i < computersToSpawn; ++i)
        {
            var playerBike = SpawnBike(PlayerPrefsController.defaultPlayerName,GenerateRandomColor());
            playerBike.GetComponentInChildren<ComputerPlayer>().enabled = true;
        }
        UpdateRemainingPlayersUI();
    }

    [ServerCallback]
    GameObject SpawnBike(String name,Color color)
    {
        // Create the players bike
        Vector3 position = new Vector3(UnityEngine.Random.RandomRange(-350f, 350f), 1, UnityEngine.Random.RandomRange(-350, 350));
        Vector3 rotation = new Vector3(0, UnityEngine.Random.RandomRange(0, 3) * 90, 0);
        var playerBike = (GameObject)Instantiate(bikePrefab, position, Quaternion.Euler(rotation));
        var bm = playerBike.GetComponent<BikeMovement>();
        bm.SetPlayerSettings(name,color);
        NetworkServer.Spawn(playerBike);
        bikesAlive.Add(playerBike);
        return playerBike;
    }

    public void RemoveBikeFromAlivePlayers(GameObject bikeToRemove)
    {
        bikesAlive.Remove(bikeToRemove);
        UpdateRemainingPlayersUI();
    }

    void UpdateRemainingPlayersUI()
    {
        remainingPlayerTextSetter.RpcUpdatePlayersRemaining(bikesAlive.Count);
    }

    Color GenerateRandomColor()
    {
        float valueSpread = 0.25f;
        float hueSpread = 1f / 9f;

        int column = (int)Mathf.Floor(UnityEngine.Random.Range(0, 8));
        int row = (int)Mathf.Floor(UnityEngine.Random.Range(0, 4));
        return Color.HSVToRGB(
          hueSpread * column,
          .25f + valueSpread * row,
          .75f
        );
    }
}
