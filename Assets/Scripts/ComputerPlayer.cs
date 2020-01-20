using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    bool turnLeft = false;

    private void Update() {
        int shouldFlipTurn = Random.Range(0,2);
        if(shouldFlipTurn==0) turnLeft = !turnLeft;
    }

    private void OnTriggerEnter(Collider other) {
        playerMovement.Turn(turnLeft);
    }
}
