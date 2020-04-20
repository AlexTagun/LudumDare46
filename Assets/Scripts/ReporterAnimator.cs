using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReporterAnimator : MonoBehaviour {
    [SerializeField] private Animator _animator;
    
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
