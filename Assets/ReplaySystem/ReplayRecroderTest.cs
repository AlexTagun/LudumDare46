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

            System.Action<GameReplay.Replay> theAfterStartNextReplayPreporation = (GameReplay.Replay inReplay) =>{
                Debug.Log("AfterStartNextReplayPreporation");
            };
            System.Action<GameReplay.Replay> theBeforeStartNextReplay = (GameReplay.Replay inReplay) => {
                Debug.Log("BeforeStartNextReplay");
            };
            System.Action theLastReplayFinished = () => {
                Debug.Log("LastReplayFinished");
            };

            StartCoroutine(_player.playReplays(theReplays,
                    theAfterStartNextReplayPreporation,
                    theBeforeStartNextReplay,
                    theLastReplayFinished));

            _isPlayed = true;
        }
    }

    bool _isPlayed = false;

    [SerializeField] private GameReplay.ReplayRecorder _recorder = null;
    [SerializeField] private GameReplay.ReplayPlayerObject _player = null;
}
