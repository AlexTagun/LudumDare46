using UnityEngine;
using System.Collections.Generic;

public class ReplayRecroderTest : MonoBehaviour
{
    void Update() {
        //if (Input.GetKey(KeyCode.A) && !_isRecording) {
        //    _recorder.startRecording();
        //    _isRecording = true;
        //}

        if (Input.GetKey(KeyCode.Q) && !_isPlayed) {
            List<GameReplay.Replay> theReplays = FindObjectOfType<CameramanInGameController>()._replays;

            System.Action theLastReplayFinished = () => {
                Debug.Log("LastReplayFinished");
            };

            StartCoroutine(_player.playReplays(theReplays, theLastReplayFinished));

            _isPlayed = true;
        }
    }

    bool _isPlayed = false;

    [SerializeField] private GameReplay.ReplayPlayerObject _player = null;
}
