using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class GameManager : NetworkBehaviour
{
  [SerializeField] float timeTillPlayersSpawn = 15;
  [SerializeField] GameObject playerPrefab = default;

  List<GameObject> remainingPlayers = new List<GameObject>();
  Dictionary<NetworkConnection, short> playerConnections = new Dictionary<NetworkConnection, short>();

  public static GameManager instance;
  bool isCountingDown = false;

  #region  Singleton
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
    if (isServer) ServerFixedUpdate();
  }

  [ServerCallback]
  void ServerFixedUpdate()
  {
    if (
      playerConnections.Count > 0 &&
      !ArePlayersLeft() &&
      !isCountingDown
    )
    {
      StartCoroutine(SpawnCountDown());
    }
  }

  IEnumerator SpawnCountDown()
  {
    isCountingDown = true;
    float timeRemaining = timeTillPlayersSpawn;
    float nextTimeToLog = timeRemaining;
    while (timeRemaining >= 0)
    {
      timeRemaining -= Time.fixedDeltaTime;
      if (timeRemaining <= nextTimeToLog)
      {
        Debug.Log($"{nextTimeToLog} seconds till spawn");
        nextTimeToLog -= 1;
      }
      yield return new WaitForFixedUpdate();
    }
    SpawnConnectedPlayers();
    isCountingDown = false;
  }

  bool ArePlayersLeft()
  {
    bool noPlayersLeft = GameObject.FindGameObjectWithTag("Player") == null;
    return !noPlayersLeft;
  }

  public void AddPlayer(NetworkConnection playerConnection, short playerId)
  {
    playerConnections.Add(playerConnection, playerId);
  }

  public void RemovePlayer(NetworkConnection connection)
  {
    playerConnections.Remove(connection);
  }


  [ServerCallback]
  void SpawnConnectedPlayers()
  {
    foreach (var ele in playerConnections)
    {
      Vector3 position = new Vector3(UnityEngine.Random.RandomRange(-350f, 350f), 1, UnityEngine.Random.RandomRange(-350, 350));
      Vector3 rotation = new Vector3(0, UnityEngine.Random.RandomRange(0, 3) * 90, 0);
      var player = (GameObject)Instantiate(playerPrefab, position, Quaternion.Euler(rotation));
      remainingPlayers.Add(player);
      NetworkServer.AddPlayerForConnection(ele.Key, player, ele.Value);
    }
  }
}
