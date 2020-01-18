using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class WallCollision : NetworkBehaviour
{
    [ServerCallback]
    private void OnCollisionEnter(Collision other) {
        Debug.Log("COLLIDE");
        NetworkServer.Destroy(other.gameObject);
    }
}
