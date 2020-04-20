using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakReporterEvent : MonoBehaviour {
    [SerializeField] private int _secondsToExecute;
    [SerializeField] private UISubtitleText _uISubtitleText;
    [SerializeField] private int _stageNumber;
    
    private void Start() {
        EventManager.OnSecondTick += OnSecondTick;
    }

    private void OnSecondTick(int second) {
        // Debug.Log(second + " : " + gameObject.name);
        if (second == _secondsToExecute) {
            Execute();
            EventManager.OnSecondTick -= OnSecondTick;
        }
    }

    private void Execute() {
        // anim Report
        StartCoroutine(_uISubtitleText.ShowSubtitleText(_stageNumber));
        EventManager.HandleOnReporterAnim(EventManager.ReporterAnim.Talk);
    }
}
