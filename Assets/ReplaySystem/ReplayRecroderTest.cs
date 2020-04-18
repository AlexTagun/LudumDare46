using UnityEngine;

public class ReplayRecroderTest : MonoBehaviour
{
    void Update() {
        if (Input.GetKey(KeyCode.A) && !_isRecording) {
            _recorder.startRecording();
            _isRecording = true;
        }

        if (Input.GetKey(KeyCode.D) && _isRecording) {
            StartCoroutine(_recorder.stopRecording((GameReplay.Replay inReplay)=> {
                _player.playReplay(inReplay);
            }));
            _isRecording = false;
        }
    }

    bool _isRecording = false;

    [SerializeField] private GameReplay.ReplayRecorder _recorder = null;
    [SerializeField] private GameReplay.ReplayPlayerObject _player = null;
}
