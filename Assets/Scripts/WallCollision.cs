﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class WallCollision : NetworkBehaviour
{
    [SerializeField] bool destroyOnExit = false;
    private void OnTriggerEnter(Collider other) {
        if(destroyOnExit) return;
        DestroyPlayer(other.gameObject);

    }

    private void OnTriggerExit(Collider other) {
        if(!destroyOnExit) return;
        DestroyPlayer(other.gameObject);
        
    }

    private void DestroyPlayer(GameObject obj){
        Debug.Log(obj.name);
        if(obj.tag == "Player"){
            NetworkAnimator.Destroy(obj);
        }
    }
}