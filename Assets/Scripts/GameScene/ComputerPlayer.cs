using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComputerPlayer : MonoBehaviour
{
    [SerializeField] BikeMovement playerMovement = default;

    private void OnTriggerEnter(Collider other) {
        TurnIfWall(other,true);
    }
    private void OnTriggerExit(Collider other){
        TurnIfWall(other,false);
    }

    private void TurnIfWall(Collider other,bool dodgeEntering){
        if(!enabled) return;
        WallCollision wallCollision = other.GetComponent<WallCollision>();
        if(wallCollision==null) return;
        if(wallCollision.KillOnEnter() == dodgeEntering){
            bool turnLeft = Random.Range(0,2)==0;
            playerMovement.Turn(turnLeft);
        }
    }
}
