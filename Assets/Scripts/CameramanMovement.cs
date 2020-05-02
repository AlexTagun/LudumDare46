using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameramanMovement : MonoBehaviour {
    [SerializeField] private Camera _camera = null;
    [SerializeField] private float _speed = 0f;
    [SerializeField] private float _speedRotation = 0f;
    [SerializeField] private float _jumpSpeed = 0f;
    [SerializeField] private int _jumpFrameTime = 0;
    [SerializeField] private CharacterController _characterController = null;
    [SerializeField] private Transform _cameraStartPos = null;
    [SerializeField] private Transform _cameraEndPos = null;
    [SerializeField] private Transform _cameraEndGamePos = null;
    [SerializeField] private Transform _cameraTransform = null;
    [SerializeField] private Image _uiBlack = null;
    [SerializeField] private GameObject _cameraEffectUI = null;

    [Header("Shake Camera")]
    [SerializeField] private float _duractionShake = 0f;



    public float JumpSpeed => _jumpSpeed;
    public int JumpFrameTime => _jumpFrameTime;
    // вращение камеры
    private float mouseDeltaX = 0f;
    private float mouseDeltaY = 0f;
    private Quaternion startRotation = Quaternion.identity;
    private Quaternion verticalRotation = Quaternion.identity;
    private Quaternion horizontalRotarion = Quaternion.identity;



    private void Awake() {
        startRotation = transform.rotation;
    }

    public void Rotate(float mouseDeltaX, float mouseDeltaY) {
        this.mouseDeltaX += mouseDeltaX * _speedRotation;
        this.mouseDeltaY += mouseDeltaY * _speedRotation;
        this.mouseDeltaY = Mathf.Clamp(this.mouseDeltaY, -60, 60);
        this.gameObject.transform.rotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        verticalRotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        horizontalRotarion = Quaternion.AngleAxis(-this.mouseDeltaY, Vector3.right);
        _camera.transform.rotation = startRotation * verticalRotation * horizontalRotarion;
    }

    public void Move(Vector3 vel) {
        vel.x *= _speed * Time.deltaTime;
        vel.z *= _speed * Time.deltaTime;
        _characterController.Move(vel);
    }

    public void ShakeCamera ()
    {
        _camera.transform.DOShakePosition(_duractionShake);
    }

    public IEnumerator LookAtCamera(Action callback = null) {
        _cameraTransform.DOLocalMove(_cameraEndPos.localPosition, 0.5f);
        _cameraTransform.DOScale(_cameraEndPos.localScale, 0.5f);
        _uiBlack.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        _cameraEffectUI.SetActive(true);
        _cameraTransform.gameObject.SetActive(false);
        _uiBlack.DOFade(0, 0.2f);
        callback?.Invoke();
    }
    
    public IEnumerator DontLookAtCamera(Action callback = null) {
        _uiBlack.DOFade(1, 0.2f);
        
        yield return new WaitForSeconds(0.1f);
        _cameraTransform.gameObject.SetActive(true);
        
        _cameraEffectUI.SetActive(false);
        _uiBlack.DOFade(0, 0.5f);
        _cameraTransform.DOLocalMove(_cameraStartPos.localPosition, 0.5f);
        _cameraTransform.DOScale(_cameraStartPos.localScale, 0.5f);
        yield return new WaitForSeconds(0.5f);
        callback?.Invoke();
    }

    public IEnumerator MoveCameraToEndGamePos() {
        _camera.transform.DOLocalMove(_cameraEndGamePos.localPosition, 4f);
        _camera.transform.DORotate(_cameraEndGamePos.localRotation.eulerAngles, 4f);
        
        yield return new WaitForSeconds(3f);
        _uiBlack.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
    }
    
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "car") {
            EventManager.HandleOnEndGame(EventManager.EndGameType.Die);
        }
    }
}
