using UnityEngine;
using UnityEngine.UI;

public class CameraEnergyFootageUIObject : MonoBehaviour
{
    private void FixedUpdate() {
        updateEnergyText();
        updateEnergyImage();
        updatePopularityText();
        updateDotBlinking();
    }

    private void updateEnergyText() {
        float theEnergyRatio = _energyManager.energyRatio;
        int theEnergyPercentAlignedToFive = Mathf.FloorToInt(theEnergyRatio * 100) / 5 * 5;
        _energyRatioTest.text = theEnergyPercentAlignedToFive.ToString() + "%";
    }

    private void updateEnergyImage() {
        _energyHighLevelImage.enabled = false;
        _energyMediumLevelImage.enabled = false;
        _energyLowLevelImage.enabled = false;

        int theEnergyPercent = Mathf.FloorToInt(_energyManager.energyRatio * 100);
        int theEnergyPart = 100 / 3;
        int theImageIndex = theEnergyPercent / theEnergyPart;
        int theImageIndexClamped = Mathf.Clamp(theImageIndex, 0, 2);

        switch(theImageIndexClamped) {
            case 0:  _energyLowLevelImage.enabled = true;      break;
            case 1:  _energyMediumLevelImage.enabled = true;   break;
            case 2:  _energyHighLevelImage.enabled = true;     break;
        }
    }

    private void updatePopularityText() {
        _popularityText.text = _popularityManager.popularityAmount.ToString();
    }

    private void updateDotBlinking() {
        _dotImage.enabled = (0 == (int)Time.fixedTime % 2);
    }

    //Fields
    [SerializeField] private CameraEnergyManager _energyManager = null;
    [SerializeField] private Text _energyRatioTest = null;
    [SerializeField] private Image _energyHighLevelImage = null;
    [SerializeField] private Image _energyMediumLevelImage = null;
    [SerializeField] private Image _energyLowLevelImage = null;

    [SerializeField] private PopularityManager _popularityManager = null;
    [SerializeField] private Text _popularityText = null;

    [SerializeField] private Image _dotImage = null;
}
