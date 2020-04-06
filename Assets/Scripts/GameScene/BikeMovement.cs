using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[NetworkSettings(sendInterval = 0.01f)]
public class BikeMovement : NetworkBehaviour, Attachable
{
  [SerializeField] MeshRenderer shipMR = default;
  private MaterialPropertyBlock shipMaterialBlock;

  Rigidbody rb;
  // ==== BOOSTING =====
  [SyncVar] bool isBoosting = false;
  [SerializeField] float lowSpeed = 5;
  [SerializeField] float boostSpeed = 10;

  public delegate void OnVariableChangeDelegate(float newVal);
  public event OnVariableChangeDelegate OnMaxBoostChange;
  [SerializeField]
  [SyncVar(hook="CallMaxBoostChangeDelegate")]
  public float maxBoostTimeAvailable = 1;
  void CallMaxBoostChangeDelegate(float newMaxBoostTime){
    if(OnMaxBoostChange!=null) OnMaxBoostChange(newMaxBoostTime);
  }
  //
  public event OnVariableChangeDelegate OnBoostChange;
  [SyncVar(hook="CallCurrentBoostDelegate")]
  public float currentBoostTimeAvailable = 1;
  void CallCurrentBoostDelegate(float newCurrentBoostTime){
    currentBoostTimeAvailable = newCurrentBoostTime;
    if(OnBoostChange!=null) OnBoostChange(newCurrentBoostTime);
  }
  // ====================


  
  [SerializeField] GameObject trailPrefab = default;
  string playerName;
  [SerializeField][SyncVar(hook="SetPrimaryColor")] Color primaryColor = new Color();
  void SetPrimaryColor(Color color){
    primaryColor = color;
    shipMaterialBlock.SetColor("_PrimaryColor", color);
    shipMR.SetPropertyBlock(shipMaterialBlock);
  }
  [SerializeField][SyncVar(hook="SetAccentColor")] Color accentColor = new Color();
  void SetAccentColor(Color color){
    accentColor = color;
    shipMaterialBlock.SetColor("_AccentColor", color);
    shipMR.SetPropertyBlock(shipMaterialBlock);
  }
  public Color GetAccentColor(){
    return accentColor;
  }
  TrailStream currentStream = null;

  override public void OnStartServer()
  {
    StartNewTrail();
  }

  void Awake()
  {
    rb = GetComponent<Rigidbody>();
    shipMaterialBlock = new MaterialPropertyBlock();
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
    if(isBoosting) CallCurrentBoostDelegate(currentBoostTimeAvailable - Time.fixedDeltaTime);
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
  public void SetPlayerSettings(string newPlayerName,Color newPrimaryColor, Color newAccentColor)
  {
    SetPrimaryColor(newPrimaryColor);
    SetAccentColor(newAccentColor);
    playerName = newPlayerName;
    if(currentStream!=null) currentStream.SetTrailColor(newAccentColor);
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
      CallCurrentBoostDelegate(currentBoostTimeAvailable + Time.fixedDeltaTime);
      yield return new WaitForFixedUpdate();
    }
    boostAvailableCoroutine = null;
  }


  public Vector3 GetAttachPoint()
  {
    return transform.position;
  }

  public void IncreaseBoostSize(){
    maxBoostTimeAvailable +=.25f;
    CallCurrentBoostDelegate(maxBoostTimeAvailable);
  }
}
