using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLaunchEvent : GlobalEvent {

    [SerializeField] private float _duraction;
    [SerializeField] private float _endPositionY;
    [SerializeField] private float _speedRotation;

    protected override void Execute() {
        Debug.Log("Rocket Launched");
        transform.DORotate(Vector3.up * _speedRotation, _duraction);
        transform.DOMoveY(_endPositionY, _duraction).OnComplete(() => { Destroy(gameObject); });
    }
}
