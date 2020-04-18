﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManInGameController : MonoBehaviour
{
    [SerializeField] private CameraManMovement _cameramanMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _cameramanMovement.MoveForward();
        }

        if (Input.GetKey(KeyCode.A))
        {
            _cameramanMovement.MoveLeft();
        }

        if (Input.GetKey(KeyCode.S))
        {
            _cameramanMovement.MoveBack();
        }

        if (Input.GetKey(KeyCode.D))
        {
            _cameramanMovement.MoveRight();
        }
    }
}
