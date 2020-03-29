using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[NetworkSettings(sendInterval = 0.01f)]
public class BikeMovement : NetworkBehaviour, Attachable
{
  Rigidbody rb;
  // ==== BOOSTING =====
  [SyncVar] bool isBoosting = false;
  [SerializeField] float lowSpeed = 5;
  [SerializeField] float boostSpeed = 10;

  public delegate void OnVariableChangeDelegate(float newVal);
  public event OnVariableChangeDelegate OnMaxBoostChange;
  [SerializeField]
  [SyncVar(hook="CallMaxBoostChangeDelegate")]
  public float maxBoostTimeAvailable = 3;
  void CallMaxBoostChangeDelegate(float newMaxBoostTime){
    if(OnMaxBoostChange!=null) OnMaxBoostChange(newMaxBoostTime);
  }
  //
  public event OnVariableChangeDelegate OnBoostChange;
  [SyncVar(hook="CallCurrentBoostDelegate")]
  public float currentBoostTimeAvailable = 3;
  void CallCurrentBoostDelegate(float newCurrentBoostTime){
    if(OnBoostChange!=null) OnBoostChange(newCurrentBoostTime);
  }
  // ====================


  
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

  [ServerCallback]
  void FixedUpdate()
  {
    UpdateRigidBody();
  }

  [Server]
  void UpdateRigidBody()
  {
    if(currentBoostTimeAvailable <= 0) SetBoost(false);
    if(isBoosting) currentBoostTimeAvailable -= Time.fixedDeltaTime;
    rb.velocity = transform.forward * (isBoosting ? boostSpeed : lowSpeed);
  }

  [Server]
  public void Turn(bool left)
  {
    float direction = left ? -1 : 1;
    transform.rotation = transform.rotation * Quaternion.Euler(0, 90 * direction, 0);
    StartNewTrail();
  }

  [Server]
  void StartNewTrail()
  {
    GameObject go = Instantiate(trailPrefab, transform.position, transform.rotation);
    NetworkServer.Spawn(go);
    TrailStream newStream = go.GetComponent<TrailStream>();
    newStream.StartStream(this);
    currentStream?.SetAttachment(go);
    currentStream = newStream;
  }

  [Server]
  public void SetPlayerName(string newPlayerName)
  {
    playerName = newPlayerName;
  }
  public string GetPlayerName()
  {
    return playerName;
  }

  Coroutine boostAvailableCoroutine;
  [Server]
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

  [Server]
  IEnumerator RefillBoost(){
    yield return new WaitForSeconds(1);
    while(currentBoostTimeAvailable < maxBoostTimeAvailable){
      currentBoostTimeAvailable += Time.fixedDeltaTime;
      yield return new WaitForFixedUpdate();
    }
    boostAvailableCoroutine = null;
  }


  public Vector3 GetAttachPoint()
  {
    return transform.position;
  }
}
