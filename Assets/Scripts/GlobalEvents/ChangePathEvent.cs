using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePathEvent : GlobalEvent
{
    [SerializeField] private ReporterMovement _reporterMovement;
    protected override void Execute()
    {
        //Debug.Log("Смена пути репортера");
        //gameObject.SetActive(true);
        _reporterMovement.CurrentPath = gameObject.GetComponent<LineRenderer>();
        _reporterMovement._canRunning = true;
        //anim Run
        EventManager.HandleOnReporterAnim(EventManager.ReporterAnim.Run);
    }

}
