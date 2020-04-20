﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramanInGameController : MonoBehaviour {
    [SerializeField] private CameramanMovement _cameramanMovement;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameReplay.ReplayRecorder _replayRecorder;
    [SerializeField] private PopularityCollector _popularityCollector;
    [SerializeField] private CameraEnergyManager _cameraEnergyManager;

    public bool CanMove = false;
    public bool CanShoot = false;

    //TEMPORARY PUBLIC FOR TESTS. CHANGE PUBLIC TO PRIVATE IF YOU SEE IT AND REMOVE TEST
    public List<GameReplay.Replay> _replays = new List<GameReplay.Replay>();

    private void Awake()
    {
        // _cameraEffectUI.SetActive(false);
        EventManager.OnCameraShake += _cameramanMovement.ShakeCamera;
    }

    private void Update()
    {
        if(EventManager.gameState != EventManager.GameState.Gameplay) return;
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 velocity = transform.right * x + transform.forward * z;
        velocity.y -= 9.8f * Time.deltaTime;

        if (_characterController.isGrounded)
        {
                // velocity.y = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // velocity.y = _cameramanMovement.JumpSpeed * Time.deltaTime;
                // velocity.y += Mathf.Sqrt(_cameramanMovement.JumpSpeed * -2f * -9.8f);
                StartCoroutine(Jump());
            };
        }
        //velocity.y -= 9.8f * Time.deltaTime;
        _cameramanMovement.Move(velocity);

        // движение камеры и поворот player
        _cameramanMovement.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButtonDown(1) && CanShoot && isPossibleToChangeCameraState) {
            if (!_replayRecorder.isRecording) {
                
                StartCoroutine(_cameramanMovement.LookAtCamera(() => {
                    _popularityCollector.setCollectingEnabled(true);
                    _replayRecorder.startRecording();
                    _cameraEnergyManager.startSpendingEnergy();
                }));

            }  else {
                isPossibleToChangeCameraState = false;
                StartCoroutine(_cameramanMovement.DontLookAtCamera(() => {
                    StartCoroutine(_replayRecorder.stopRecording((GameReplay.Replay inReplay) => {
                        _replays.Add(inReplay);
                        isPossibleToChangeCameraState = true;
                        _popularityCollector.setCollectingEnabled(false);
                        _cameraEnergyManager.stopSpendingEnergy();
                    }));
                }));
            }
        }
        
        //TODO: DELETE

        if (Input.GetKeyDown(KeyCode.F)) {
            EventManager.HandleOnEndGame(EventManager.EndGameType.Die);
        }
    }
    private bool isPossibleToChangeCameraState = true;

    private IEnumerator Jump() {
        var k = 0.01f;
        for (int i = 0; i < _cameramanMovement.JumpFrameTime; i++) {
            Vector3 velocity = Vector3.zero;
            velocity.y += Mathf.Sqrt((_cameramanMovement.JumpSpeed - k) * -2f * -9.8f);
            k += k;
            _cameramanMovement.Move(velocity);
            yield return null;
        }
    }

}
