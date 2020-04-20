using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private float _secondBetweenWindow;
    [SerializeField] private CameramanInGameController _cameramanInGameController;
    [SerializeField] private GameObject _tutorialUIPanel;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _textPlaceUI;
    [SerializeField] private Sprite[] _iconsForTutorial;
    [SerializeField] private string[] _textsForTutorial;



    private void Start()
    {
        StartCoroutine(ShowTutorial());
    }

    void Update()
    {
        
    }
    public IEnumerator ShowTutorial ()
    {
        // window 1
        EventManager.lockMovements = true;
        ShowTutorialUIPanel();
        yield return UpdateTutorialUI(0, 1);
        // window 2
        ChangeIcon(_iconsForTutorial[1]);
        ChangeText(_textsForTutorial[1]);
        _cameramanInGameController.CanMove = true;
        EventManager.lockMovements = false;
        yield return WaitForClickOnW();
        // window 3
        yield return UpdateTutorialUI(0, 3);
        // window 4
        ChangeIcon(_iconsForTutorial[1]);
        ChangeText(_textsForTutorial[3]);
        _cameramanInGameController.CanShoot = true;
        yield return WaitForMouseClick();
        // window 5
        _cameramanInGameController.CanMove = false;
        _cameramanInGameController.CanShoot = false;
        yield return UpdateTutorialUI(0, 5);
        // window 6
        yield return UpdateTutorialUI(0, 6);
        // window 7
        EventManager.HandleOnStartGlobalTimer(); // запускается глобальное время
        ChangeIcon(_iconsForTutorial[1]);
        ChangeText(_textsForTutorial[6]);
        _cameramanInGameController.CanShoot = true;
        _cameramanInGameController.CanMove = true;
        yield return WaitForMouseClick();
        // window 8
        yield return UpdateTutorialUI(1, 8);
        CloseTutorialUIPanel();
        yield break;
    }

    private IEnumerator UpdateTutorialUI (int indexIcon, int indexStage)
    {
        ChangeIcon(_iconsForTutorial[indexIcon]);
        ChangeText(_textsForTutorial[indexStage-1]);
        yield return new WaitForSeconds(_secondBetweenWindow);
    }
    private IEnumerator WaitForMouseClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }private IEnumerator WaitForClickOnW()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                yield break;
            }
            else
            {
                yield return null;
            }
        }
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
    public void ChangeText (string text)
    {
        _textPlaceUI.text = text;
    }

}
