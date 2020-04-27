using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CamGrabber : NetworkBehaviour
{
  GameObject currentTarget;
  FlatDistFollowCam mainCamera;
  FlatDistFollowCam minimapCamera;
  BoostGaugeUiController boostUI;

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

  [TargetRpc]
  public void TargetSetPlayersBike(NetworkConnection networkConnection,GameObject playersBike){
    ChangeFocus(playersBike);
  }

  Coroutine currentSwapCoroutine;
  IEnumerator FindTargetAndSwitch(int switchTime){
    yield return new WaitForSeconds(switchTime);
    // check we haven't assigned to a bike aready, and a new bike exists
    var go = GameObject.FindGameObjectWithTag("Player");
    if(go!=null && currentTarget==null) ChangeFocus(go);
    currentSwapCoroutine=null;
  }
  
  [TargetRpc]
  public void TargetSetVictoryCam(NetworkConnection networkConnection){
    if(currentSwapCoroutine==null) StopCoroutine(currentSwapCoroutine);
    StartCoroutine(FindTargetAndSwitch(0));
  }

  void Update(){
    if(currentTarget==null && currentSwapCoroutine==null)
      currentSwapCoroutine = StartCoroutine(FindTargetAndSwitch(2));
  }

}