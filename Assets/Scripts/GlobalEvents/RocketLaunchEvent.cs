using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLaunchEvent : GlobalEvent {

    [SerializeField] private float _duraction;
    [SerializeField] private float _endPosition;

    protected override void Execute() {
        Debug.Log("Rocket Launched");
        transform.DOMoveY(_endPosition, _duraction).OnComplete(() => { Destroy(gameObject); });
    }
}
