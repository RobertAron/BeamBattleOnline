using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class GameManager : NetworkBehaviour
{
  [SerializeField] float timeTillPlayersSpawn = 15;
  [SerializeField] GameObject bikePrefab = default;
  [SerializeField] GameObject playerControllerPrefab = default;
  [SerializeField] GameObject dangerSpherePrefab = default;
  Dictionary<NetworkConnection, GameObject> playerConnections = new Dictionary<NetworkConnection, GameObject>();

  public static GameManager instance;
  bool isRestartingGame = false;

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
    if (
      playerConnections.Count > 0 &&
      !ArePlayersLeft() &&
      !isRestartingGame
    )
    {
      StartCoroutine(ResetGameCountdown());
    }
  }

  IEnumerator ResetGameCountdown()
  {
    isRestartingGame = true;
    float timeRemaining = timeTillPlayersSpawn;
    float nextTimeToLog = timeRemaining;
    var gosToClear = GameObject.FindGameObjectsWithTag("ClearAfterGame");
    foreach (var go in gosToClear)
    {
      NetworkServer.Destroy(go);
    }
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
    SpawnGameObjects();
    isRestartingGame = false;
  }

  bool ArePlayersLeft()
  {
    bool noPlayersLeft = GameObject.FindGameObjectWithTag("Player") == null;
    return !noPlayersLeft;
  }

  public void AddPlayer(NetworkConnection playerConnection, short playerId)
  {
    var playerController = (GameObject)Instantiate(playerControllerPrefab);
    NetworkServer.AddPlayerForConnection(playerConnection, playerController, playerId);
    playerConnections.Add(playerConnection, playerController);
  }

  public void RemovePlayer(NetworkConnection connection)
  {
    playerConnections.Remove(connection);
  }


  [ServerCallback]
  void SpawnGameObjects()
  {
    var dangerSphere = (GameObject)Instantiate(dangerSpherePrefab);
    NetworkServer.Spawn(dangerSphere);
    foreach (var ele in playerConnections)
    {
      // Create the players bike
      Vector3 position = new Vector3(UnityEngine.Random.RandomRange(-350f, 350f), 1, UnityEngine.Random.RandomRange(-350, 350));
      Vector3 rotation = new Vector3(0, UnityEngine.Random.RandomRange(0, 3) * 90, 0);
      var playerBike = (GameObject)Instantiate(bikePrefab, position, Quaternion.Euler(rotation));
      NetworkServer.Spawn(playerBike);
      // Set player to link to that bike
      var bikeMovement = playerBike.GetComponent<BikeMovement>();
      var pic = ele.Value.GetComponent<PlayerInputCommunicator>();
      var camGrabber = ele.Value.GetComponent<CamGrabber>();
      pic.SetBike(bikeMovement);
      camGrabber.TargetSetPlayersBike(ele.Key,playerBike);
    }
  }
}
