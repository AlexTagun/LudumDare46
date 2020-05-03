using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameWindow : MonoBehaviour {
    [SerializeField] private GameObject _container = null;
    [SerializeField] private CameramanMovement _cameramanMovement = null;
    [SerializeField] private Button _playButton = null;
    [SerializeField] private Button _restartButton = null;
    [SerializeField] private Button _closeButton = null;
    [SerializeField] private GameReplay.ReplayPlayerObject _playerReplay = null;
    [SerializeField] private Image _back = null;
    [SerializeField] private CanvasGroup _canvasGroup = null;
    [SerializeField] private GameObject[] _objectsToOff = null;
    [SerializeField] private Sprite _simpleBack = null;
    [SerializeField] private Sprite _whaleBack = null;
    [SerializeField] private Text _finalPointsText = null;

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
        _container.SetActive(true);
        _canvasGroup.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
    }

    private void OnRestartButtonClicked() {
        SceneManager.LoadScene("StartMenu");
    }

    private void OnPlayButtonClicked() {
        _playButton.gameObject.SetActive(false);
        List<GameReplay.Replay> theReplays = FindObjectOfType<CameramanInGameController>()._replays;

        StartCoroutine(_playerReplay.playReplays(theReplays, () => {
            _playButton.gameObject.SetActive(true);
            _playerReplay.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);
        }));
    }

    private void OnCloseButtonClicked() {
        Application.Quit();
    }
}
