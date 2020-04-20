using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReporterAnimator : MonoBehaviour {
    [SerializeField] private Animator _animator;

    private void Awake() {
        EventManager.OnReporterAnim += StartAnimation;
        // StartAnimation(EventManager.ReporterAnim.Run);
    }

    private void StartAnimation(EventManager.ReporterAnim state) {
        switch (state) {
            case EventManager.ReporterAnim.Run:
                Run();
                break;
            case EventManager.ReporterAnim.Talk:
                Talk();
                break;
            case EventManager.ReporterAnim.Jump:
                Jump();
                break;
        }
    }

    public void Run() {
        _animator.Play("Reporter_run_cure");
    }

    public void Talk() {
        _animator.Play("Reporter_talking_cure");
    }

    public void Jump() {
        _animator.Play("Reporter_talking_cure");
    }
}
