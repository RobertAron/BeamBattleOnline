using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class SafeCircleRender : NetworkBehaviour
{
    [SerializeField] LineRenderer lineRenderer = default;
    Transform minimapPos;
    [SyncVar(hook="UpdateScale")] Vector3 scale;
    void UpdateScale(Vector3 newScale){
        transform.localScale = newScale;
    }

    [ServerCallback]
    public void UpdateSafeArea(Vector3 position, Vector3 scale){
        transform.position = position;
        this.scale = scale;
    }


    public override void OnStartClient(){
        minimapPos = GameObject.FindGameObjectWithTag("MinimapCamera").transform;
        if(minimapPos==null) Debug.LogError("Safe Circle Render could not find the minimap camera");
    }

    void Update(){
        if(minimapPos==null) return;
        Vector3 circleCenter = new Vector3(transform.position.x,10, transform.position.z);
        Vector3 playerPos = new Vector3(minimapPos.position.x,10,minimapPos.position.z);
        lineRenderer.SetPosition(0,circleCenter);
        lineRenderer.SetPosition(1,playerPos);
    }

}
