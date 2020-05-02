using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationStarter : MonoBehaviour {
    [SerializeField] private Animator _animator = null;
    [SerializeField] private string _animationName = null;
    
    void Start() {
        _animator.Play(_animationName);
    }
}
