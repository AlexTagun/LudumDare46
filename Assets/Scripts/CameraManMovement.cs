using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float verticalSpeed;
    private float horizontalSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveForward ()
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
    public void MoveLeft ()
    {
        horizontalSpeed = _speed * Time.deltaTime;
        verticalSpeed = 0f;
        Move();
    }

    public void MoveRight ()
    {
        horizontalSpeed = (-1) * _speed * Time.deltaTime;
        verticalSpeed = 0f;
        Move();
    }

    private void Move ()
    {
        transform.Translate(new Vector3(verticalSpeed, 0, horizontalSpeed));
    }
}
