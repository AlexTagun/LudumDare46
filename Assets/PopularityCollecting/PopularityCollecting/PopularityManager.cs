using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopularityManager : MonoBehaviour
{
    public void gainPopularity(int inPopularityToGain) {
        _popularityAmount += inPopularityToGain;
    }

    private void Awake() {
        _popularityAmount = _initialPopularityAmount;
    }

    //Fields
    [SerializeField] private int _initialPopularityAmount = 0;

    private int _popularityAmount = 0;
}
