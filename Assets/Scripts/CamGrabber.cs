using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CamGrabber : NetworkBehaviour
{
  void Start()
  {
    if (!isLocalPlayer) return;
    Camera.main.transform.GetComponent<FlatDistFollowCam>().objectToFollow = transform;
  }
}
