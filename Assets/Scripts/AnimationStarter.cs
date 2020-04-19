using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationStarter : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationName;
    
    void Start() {
        _animator.Play(_animationName);
    }
}
