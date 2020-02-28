using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class WallCollision : NetworkBehaviour
{
  [SerializeField] bool destroyOnExit = false;
  [SyncVar][SerializeField] string killfeedName = "";

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
    string killedPlayer = bm.GetPlayerName();
    Debug.Log($"{killfeedName} has killed {killedPlayer}");
    NetworkServer.Destroy(obj);
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