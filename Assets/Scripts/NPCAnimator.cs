using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : MonoBehaviour {
    [SerializeField] private Animator _animator = null;

    private void Awake() {
        Run();
    }

    public void Run() {
        _animator.Play("NPC_run_scared");
    }

    public void Die(Action callback) {
        StartCoroutine(DieAnimation(callback));
    }

    private IEnumerator DieAnimation(Action callback) {
        _animator.Play("NPC_Death");
        yield return new WaitForSeconds(1f);
        callback?.Invoke();
    }
}
