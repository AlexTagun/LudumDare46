using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnergyManager : MonoBehaviour
{
    public void startSpendingEnergy() { _isUsingEnergy = true; }
    public void stopSpendingEnergy() { _isUsingEnergy = false; }
    public float energyAspectRatio => _energyAmount / _startingEnergyAmount;

    private void FixedUpdate() {
        if (_isUsingEnergy)
            updateSpendingEnergy();
    }

    private void updateSpendingEnergy() {
        _energyAmount -= Time.fixedDeltaTime;
    }

    private void Awake() {
        _energyAmount = _startingEnergyAmount;
    }

    //Fields

    [SerializeField] float _startingEnergyAmount = 100f;
    
    private float _energyAmount = 0f;
    private bool _isUsingEnergy = false;
}
