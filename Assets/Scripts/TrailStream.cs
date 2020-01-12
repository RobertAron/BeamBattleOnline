using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class TrailStream : NetworkBehaviour
{
    [SyncVar] Vector3 startingPos;
    [SyncVar] Vector3 endingPos;
    [SerializeField] GameObject trackedPlayer;
    public void StartStream(GameObject playerGameObject){
        trackedPlayer = playerGameObject;
        startingPos = playerGameObject.transform.position;
        endingPos = playerGameObject.transform.position;
    }

    void FixedUpdate() {
        if(isServer){
            if(trackedPlayer==null) return;
            endingPos = trackedPlayer.transform.position;
        }
        transform.position = (startingPos + endingPos)/2;
        float length = (startingPos-endingPos).magnitude;
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y, length);
    }

    public void BreakStream()
    {
        trackedPlayer = null;
    }
}
