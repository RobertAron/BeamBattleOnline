using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class WallCollision : NetworkBehaviour
{
  KillFeed killFeed;
  [SerializeField] bool destroyOnExit = false;
  [SyncVar][SerializeField] string killfeedName = "";
  TrailStream trailStream = null;

  private void Start() {
    killFeed = KillFeed.instance;
    trailStream = GetComponent<TrailStream>();
  }

  public void SetKillfeedName(string newKillfeedName){
    killfeedName = newKillfeedName;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (destroyOnExit) return;
    DestroyPlayer(other.gameObject);

  }

  private void OnTriggerExit(Collider other)
  {
    if (!destroyOnExit) return;
    DestroyPlayer(other.gameObject);

  }

  [ServerCallback]
  private void DestroyPlayer(GameObject obj)
  {
    var bm = obj.GetComponent<BikeMovement>();
    if (bm == null) return;
    string victim = bm.GetPlayerName();
    RpcUIKIllFeed(victim);
    if(trailStream) trailStream.IncreasePlayerTakedowns();
    NetworkServer.Destroy(obj);
  }

  [ClientRpc]
  void RpcUIKIllFeed(string victim){
    killFeed.AddKillFeedItem(killfeedName,victim);
  }

  public bool KillOnEnter()
  {
    return !destroyOnExit;
  }
  public bool KillOnExit()
  {
    return destroyOnExit;
  }
}