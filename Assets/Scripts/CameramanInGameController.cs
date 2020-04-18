using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramanInGameController : MonoBehaviour {
    [SerializeField] private CameramanMovement _cameramanMovement;
    [SerializeField] private GameObject _cameraEffectUI;

    private bool shootOnCamera = false;

    private void Awake() {
        //_cameraEffectUI.SetActive(false); TODO: uncomment
    }

    private void Update() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 velocity = transform.right * x + transform.forward * z;
        velocity.y = -9.8f * Time.deltaTime;
        
        _cameramanMovement.Move(velocity);
            
        // движение камеры и поворот player
        _cameramanMovement.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButtonDown(1)) {
            if (shootOnCamera) {
                shootOnCamera = false;
            } else {
                shootOnCamera = true;
            }
            _cameraEffectUI.SetActive(!_cameraEffectUI.activeSelf);
        }
    }
}
