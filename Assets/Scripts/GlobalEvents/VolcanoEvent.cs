using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VolcanoEvent : GlobalEvent
{
    [SerializeField] private float _timeRiseFromUnderground = 0f;
    [SerializeField] private float _positionOnGround = 0f;
    [SerializeField] private float _preparationTimeToEruption = 0f;
    [SerializeField] private ParticleSystem _particleSystemEruptionVolcano = null;
    [SerializeField] private Animator _animator = null;

    protected override void Execute()
    {
        StartCoroutine(VolcanoEruption());
    }


    private void Awake()
    {
       _particleSystemEruptionVolcano.gameObject.SetActive(false);
       _animator.enabled = false;
    }


    private IEnumerator VolcanoEruption()
    {
        transform.DOMoveY(_positionOnGround, _timeRiseFromUnderground);
        yield return new WaitForSeconds(_preparationTimeToEruption + _timeRiseFromUnderground);
        _particleSystemEruptionVolcano.gameObject.SetActive(true);
        _particleSystemEruptionVolcano.Play();
        _animator.enabled = true;
        _animator.Play("volcano");
    }
}
