using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class WallCollision : NetworkBehaviour
{
  [SerializeField] bool destroyOnExit = false;
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
    if (obj.tag == "Player")
    {
      NetworkServer.Destroy(obj);
    }
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