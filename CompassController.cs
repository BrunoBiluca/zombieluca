using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    public Transform Player { get; private set; }
    public int RegisteredObjectsCount = 0;
    public Dictionary<Transform, CompassTrackedObject> compassObjects;

    public void Setup(Transform player)
    {
        Player = player != null ? player 
            : throw new ArgumentNullException("Player can't be null");
    }

    public void Start()
    {

    }

    public void Update()
    {

    }

}
