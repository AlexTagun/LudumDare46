using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramanMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;

    private float verticalSpeed;
    private float horizontalSpeed;
    // вращение камеры
    private float mouseDeltaX;
    private float mouseDeltaY;
    private Quaternion startRotation;
    private Quaternion verticalRotation;
    private Quaternion horizontalRotarion;

    private void Awake()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveForward()
    {
        verticalSpeed = _speed * Time.deltaTime;
        horizontalSpeed = 0f;
        Move();
    }

    public void MoveBack()
    {
        verticalSpeed = (-1) * _speed * Time.deltaTime;
        horizontalSpeed = 0f;
        Move();
    }
    public void MoveLeft()
    {
        horizontalSpeed = (-1) * _speed * Time.deltaTime;
        verticalSpeed = 0f;
        Move();
    }

    public void MoveRight()
    {
        horizontalSpeed = _speed * Time.deltaTime;
        verticalSpeed = 0f;
        Move();
    }

    public void Rotate(float mouseDeltaX, float mouseDeltaY)
    {
        this.mouseDeltaX += mouseDeltaX * _speedRotation;
        this.mouseDeltaY += mouseDeltaY * _speedRotation;
        this.mouseDeltaY = Mathf.Clamp(this.mouseDeltaY, -60, 60);
        this.gameObject.transform.rotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        verticalRotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        horizontalRotarion = Quaternion.AngleAxis(-this.mouseDeltaY, Vector3.right);
        _camera.transform.rotation = startRotation * verticalRotation * horizontalRotarion;

    }


    private void Move()
    {
        transform.Translate(new Vector3(horizontalSpeed, 0, verticalSpeed));
    }
}
