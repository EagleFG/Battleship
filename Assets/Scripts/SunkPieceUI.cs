using UnityEngine;
using TMPro;

public class SunkPieceUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _sunkPieceText;

    [SerializeField]
    private float _appearanceDuration = 4;

    private float _disableStartTime = 0;

    private void Update()
    {
        if (Time.time > _disableStartTime + _appearanceDuration)
        {
            gameObject.SetActive(false);
        }
    }

    public void TriggerSunkPieceText(string nameOfSunkPiece)
    {
        _disableStartTime = Time.time;

        _sunkPieceText.text = "You sank my " + nameOfSunkPiece + "!";
    }
}
