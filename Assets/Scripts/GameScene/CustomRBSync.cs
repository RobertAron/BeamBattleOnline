using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[NetworkSettings(sendInterval = 0.005f)]
public class CustomRBSync : NetworkBehaviour
{
    [SerializeField] float snapDistance = 4;
    [SyncVar] Vector3 velocity = new Vector3(0,0,0);
    [SyncVar] Vector3 targetPosition = new Vector3(0,0,0);
    [SyncVar] Quaternion targetRotation = Quaternion.identity;
    [Range(0f, 1f)]
    [SerializeField] float lerpPercent = .1f;
    [Range(0f,1f)]
    [SerializeField] float movePercent = .9f;
    [SerializeField] bool debugPrediction = false;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    private void FixedUpdate() {
        if(isServer) ServerFixedUpdate();
        else ClientFixedUpdate();
    }

    [Server]
    void ServerFixedUpdate(){
        if(Vector3.Distance(velocity,rb.velocity)>float.Epsilon) velocity = rb.velocity;
        if(Vector3.Distance(targetPosition,transform.position)>float.Epsilon) targetPosition = transform.position;
        if(Vector3.Distance(
            targetRotation.ToEuler(),
            transform.rotation.ToEuler()
        )>float.Epsilon) targetRotation = transform.rotation;
    }

    void ClientFixedUpdate(){
        if(isServer) return;
        // simulate predicted movement
        targetPosition += velocity * Time.fixedDeltaTime;

        transform.rotation = targetRotation;
        // Sanp to target
        if(Vector3.Distance(transform.position,targetPosition)>snapDistance){
            transform.position = targetPosition;
        }
        // Lerp to target
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            movePercent*velocity.magnitude
        ); 
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            lerpPercent
        );
    }

    private void OnDrawGizmos() {
        if(debugPrediction) Gizmos.DrawCube(targetPosition,Vector3.one);
    }

}
