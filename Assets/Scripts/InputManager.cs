using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent: UnityEvent<bool>{}

[System.Obsolete]
public class InputManager : NetworkBehaviour
{
    [SerializeField] BoolEvent turnPlayer;

    void Update(){
        if(!isLocalPlayer) return;
        if(Input.GetKeyDown(KeyCode.LeftArrow)) turnPlayer.Invoke(true);
        if(Input.GetKeyDown(KeyCode.RightArrow)) turnPlayer.Invoke(false);
    }
}