using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CamGrabber : NetworkBehaviour
{
  GameObject currentTarget;
  bool swappingCam = false;
  FlatDistFollowCam mainCamera;
  FlatDistFollowCam minimapCamera;
  BoostGaugeUiController boostUI;



  [TargetRpc]
  public void TargetSetPlayersBike(NetworkConnection networkConnection,GameObject playersBike){
    ChangeFocus(playersBike);
  }

  void Start() {
    mainCamera = Camera.main.transform.GetComponent<FlatDistFollowCam>();
    minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera")?.GetComponent<FlatDistFollowCam>();
    boostUI = BoostGaugeUiController.instance;
  }

  public void ChangeFocus(GameObject bike){
    currentTarget = bike;
    mainCamera.objectToFollow = bike.transform;
    minimapCamera.objectToFollow = bike.transform;
    BikeMovement newBikeMovement = bike.GetComponent<BikeMovement>();
    boostUI.SubscribeToBike(newBikeMovement);

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
    if(currentTarget==null && go!=null) ChangeFocus(go);
  }

}