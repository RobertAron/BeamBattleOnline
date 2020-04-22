using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class PlayerInputCommunicator : NetworkBehaviour
{
  BikeMovement bikeMovement;
  [SerializeField][SyncVar] GameObject playerBikeGo;
  [SerializeField][SyncVar] string playerName;
  [SerializeField] public Color accentColor;
  [SerializeField] GameObject playerWaitingUI;
  PlayerPrefsController playerPrefsController = new PlayerPrefsController();

  void Update(){
      if(!isLocalPlayer) return;
      if(Input.GetKeyDown(KeyCode.LeftArrow)) CmdTurnPlayer(true);
      if(Input.GetKeyDown(KeyCode.RightArrow)) CmdTurnPlayer(false);
      if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space)) CmdSetPlayerBoost(true);
      if(Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Space)) CmdSetPlayerBoost(false);
      playerWaitingUI.active = playerBikeGo==null;
  }

  public bool HasBike(){
    return playerBikeGo!=null;
  }

  public void Start(){
    if(isLocalPlayer) CmdSetPlayerSettings(playerPrefsController.playerName,playerPrefsController.accentColor);
    playerWaitingUI = GameObject.FindGameObjectWithTag("PlayerWaitingUi");
  }

  [ServerCallback]
  public void SetBike(BikeMovement bikeMovement)
  {
    this.bikeMovement = bikeMovement;
    playerBikeGo = bikeMovement.gameObject;
  }

  [ServerCallback]
  public GameObject GetBike(){
    return this.bikeMovement.gameObject;
  }


  [Command]
  void CmdTurnPlayer(bool left)
  {
    if (bikeMovement != null) bikeMovement.Turn(left);
  }

  [Command]
  void CmdSetPlayerSettings(string name, Color accentColor){
    playerName = name;
    this.accentColor = accentColor;
  }

  [Command]
  void CmdSetPlayerBoost(bool boost){
    if (bikeMovement != null)  bikeMovement.SetBoost(boost);
  }

  public string GetPlayerName(){
    return playerName;
  }
}
