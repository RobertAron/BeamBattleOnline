using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[NetworkSettings(sendInterval = 0.01f)]
public class BikeMovement : NetworkBehaviour, Attachable
{
  Rigidbody rb;
  bool isBoosting = false;
  [SerializeField] float lowSpeed = 5;
  [SerializeField] float boostSpeed = 10;
  [SerializeField] float maxBoostTimeAvailable = 3;
  [SerializeField] float currentBoostTimeAvailable = 3;
  [SerializeField] GameObject trailPrefab = default;
  string playerName;
  TrailStream currentStream = null;

  override public void OnStartServer()
  {
    StartNewTrail();
  }

  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    UpdateRigidBody();
  }

  void UpdateRigidBody()
  {
    if(currentBoostTimeAvailable<=0) SetBoost(false);
    currentBoostTimeAvailable -= isBoosting?Time.fixedDeltaTime:0;
    rb.velocity = transform.forward * (isBoosting ? boostSpeed : lowSpeed);
  }

  public void Turn(bool left)
  {
    float direction = left ? -1 : 1;
    transform.rotation = transform.rotation * Quaternion.Euler(0, 90 * direction, 0);
    StartNewTrail();
  }

  void StartNewTrail()
  {
    GameObject go = Instantiate(trailPrefab, transform.position, transform.rotation);
    NetworkServer.Spawn(go);
    TrailStream newStream = go.GetComponent<TrailStream>();
    newStream.StartStream(this);
    currentStream?.SetAttachment(go);
    currentStream = newStream;
  }

  public void SetPlayerName(string newPlayerName)
  {
    playerName = newPlayerName;
  }
  public string GetPlayerName()
  {
    return playerName;
  }

  Coroutine boostAvailableCoroutine;
  public void SetBoost(bool shouldBoost)
  {
    isBoosting = shouldBoost;
    if(
      shouldBoost &&
      boostAvailableCoroutine!=null
    ) {
      StopCoroutine(boostAvailableCoroutine);
      boostAvailableCoroutine = null;
    }
    if(
      !shouldBoost &&
      boostAvailableCoroutine==null
    ) boostAvailableCoroutine = StartCoroutine(RefillBoost());
  }

  IEnumerator RefillBoost(){
    yield return new WaitForSeconds(2);
    while(currentBoostTimeAvailable<maxBoostTimeAvailable){
      currentBoostTimeAvailable+= Time.fixedDeltaTime;
      yield return new WaitForFixedUpdate();
    }
    boostAvailableCoroutine = null;
  }

  public Vector3 GetAttachPoint()
  {
    return transform.position;
  }
}
