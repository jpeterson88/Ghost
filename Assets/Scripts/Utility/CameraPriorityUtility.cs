using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class CameraPriorityUtility : MonoBehaviour
    {
        public CinemachineCamera mainCam, secondaryCam;
        private int mainDefaultCamPriority, secondaryDefaultCamPriority;
        private const int highPriority = 100, lowPriority = 1;

        private int currentlyCamId;
        private void Start()
        {
            mainDefaultCamPriority = mainCam.Priority.Value;
            secondaryDefaultCamPriority = secondaryCam.Priority.Value;

            
        }

        public void SwitchCameras(CinemachineCamera switchToCamera)
        {
            int switchToCamId = switchToCamera.GetInstanceID();
            if (currentlyCamId != switchToCamId)
            {
                if (switchToCamId == mainCam.GetInstanceID())
                {
                    mainCam.Priority.Value = highPriority;
                    secondaryCam.Priority.Value = lowPriority;

                    currentlyCamId = mainCam.GetInstanceID();
                }
                else if (switchToCamId == secondaryCam.GetInstanceID())
                {
                    secondaryCam.Priority.Value = highPriority;
                    mainCam.Priority.Value = lowPriority;

                    currentlyCamId = secondaryCam.GetInstanceID();
                }
            }
        }
    }
}