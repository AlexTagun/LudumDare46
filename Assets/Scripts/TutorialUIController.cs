using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialUIPanel;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshPro _textPlaceUI;

    private void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowTutorialUIPanel()
    {
        _tutorialUIPanel.SetActive(true);
    }
    public void CloseTutorialUIPanel ()
    {
        _tutorialUIPanel.SetActive(false);
    }
    public void ChangeIcon (Sprite icon)
    {
        _iconImage.sprite = icon;
    }
    public void ChangeText (Text text)
    {
        _textPlaceUI.text = text.text;
    }

}
