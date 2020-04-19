using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramanInGameController : MonoBehaviour {
    [SerializeField] private CameramanMovement _cameramanMovement;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameObject _cameraEffectUI;
    [SerializeField] private GameReplay.ReplayRecorder _replayRecorder;

    //TEMPORARY PUBLIC FOR TESTS. CHANGE PUBLIC TO PRIVATE IF YOU SEE IT AND REMOVE TEST
    public List<GameReplay.Replay> _replays = new List<GameReplay.Replay>();

    private void Awake()
    {
        _cameraEffectUI.SetActive(false);
        EventManager.OnCameraShake += _cameramanMovement.ShakeCamera;
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 velocity = transform.right * x + transform.forward * z;
        //velocity.y = -9.8f * Time.deltaTime;


        // движение камеры и поворот player
        _cameramanMovement.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

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
        velocity.y -= 9.8f * Time.deltaTime;
        _cameramanMovement.Move(velocity);

        if (Input.GetMouseButtonDown(1) && isPossibleToChangeCameraState) {
            if (!_replayRecorder.isRecording) {
                _replayRecorder.startRecording();
                _cameraEffectUI.SetActive(true);
            }  else {
                isPossibleToChangeCameraState = false;
                StartCoroutine(_replayRecorder.stopRecording((GameReplay.Replay inReplay) => {
                    _replays.Add(inReplay);

                    _cameraEffectUI.SetActive(false);
                    isPossibleToChangeCameraState = true;
                }));
            }
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
