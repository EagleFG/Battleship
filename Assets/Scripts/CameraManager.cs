using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera[] _cameras;

    // To ensure proper camera switching, virtual camera array should contain items in this order:
    // Piece Placement Camera = 0, Turn Taking Camera = 1
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
