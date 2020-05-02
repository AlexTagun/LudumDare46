using UnityEngine;

public class SpeakReporterEvent : MonoBehaviour {
    [SerializeField] private int _secondsToExecute = 0;
    [SerializeField] private UISubtitleText _uISubtitleText = null;
    [SerializeField] private int _stageNumber = 0;
    
    private void Start() {
        EventManager.OnSecondTick += OnSecondTick;
    }

    private void OnSecondTick(int second) {
        if (second == _secondsToExecute) {
            Execute();
            EventManager.OnSecondTick -= OnSecondTick;
        }
    }

    private void Execute() {
        StartCoroutine(_uISubtitleText.ShowSubtitleText(_stageNumber));
        EventManager.HandleOnReporterAnim(EventManager.ReporterAnim.Talk);
    }
}
