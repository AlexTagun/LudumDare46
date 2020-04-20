using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VolcanoEvent : GlobalEvent
{
    [SerializeField] private float _timeRiseFromUnderground;
    [SerializeField] private float _positionOnGround;
    [SerializeField] private float _preparationTimeToEruption;
    [SerializeField] private ParticleSystem _particleSystemEruptionVolcano;

    protected override void Execute()
    {
        StartCoroutine(VolcanoEruption());
    }


    private void Awake()
    {
       _particleSystemEruptionVolcano.gameObject.SetActive(false);
    }


    private IEnumerator VolcanoEruption()
    {
        transform.DOMoveY(_positionOnGround, _timeRiseFromUnderground);
        yield return new WaitForSeconds(_preparationTimeToEruption + _timeRiseFromUnderground);
        _particleSystemEruptionVolcano.gameObject.SetActive(true);
        _particleSystemEruptionVolcano.Play();
    }
}
