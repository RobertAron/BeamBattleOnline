using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[NetworkSettings(sendInterval=0.005f)]
public class CircleMovement : NetworkBehaviour
{
    [SyncVar] Vector3 scale;
    [SyncVar] Vector3 position;
    [SerializeField] float movementPauseTime = 0;
    [SerializeField] float movementTime = 10;
    [SerializeField] GameObject safeCircleRenderPrefab = default;
    SafeCircleRender safeCircleRender;

    void FixedUpdate(){
        transform.localScale = scale;
        transform.position = position;
    }
        
    public override void OnStartServer(){
        scale = transform.localScale;
        position = transform.position;
        var safeCircleGameObject = Instantiate(safeCircleRenderPrefab,transform.position,transform.rotation);
        safeCircleRender = safeCircleGameObject.GetComponent<SafeCircleRender>();
        NetworkServer.Spawn(safeCircleGameObject);
        StartCoroutine(MoveCircleIn());
    }

    IEnumerator MoveCircleIn(){
        float currentTime = 0;
        Vector3 startingScale = scale;
        Vector3 targetScale = scale/2;
        Vector3 startingPos = transform.position;
        Vector3 xy = Random.insideUnitCircle;
        Vector3 targetPos = new Vector3(xy.x,0,xy.z) * (transform.localScale.x-targetScale.x/2) + transform.position;
        safeCircleRender.UpdateSafeArea(targetPos,targetScale);
        while(currentTime<movementTime){
            currentTime+=Time.fixedDeltaTime;
            float timePercentage = currentTime/movementTime;
            scale = Vector3.Lerp(startingScale,targetScale,timePercentage);
            position = Vector3.Lerp(startingPos,targetPos,timePercentage);
            yield return new WaitForFixedUpdate();
        }
        scale = targetScale;
        position = targetPos;
        yield return new WaitForFixedUpdate();
        StartCoroutine(CountDownCircleTime());
    }

    IEnumerator CountDownCircleTime(){
        yield return new WaitForSeconds(movementPauseTime);
        StartCoroutine(MoveCircleIn());
    }
    
}
