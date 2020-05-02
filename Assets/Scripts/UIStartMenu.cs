using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStartMenu : MonoBehaviour {
    [SerializeField] private Button _playButton = null;

    private void Awake() {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked() {
        EventManager.gameState = EventManager.GameState.Gameplay;
        SceneManager.LoadScene("SampleScene");
    }
}
