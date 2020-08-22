﻿using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

[DisallowMultipleComponent]
public class oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector=new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;
    [Range(0, 1)] [SerializeField] float movementFactor;
    Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time * period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 displacement = movementVector * movementFactor;
        transform.position = startingPosition + displacement;
    }
}
