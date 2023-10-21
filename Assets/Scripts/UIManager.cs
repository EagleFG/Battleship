using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button _startGameButton;

    [SerializeField]
    private GameObject _piecePlacementUI, _victoryUI, _defeatUI;

    public void StartPiecePlacementPhaseUI()
    {
        _startGameButton.gameObject.SetActive(false);

        _piecePlacementUI.SetActive(true);
    }

    public void StartTurnTakingPhaseUI()
    {
        _piecePlacementUI.SetActive(false);
    }

    public void TriggerVictoryUI()
    {
        _victoryUI.SetActive(true);
    }

    public void TriggerDefeatUI()
    {
        _defeatUI.SetActive(true);
    }

    // called from button
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    // called from button
    public void QuitGame()
    {
        Application.Quit();
    }
}
