using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class PlayerMovement : NetworkBehaviour
{
    public float speed;
    [SerializeField] GameObject trailPrefab;
    TrailStream currentStream = null;
    void FixedUpdate()
    {
        UpdatePosition();
    }

    void UpdatePosition(){
        transform.position += transform.forward*speed*Time.deltaTime;
    }

    public void Turn(bool left){
        float direction = left?-1:1;
        transform.rotation = transform.rotation*Quaternion.Euler(0,90*direction,0);
        CmdStartNewTrail(transform.rotation);
    }

    [Command]
    void CmdStartNewTrail(Quaternion t){
        if(currentStream!=null) currentStream.BreakStream();
        GameObject go = Instantiate(trailPrefab,transform.position,t);
        currentStream = go.GetComponent<TrailStream>();
        currentStream.StartStream(transform.gameObject);
        NetworkServer.Spawn(go);
    }
}
