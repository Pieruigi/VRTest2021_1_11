using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Zomp
{
    public class Climber : MonoBehaviour
    {
        ContinuousMovement continuousMovement;

        public static XRController ClimbingHand
        {
            get { return climbingHand; }
            set { climbingHand = value; }
        }
        static XRController climbingHand;
        CharacterController characterController;

        private void Awake()
        {
            continuousMovement = GetComponent<ContinuousMovement>();
            characterController = GetComponent<CharacterController>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (climbingHand)
            {
                continuousMovement.enabled = false;
                Climb();
            }
            else
            {
                continuousMovement.enabled = true;
            }
        }

        void Climb()
        {
            InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
            characterController.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
        }

        
    }

}
