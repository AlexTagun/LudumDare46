using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWhaleEvent : MonoBehaviour
{
    [SerializeField] private Transform _pointToFallWhale = null;
    [SerializeField] private float _timeFalling = 0f;
    [SerializeField] private float _speedRotation = 0f;
    [SerializeField] private int _secondsToExecute = 0;

    private void Start() {
        EventManager.OnSecondTick += OnSecondTick;
        gameObject.SetActive(false);
    }

    private void OnSecondTick(int second) {
        if (second == _secondsToExecute) {
            Execute();
            EventManager.OnSecondTick -= OnSecondTick;
        }
    }
    private void Execute()
    {
        gameObject.SetActive(true);
        StartCoroutine(FallingWhale());
    }

    private IEnumerator FallingWhale ()
    {
        transform.DOMove(_pointToFallWhale.position, _timeFalling);
        transform.DORotate(Vector3.up * _speedRotation, _timeFalling);
        yield return new WaitForSeconds(_timeFalling);
        EventManager.HandleOnEndGame(EventManager.EndGameType.Win);
    }

}
