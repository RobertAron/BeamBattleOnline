using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class PlayerInputCommunicator : NetworkBehaviour
{
  BikeMovement bikeMovement;
  [SerializeField] string playerName;
  [SerializeField] public Color primaryColor;
  [SerializeField] public Color accentColor;
  PlayerPrefsController playerPrefsController = new PlayerPrefsController();

  void Update(){
      if(!isLocalPlayer) return;
      if(Input.GetKeyDown(KeyCode.LeftArrow)) CmdTurnPlayer(true);
      if(Input.GetKeyDown(KeyCode.RightArrow)) CmdTurnPlayer(false);
      if(Input.GetKeyDown(KeyCode.Z)) CmdSetPlayerBoost(true);
      if(Input.GetKeyUp(KeyCode.Z)) CmdSetPlayerBoost(false);
  }

  public void Start(){
    if(!isClient) return;
    CmdSetPlayerSettings(playerPrefsController.playerName,playerPrefsController.primaryColor,playerPrefsController.accentColor);
  }

  [ServerCallback]
  public void SetBike(BikeMovement bikeMovement)
  {
    this.bikeMovement = bikeMovement;
  }


  [Command]
  void CmdTurnPlayer(bool left)
  {
    if (bikeMovement != null) bikeMovement.Turn(left);
  }

  [Command]
  void CmdSetPlayerSettings(string name, Color primary, Color accent){
    playerName = name;
    primaryColor = primary;
    accentColor = accent;
  }

  [Command]
  void CmdSetPlayerBoost(bool boost){
    if (bikeMovement != null)  bikeMovement.SetBoost(boost);
  }

  public string GetPlayerName(){
    return playerName;
  }
}
