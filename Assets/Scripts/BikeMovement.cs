using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class BikeMovement : NetworkBehaviour
{
    Rigidbody rb;
    public float speed;
    [SerializeField] GameObject trailPrefab = default;
    TrailStream currentStream = null;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    [ServerCallback]
    void UpdatePosition(){
        // transform.position += transform.forward*speed*Time.deltaTime;
        rb.velocity = transform.forward*speed;
    }

    public void Turn(bool left){
        float direction = left?-1:1;
        transform.rotation = transform.rotation*Quaternion.Euler(0,90*direction,0);
        StartNewTrail(transform.position, transform.rotation );
    }
    
    void StartNewTrail(Vector3 startPosition, Quaternion t){
        if(currentStream!=null) currentStream.BreakStream(startPosition);
        GameObject go = Instantiate(trailPrefab,transform.position,t);
        currentStream = go.GetComponent<TrailStream>();
        currentStream.StartStream(startPosition, transform.gameObject);
        NetworkServer.Spawn(go);
    }
}
