using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLaunchEvent : GlobalEvent {

    [SerializeField] private float _timeRiseFromUnderground = 0f;
    [SerializeField] private float _positionOnGround = 0f;
    [SerializeField] private float _preparationTime = 0f;
    [SerializeField] private float _duractionFly = 0f;
    [SerializeField] private float _endPositionY = 0f;
    [SerializeField] private float _speedRotation = 0f;

    protected override void Execute() {
        StartCoroutine(LaunchRocket());
    }

    private IEnumerator LaunchRocket ()
    {
        transform.DOMoveY(_positionOnGround, _timeRiseFromUnderground);
        yield return new WaitForSeconds(_preparationTime + _timeRiseFromUnderground);
        transform.DORotate(Vector3.up * _speedRotation, _duractionFly);
        transform.DOMoveY(_endPositionY, _duractionFly).OnComplete(() => { Destroy(gameObject); });
    }
}
