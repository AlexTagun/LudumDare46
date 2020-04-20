using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    private TimeHandler _timer;
    
    private void Awake() {
        EventManager.OnStartGlobalTimer += StartTimer;
    }

    private void Update() {
        
        _timer?.Update(Time.deltaTime);
    }

    private void StartTimer()
    {
        _timer = new TimeHandler(1f, EventManager.HandleOnSecondTick);
    }

    private class TimeHandler {
        private readonly Action<int> _handler;
        private readonly float _period;
        private float _delta;
        private int _circle;

        public TimeHandler(float period, Action<int> handler) {
            _period = period;
            _handler = handler;
            _circle = 0;
        }

        public void Update(float dt) {
            _delta += dt;
            if (_delta >= _period) {
                try {
                    _circle++;
                    _handler.Invoke(_circle);
                } catch (Exception e) {
                    Debug.LogException(e);                        
                }

                _delta = 0f;
            }
        }
    }
}
