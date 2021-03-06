﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBikeMovement : MonoBehaviour
{
    [SerializeField] float speed = 20;
    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * speed * transform.forward;
        if(transform.position.z>800) transform.position = transform.position - transform.forward * 800;
    }
}
