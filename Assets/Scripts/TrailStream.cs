using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class TrailStream : NetworkBehaviour
{
    [SyncVar] Vector3 startingPos;
    [SyncVar] Vector3 endingPos;
    [SerializeField] GameObject trackedPlayer;
    BikeMovement originator;
    public void StartStream(Vector3 startPosition, BikeMovement bikeMovement, float liftTime,float playerSpeed){
        originator = bikeMovement;
        trackedPlayer = bikeMovement.gameObject;
        startingPos = bikeMovement.transform.position;
        endingPos = startingPos;
        StartCoroutine(DelayClearStream(liftTime,playerSpeed));
    }

    void FixedUpdate() {
        if(isServer){
            if(trackedPlayer==null) return;
            endingPos = trackedPlayer.transform.position;
        }
        transform.position = (startingPos + endingPos)/2;
        float length = (startingPos-endingPos).magnitude;
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y, length);
    }

    public void BreakStream(Vector3 endPosition)
    {
        endingPos = endPosition;
        trackedPlayer = null;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag!="Player" || originator==null) return;
        // todo get the players name and stuff to make killfeed
        originator.IncraseStreamSize();
    }

    IEnumerator DelayClearStream(float lifeTime,float playerSpeed){
        yield return new WaitForSeconds(lifeTime);
        while((startingPos-endingPos).magnitude>float.Epsilon){
            startingPos = Vector3.MoveTowards(startingPos,endingPos,playerSpeed*Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        Destroy(this.gameObject);
    }
}
