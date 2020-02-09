using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CamGrabber : NetworkBehaviour
{

  [TargetRpc]
  public void TargetSetPlayersBike(NetworkConnection networkConnection,GameObject playersBike){
    Camera.main.transform.GetComponent<FlatDistFollowCam>().objectToFollow = playersBike.transform;
    var minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera");
    minimapCamera.GetComponent<FlatDistFollowCam>().objectToFollow = playersBike.transform;
  }
}