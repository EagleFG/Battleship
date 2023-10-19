using UnityEngine;

public class RotateUnplacedPieces : MonoBehaviour
{
    [SerializeField]
    private Piece[] _pieces;

    private bool _isHorizontal = true;

    // called by button
    public void SwitchPieceRotation()
    {
        if (_isHorizontal == true)
        {
            gameObject.transform.eulerAngles = new Vector3(0, -90, 0);

            _isHorizontal = false;
        }
        else
        {
            gameObject.transform.eulerAngles = Vector3.zero;

            _isHorizontal = true;
        }

        for (int i = 0; i < _pieces.Length; i++)
        {
            if (_pieces[i].IsPiecePlaced() == false)
            {
                _pieces[i].ResetPosition();
            }
        }
    }
}
