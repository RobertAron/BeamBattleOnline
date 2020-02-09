using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CamGrabber : NetworkBehaviour
{
  GameObject currentTarget;
  bool swappingCam = false;
  [TargetRpc]
  public void TargetSetPlayersBike(NetworkConnection networkConnection,GameObject playersBike){
    SetCameraToBike(playersBike);
  }

  public void SetCameraToBike(GameObject bike){
    currentTarget = bike;
    Camera.main.transform.GetComponent<FlatDistFollowCam>().objectToFollow = bike.transform;
    var minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera");
    minimapCamera.GetComponent<FlatDistFollowCam>().objectToFollow = bike.transform;
  }

  void Update(){
    if(currentTarget==null && swappingCam==false) StartCoroutine(PauseThenPickBike());
  }

  IEnumerator PauseThenPickBike(){
    swappingCam = true;
    yield return new WaitForSeconds(4);
    swappingCam = false;
    var go = GameObject.FindGameObjectWithTag("Player");
    // check we haven't assigned to a bike aready, and a new bike exists
    if(currentTarget==null && go!=null) SetCameraToBike(go);
  }

}