using UnityEngine;

public class ChangePathEvent : GlobalEvent
{
    [SerializeField] private ReporterMovement _reporterMovement = null;
    protected override void Execute()
    {
        _reporterMovement.CurrentPath = gameObject.GetComponent<LineRenderer>();
        _reporterMovement._canRunning = true;
        EventManager.HandleOnReporterAnim(EventManager.ReporterAnim.Run);
    }
}
