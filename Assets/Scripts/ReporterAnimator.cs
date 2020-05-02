using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReporterAnimator : MonoBehaviour {
    [SerializeField] private Animator _animator = null;

    private Transform _player = null;
    private bool _isNeedToLookAtPlayer = false;

    private void Awake() {
        EventManager.OnReporterAnim += StartAnimation;
        // StartAnimation(EventManager.ReporterAnim.Run);
    }

    private void Start() {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void StartAnimation(EventManager.ReporterAnim state) {
        switch (state) {
            case EventManager.ReporterAnim.Run:
                Run();
                _isNeedToLookAtPlayer = false;
                break;
            case EventManager.ReporterAnim.Talk:
                Talk();
                _isNeedToLookAtPlayer = true;
                break;
            case EventManager.ReporterAnim.Jump:
                Jump();
                _isNeedToLookAtPlayer = false;
                break;
        }
    }

    private void Update() {
        if(!_isNeedToLookAtPlayer) return;
        transform.LookAt(_player);
    }

    public void Run() { _animator.Play("Reporter_run_cure"); }
    public void Talk() { _animator.Play("Reporter_talking_cure"); }
    public void Jump() { _animator.Play("Reporter_talking_cure"); }
}
