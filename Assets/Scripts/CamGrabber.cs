using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CamGrabber : NetworkBehaviour
{

  [ClientRpc]
  public void RpcSetPlayersBike(GameObject playersBike){
    Camera.main.transform.GetComponent<FlatDistFollowCam>().objectToFollow = playersBike.transform;
  }
}