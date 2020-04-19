using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramanMovement : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private CharacterController _characterController;

    public float JumpSpeed => _jumpSpeed;
    // вращение камеры
    private float mouseDeltaX;
    private float mouseDeltaY;
    private Quaternion startRotation;
    private Quaternion verticalRotation;
    private Quaternion horizontalRotarion;

    private void Awake() {
        startRotation = transform.rotation;
    }

    public void Rotate(float mouseDeltaX, float mouseDeltaY) {
        this.mouseDeltaX += mouseDeltaX * _speedRotation;
        this.mouseDeltaY += mouseDeltaY * _speedRotation;
        this.mouseDeltaY = Mathf.Clamp(this.mouseDeltaY, -60, 60);
        this.gameObject.transform.rotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        verticalRotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        horizontalRotarion = Quaternion.AngleAxis(-this.mouseDeltaY, Vector3.right);
        _camera.transform.rotation = startRotation * verticalRotation * horizontalRotarion;
    }

    public void Move(Vector3 vel) {
        vel.x *= _speed * Time.deltaTime;
        vel.z *= _speed * Time.deltaTime;
        _characterController.Move(vel);
    }


}
