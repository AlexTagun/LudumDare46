using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISubtitleText  : MonoBehaviour
{
    [SerializeField] private float _secondBetweenWindow;
    [SerializeField] private GameObject _subtitleUIPanel;
    [SerializeField] private TextMeshProUGUI _textPlaceUI;
    [SerializeField] private string[] _textForFirstReport;
    [SerializeField] private string[] _textForSecondReport;
    [SerializeField] private string[] _textForThirdReport;
    [SerializeField] private string[] _textForFourthReport;
    [SerializeField] private string[] _textForFifthReport;
    // Start is called before the first frame update

    private void Awake()
    {
        _subtitleUIPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowSubtitlelUIPanel()
    {
        _subtitleUIPanel.SetActive(true);
    }
    public void CloseSubtitleUIPanel()
    {
        _subtitleUIPanel.SetActive(false);
    }
    public void ChangeText(string text)
    {
        _textPlaceUI.text = text;
    }

    public IEnumerator ShowSubtitleText(int stage)
    {
        switch (stage)
        {
            case 1:
                ShowSubtitlelUIPanel();
                for (int i = 0; i < _textForFirstReport.Length; i++)
                {
                    ChangeText(_textForFirstReport[i]);
                    yield return new WaitForSeconds(_secondBetweenWindow);
                }
                CloseSubtitleUIPanel();
                break;
            case 2:
                ShowSubtitlelUIPanel();
                for (int i = 0; i < _textForSecondReport.Length; i++)
                {
                    ChangeText(_textForSecondReport[i]);
                    yield return new WaitForSeconds(_secondBetweenWindow);
                }
                CloseSubtitleUIPanel();
                break;
            case 3:
                ShowSubtitlelUIPanel();
                for (int i = 0; i < _textForThirdReport.Length; i++)
                {
                    ChangeText(_textForThirdReport[i]);
                    yield return new WaitForSeconds(_secondBetweenWindow);
                }
                CloseSubtitleUIPanel();
                break;
            case 4:
                ShowSubtitlelUIPanel();
                for (int i = 0; i < _textForFourthReport.Length; i++)
                {
                    ChangeText(_textForFourthReport[i]);
                    yield return new WaitForSeconds(_secondBetweenWindow);
                }
                CloseSubtitleUIPanel();
                break;
            case 5:
                ShowSubtitlelUIPanel();
                for (int i = 0; i < _textForFifthReport.Length; i++)
                {
                    ChangeText(_textForFifthReport[i]);
                    yield return new WaitForSeconds(_secondBetweenWindow);
                }
                CloseSubtitleUIPanel();
                break;

        }
        
        yield return new WaitForSeconds(_secondBetweenWindow);
    }
}
