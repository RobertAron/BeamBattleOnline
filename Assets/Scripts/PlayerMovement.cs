using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class PlayerMovement : NetworkBehaviour
{
    public float speed;
    // Update is called once per frame
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
    }
}
