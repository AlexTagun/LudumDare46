using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectsObject : MonoBehaviour
{
    public static UIEffectsObject instance => FindObjectOfType<UIEffectsObject>();

    public EffectPrefabType spawnEffect<EffectPrefabType>(EffectPrefabType inPrefab)
        where EffectPrefabType : UIEffect
    {
        GameObject theNewEffect = Instantiate(inPrefab.gameObject);

        var theEffect = theNewEffect.GetComponent<UIEffect>();

        var theTranform = theNewEffect.GetComponent<RectTransform>();
        theTranform.SetParent(ownTransform, false);

        return theNewEffect.GetComponent<EffectPrefabType>();
    }

    private RectTransform ownTransform => _ownTransform;

    //Fields

    [SerializeField] private RectTransform _ownTransform = null;
}
