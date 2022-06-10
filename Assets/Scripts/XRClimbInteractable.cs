using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Zomp
{
    public class XRClimbInteractable : XRBaseInteractable
    {
        
    
        private void Start()
        {
        
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            if(args.interactorObject is XRDirectInteractor)
            {
                Climber.ClimbingHand = args.interactorObject.transform.GetComponent<XRController>();
            }
            Debug.Log("OnEntered");
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            if (args.interactorObject is XRDirectInteractor)
            {
                if (Climber.ClimbingHand && Climber.ClimbingHand == args.interactorObject.transform.GetComponent<XRController>())
                    Climber.ClimbingHand = null;
            }
            Debug.Log("OnExited");

        }
    }

}
