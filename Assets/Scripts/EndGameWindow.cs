using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameWindow : MonoBehaviour {
    [SerializeField] private GameObject _container;
    [SerializeField] private CameramanMovement _cameramanMovement;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameReplay.ReplayPlayerObject _playerReplay = null;
    [SerializeField] private Image _back;
    [SerializeField] private CanvasGroup _diedImage;
    [SerializeField] private GameObject[] _objectsToOff;
    [SerializeField] private Sprite _simpleBack;
    [SerializeField] private Sprite _whaleBack;
    [SerializeField] private Text _finalPointsText;

    private void Awake() {
        EventManager.OnEndGame += Show;
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
        _restartButton.gameObject.SetActive(false);
        _container.SetActive(false);
        _finalPointsText.enabled = false;
    }

    private void Show(EventManager.EndGameType endGameType) {
        EventManager.gameState = EventManager.GameState.End;
        foreach (var obj in _objectsToOff) {
            obj.SetActive(false);
        }

        switch (endGameType) {
            case EventManager.EndGameType.Win:
                _back.sprite = _whaleBack;
                break;
            case EventManager.EndGameType.Die:
                _back.sprite = _simpleBack;
                break;
            case EventManager.EndGameType.LowBattery:
                _back.sprite = _simpleBack;
                break;
        }
        StartCoroutine(EndGameAnimation());

        _finalPointsText.enabled = true;

        _finalPointsText.text =
                FindObjectOfType<PopularityManager>().popularityAmount.ToString();
    }

    private IEnumerator EndGameAnimation() {
        yield return _cameramanMovement.MoveCameraToEndGamePos();
        // _diedImage.DOFade(1, 1.5f);
        yield return new WaitForSeconds(1.5f);
         // _diedImage.DOFade(0, 0.5f);
         yield return new WaitForSeconds(0.5f);
        _container.SetActive(true);
    }

    private void OnRestartButtonClicked() {
        SceneManager.LoadScene("StartMenu");
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

        StartCoroutine(_playerReplay.playReplays(theReplays,
            theAfterStartNextReplayPreporation,
            theBeforeStartNextReplay,
            () => {
                _playButton.gameObject.SetActive(true);
                _playerReplay.gameObject.SetActive(true);
                _restartButton.gameObject.SetActive(true);
            }));
    }

    private void OnCloseButtonClicked() {
        Application.Quit();
    }
}
