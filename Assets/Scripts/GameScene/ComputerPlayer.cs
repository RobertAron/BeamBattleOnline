using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class ComputerPlayer : MonoBehaviour
{
    [SerializeField] BikeMovement playerMovement = default;
    bool turnLeft = false;

    private void Update() {
        int shouldFlipTurn = Random.Range(0,2);
        if(shouldFlipTurn==0) turnLeft = !turnLeft;
    }

    private void OnTriggerEnter(Collider other) {
        if(!enabled) return;
        WallCollision wallCollision = other.GetComponent<WallCollision>();
        if(wallCollision!=null && wallCollision.KillOnEnter()) playerMovement.Turn(turnLeft);
    }
    private void OnTriggerExit(Collider other){
        if(!enabled) return;
        WallCollision wallCollision = other.GetComponent<WallCollision>();
        if(wallCollision!=null && wallCollision.KillOnExit()) playerMovement.Turn(turnLeft);
    }
}
