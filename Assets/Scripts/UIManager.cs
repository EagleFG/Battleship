using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button _startGameButton;

    [SerializeField]
    private GameObject _piecePlacementUI;

    [SerializeField]
    private SunkPieceUI _sunkPieceUI;

    [SerializeField]
    private GameObject _victoryUI, _defeatUI;

    public void StartPiecePlacementPhaseUI()
    {
        _startGameButton.gameObject.SetActive(false);

        _piecePlacementUI.SetActive(true);
    }

    public void StartTurnTakingPhaseUI()
    {
        _piecePlacementUI.SetActive(false);
    }

    public void TriggerSunkPieceUI(string nameOfSunkPiece)
    {
        _sunkPieceUI.gameObject.SetActive(true);

        _sunkPieceUI.TriggerSunkPieceText(nameOfSunkPiece);
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
