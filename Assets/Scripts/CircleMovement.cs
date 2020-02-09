using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class CircleMovement : NetworkBehaviour
{
    [SyncVar] Vector3 scale;
    [SerializeField] float movementPauseTime = 0;
    [SerializeField] float movementTime = 10;
    [SerializeField] GameObject safeCircleRenderPrefab = default;
    SafeCircleRender safeCircleRender;

    void FixedUpdate(){
        transform.localScale = scale;
    }
    
    public override void OnStartServer(){
        scale = transform.localScale;
        var safeCircleGameObject = (GameObject)Instantiate(safeCircleRenderPrefab,transform.position,transform.rotation);
        NetworkServer.Spawn(safeCircleGameObject);
        safeCircleRender = safeCircleGameObject.GetComponent<SafeCircleRender>();

        StartCoroutine(MoveCircleIn());
    }

    IEnumerator MoveCircleIn(){
        Debug.Log("moveing circle in");
        float currentTime = 0;
        Vector3 startingScale = scale;
        Vector3 targetScale = scale/2;
        Vector3 startingPos = transform.position;
        Vector3 xy = Random.insideUnitCircle;
        Vector3 targetPos = new Vector3(xy.x,0,xy.z) * transform.localScale.x + transform.position;
        safeCircleRender.UpdateSafeArea(targetPos,targetScale);
        while(currentTime<movementTime){
            currentTime+=Time.fixedDeltaTime;
            float timePercentage = currentTime/movementTime;
            scale = Vector3.Lerp(startingScale,targetScale,timePercentage);
            transform.position = Vector3.Lerp(startingPos,targetPos,timePercentage);
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = targetScale;
        // transform.position = targetPos;
        StartCoroutine(CountDownCircleTime());
    }

    IEnumerator CountDownCircleTime(){
        yield return new WaitForSeconds(movementPauseTime);
        StartCoroutine(MoveCircleIn());
    }
    
}
