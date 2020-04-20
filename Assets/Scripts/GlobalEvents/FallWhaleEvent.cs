using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWhaleEvent : GlobalEvent
{
    [SerializeField] private Transform _pointToFallWhale;
    [SerializeField] private float _timeFalling;
    [SerializeField] private float _speedRotation;

    private void Start()
    {
    }
    protected override void Execute()
    {
        FallingWhale();
    }

    private void FallingWhale ()
    {
        transform.DOMove(_pointToFallWhale.position, _timeFalling);
        transform.DORotate(Vector3.up * _speedRotation, _timeFalling);
    }

}
