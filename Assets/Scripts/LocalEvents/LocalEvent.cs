﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocalEvent : MonoBehaviour
{
    [SerializeField] private int _secondsToExecute;

    private void Awake()
    {

    }

    protected abstract void Execute();

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ExecuteCoroutine());
    }

    private IEnumerator ExecuteCoroutine()
    {
        yield return new WaitForSeconds(_secondsToExecute);
        Execute();
    }
}
