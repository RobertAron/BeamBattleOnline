using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IsLocalEnabler : NetworkBehaviour
{
    [SerializeField] Camera playerCamera = default;
    [SerializeField] AudioListener audioListener = default;
    [SerializeField] InputManager inputManager = default;
    [SerializeField] FlatDistFollowCam flatDistFollow = default;
    [SerializeField] PlayerMovement playerMovement = default;

    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer){
            playerCamera.enabled = true;
            audioListener.enabled = true;
            inputManager.enabled = true;
            flatDistFollow.enabled = true;
            playerMovement.enabled = true;
        }
    }
}
