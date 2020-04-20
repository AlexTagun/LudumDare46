using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoEruptionEvent : GlobalEvent
{
    [SerializeField] private float _timeRiseFromUnderground;
    [SerializeField] private float _positionOnGround;
    [SerializeField] private float _preparationTimeToEruption;
    [SerializeField] private ParticleSystem _particleSystemEruptionVolcano;

    private void Awake()
    {
        _particleSystemEruptionVolcano.gameObject.SetActive(false);
    }
    protected override void Execute()
    {
        Debug.Log("Volcano");
        StartCoroutine(VolcanoEruption());  
    }

    private IEnumerator VolcanoEruption()
    {
        Debug.Log("12231");
        transform.DOMoveY(_positionOnGround, _timeRiseFromUnderground);
        yield return new WaitForSeconds(_preparationTimeToEruption + _timeRiseFromUnderground);
        _particleSystemEruptionVolcano.gameObject.SetActive(true);
        _particleSystemEruptionVolcano.Play();
    }
}
