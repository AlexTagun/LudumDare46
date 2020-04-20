using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallUFO : MonoBehaviour
{
    [SerializeField] private int _secondsToExecute;
    [SerializeField] private Transform _pointToFallUFO;
    [SerializeField] private float _timeFalling;
    [SerializeField] private float _speedRotation;

    [Header("MoveToCircle")]
    [SerializeField] private Transform _cercentreMoveingCircle;
    [SerializeField] private float _timeCircularMotion;
    [SerializeField] private float _speed;





     private bool isCircle;
     private float radius;
     private float _angleRotation = 0f;

    private void Start()
     {
        EventManager.OnSecondTick += OnSecondTick;
        radius = Vector3.Distance(transform.position, _cercentreMoveingCircle.position);
        Debug.Log(radius);
     }

    private void Update()
    {
        MoveToCircle();
    }
    private void OnSecondTick(int second)
    {
        // Debug.Log(second + " : " + gameObject.name);
        if (second == _secondsToExecute)
        {
            StartCoroutine(Execute());
            EventManager.OnSecondTick -= OnSecondTick;
        }
    }

    private IEnumerator Execute()
    {
        isCircle = true;
        yield return new WaitForSeconds(_timeCircularMotion);
        isCircle = false;
        transform.DOMove(_pointToFallUFO.position, _timeFalling);
        transform.DORotate(Vector3.up * _speedRotation, _timeFalling);
    }

    private void MoveToCircle ()
    {
        if (isCircle)
        {
            _angleRotation += Time.deltaTime; // меняется плавно значение угла

            var x = Mathf.Cos(_angleRotation * _speed) * radius;
            var z = Mathf.Sin(_angleRotation * _speed) * radius;
            transform.position = new Vector3(x, transform.position.y, z);
        }
    }
}
