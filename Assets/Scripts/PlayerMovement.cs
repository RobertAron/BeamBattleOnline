using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward*speed*Time.deltaTime;
    }

    public void TurnLeft(){
        transform.rotation *= Quaternion.Euler(0,-90,0);
    }

    public void TurnRight(){
        transform.rotation *= Quaternion.Euler(0,90,0);
    }
}
