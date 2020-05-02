using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlobalEvent : MonoBehaviour {
    [SerializeField] private int _secondsToExecute = 0;

    private void Start() {
        EventManager.OnSecondTick += OnSecondTick;
    }

    private void OnSecondTick(int second) {
        //Debug.Log(second);
        if (second == _secondsToExecute) {
            Execute();
            EventManager.OnSecondTick -= OnSecondTick;
        }
    }

    protected abstract void Execute();
}
