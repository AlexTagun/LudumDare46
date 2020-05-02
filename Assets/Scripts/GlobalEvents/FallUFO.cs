using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallUFO : MonoBehaviour
{
    [SerializeField] private int _secondsToExecute = 0;
    [SerializeField] private Transform _startPositionOnSky = null;
    [SerializeField] private Transform _pointToFallUFO = null;
    [SerializeField] private float _timeFalling = 0f;
    [SerializeField] private float _speedRotation = 0f;

    [Header("MoveToCircle")]
    [SerializeField] private Transform _cercentreMoveingCircle = null;
    [SerializeField] private float _timeCircularMotion = 0f;
    [SerializeField] private float _speed = 0f;





     private bool isCircle = false;
     private float radius = 0f;
     private float _angleRotation = 0f;

    private void Start()
     {
        EventManager.OnSecondTick += OnSecondTick;
        radius = Vector3.Distance(transform.position, _cercentreMoveingCircle.position);
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
        transform.position = _startPositionOnSky.position;
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
