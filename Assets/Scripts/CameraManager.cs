using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera[] _cameras;

    // To ensure proper camera switching, material array should contain materials in this order:
    // Piece Placement = 0, Player Turn = 1, Opponent Turn = 2
    public void SwitchCameras(int cameraIndex)
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            if (i == cameraIndex)
            {
                _cameras[i].Priority = 11;
            }
            else
            {
                _cameras[i].Priority = 9;
            }
        }
    }
}
