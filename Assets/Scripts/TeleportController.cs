using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Zomp
{
    public class TeleportController : MonoBehaviour
    {
        [SerializeField]
        XRController leftTeleportXRController;

        [SerializeField]
        XRController rightTeleportXRController;

        [SerializeField]
        InputHelpers.Button activationButton;

        [SerializeField]
        float threshold = 0.1f;

        public bool TeleportDisabled
        {
            get { return teleportDisabled; }
            set { teleportDisabled = value; }
        }
        bool teleportDisabled = false;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (leftTeleportXRController)
                leftTeleportXRController.gameObject.SetActive(!teleportDisabled && CheckLeftIsActive());

            if (rightTeleportXRController)
                rightTeleportXRController.gameObject.SetActive(!teleportDisabled && CheckRightIsActive());
        }

        bool CheckLeftIsActive()
        {
            bool isPressed;
            InputHelpers.IsPressed(leftTeleportXRController.inputDevice, activationButton, out isPressed, threshold);
            return isPressed;
        }

        bool CheckRightIsActive()
        {
            bool isPressed;
            InputHelpers.IsPressed(rightTeleportXRController.inputDevice, activationButton, out isPressed, threshold);
            return isPressed;
        }
    }

}
