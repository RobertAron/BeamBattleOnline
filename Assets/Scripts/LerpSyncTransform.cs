using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LerpSyncTransform : NetworkBehaviour
{
    [SerializeField] Transform characterTransform;
    // Auto Updates Clients from server when this is set.
    [SyncVar] private Vector3 syncTransform;
    [SyncVar] private Quaternion syncRotation;
    [SyncVar] private Vector3 syncScale;
    [SerializeField] float lerpRate = 1;

    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition() {
        if(!isLocalPlayer){
            characterTransform.position = Vector3.Lerp(characterTransform.position,syncTransform,Time.fixedDeltaTime*lerpRate);
            characterTransform.rotation = Quaternion.Lerp(characterTransform.rotation,syncRotation,Time.fixedDeltaTime*lerpRate);
            characterTransform.localScale = Vector3.Lerp(characterTransform.localScale,syncScale,Time.fixedDeltaTime*lerpRate);
        }
    }

    // Updates the SERVER ONLY when not used with SyncVar
    [Command]
    void CmdProvideTransformToServer(Vector3 position, Quaternion rotation, Vector3 scale){
        syncTransform = position;
        syncRotation = rotation;
        syncScale = scale;
    }

    // Only runs on the client
    [ClientCallback]
    void TransmitPosition(){
        if(isLocalPlayer){
            CmdProvideTransformToServer(characterTransform.position,characterTransform.rotation,characterTransform.localScale);
        }
    }


}
