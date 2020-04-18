using UnityEngine;

public class ReplayRecroderTest : MonoBehaviour
{
    void Update() {
        if (Input.GetKey(KeyCode.A) && !_isRecording) {
            _recorder.startRecording();
            _isRecording = true;
        }

        if (Input.GetKey(KeyCode.D) && _isRecording) {
            _player.playReplay(_recorder.stopRecording());
            _isRecording = false;
        }
    }

    bool _isRecording = false;

    [SerializeField] private GameReplay.ReplayRecorder _recorder = null;
    [SerializeField] private GameReplay.ReplayPlayerObject _player = null;
}
