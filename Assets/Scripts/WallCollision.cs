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

  private void Start() {
    killFeed = KillFeed.instance;
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
    NetworkServer.Destroy(obj);
  }

  [ClientRpc]
  void RpcUIKIllFeed(string victim){
    killFeed.AddKillFeedItem(killfeedName,victim);
    Debug.Log($"{killfeedName} has killed {victim}");
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