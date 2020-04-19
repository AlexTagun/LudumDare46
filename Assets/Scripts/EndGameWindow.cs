using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameWindow : MonoBehaviour {
    [SerializeField] private GameObject _container;
    [SerializeField] private CameramanMovement _cameramanMovement;
    [SerializeField] private Button _playButton;
    [SerializeField] private GameReplay.ReplayPlayerObject _playerReplay = null;
    [SerializeField] private Image _back;

    private void Awake() {
        EventManager.OnEndGame += Show;
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _container.SetActive(false);
    }

    private void Show(EventManager.EndGameType endGameType) {
        StartCoroutine(EndGameAnimation());
        
    }

    private IEnumerator EndGameAnimation() {
        yield return _cameramanMovement.MoveCameraToEndGamePos();
        _container.SetActive(true);
    }

    private void OnPlayButtonClicked() {
        _playButton.gameObject.SetActive(false);
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

        StartCoroutine(_playerReplay.playReplays(theReplays,
            theAfterStartNextReplayPreporation,
            theBeforeStartNextReplay,
            theLastReplayFinished));
    }
}
