﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakReporterEvent : GlobalEvent
{
    [SerializeField] private UISubtitleText _uISubtitleText;
    [SerializeField] private int _stageNumber;
    protected override void Execute()
    {
        // anim Report
        StartCoroutine(_uISubtitleText.ShowSubtitleText(_stageNumber));
        EventManager.HandleOnReporterAnim(EventManager.ReporterAnim.Talk);
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
