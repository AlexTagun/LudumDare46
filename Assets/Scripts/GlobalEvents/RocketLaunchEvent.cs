using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLaunchEvent : GlobalEvent {

    [SerializeField] private float _timeRiseFromUnderground;
    [SerializeField] private float _positionOnGround;
    [SerializeField] private float _preparationTime;
    [SerializeField] private float _duractionFly;
    [SerializeField] private float _endPositionY;
    [SerializeField] private float _speedRotation;

    protected override void Execute()
    {
        StartCoroutine(LaunchRocket());
    }

    private IEnumerator LaunchRocket ()
    {
        transform.DOMoveY(_positionOnGround, _timeRiseFromUnderground);
        yield return new WaitForSeconds(_preparationTime);
        transform.DORotate(Vector3.up * _speedRotation, _duractionFly);
        transform.DOMoveY(_endPositionY, _duractionFly).OnComplete(() => { Destroy(gameObject); });
    }
}
