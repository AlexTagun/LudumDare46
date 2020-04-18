using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramanInGameController : MonoBehaviour
{
    [SerializeField] private CameramanMovement _cameramanMovement;
    [SerializeField] private GameObject _cameraEffectUI;

    private bool shootOnCamera = false;

    private void Awake()
    {
        _cameraEffectUI.SetActive(false);
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
        // движение камеры и поворот player
        _cameramanMovement.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButtonDown(1))
        {
            if (shootOnCamera)
            {
                shootOnCamera = false;
            }
            else
            {
                shootOnCamera = true;
            }
            _cameraEffectUI.SetActive(!_cameraEffectUI.activeSelf);
        }
    }
}
