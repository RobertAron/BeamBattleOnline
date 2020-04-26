using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class ComputerPlayer : MonoBehaviour
{
    [SerializeField] BikeMovement playerMovement = default;

    private void OnTriggerEnter(Collider other) {
        TurnIfWall(other);
    }
    // private void OnTriggerExit(Collider other){
    //     TurnIfWall(other);
    // }

    private void TurnIfWall(Collider other){
        if(!enabled) return;
        WallCollision wallCollision = other.GetComponent<WallCollision>();
        if(wallCollision!=null && wallCollision.KillOnEnter()){
            bool turnLeft = Random.Range(0,2)==0;
            playerMovement.Turn(turnLeft);
        }
    }
}
