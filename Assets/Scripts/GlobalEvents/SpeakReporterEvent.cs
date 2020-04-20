using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakReporterEvent : GlobalEvent
{
    [SerializeField] private UISubtitleText _uISubtitleText;
    [SerializeField] private int _stageNumber;
    protected override void Execute()
    {
        _uISubtitleText.ShowSubtitleText(_stageNumber);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
