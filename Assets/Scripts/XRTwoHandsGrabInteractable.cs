using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Zomp
{
    public class XRTwoHandsGrabInteractable : XRGrabInteractable
    {
        [SerializeField]
        List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();

        XRBaseInteractor secondInteractor;
        Quaternion attachTransformRotationOld;
        float smoothTime = 5;

        // Start is called before the first frame update
        void Start()
        {
            foreach(XRSimpleInteractable item in secondHandGrabPoints)
            {
                item.selectEntered.AddListener(OnSecondHandGrab);
                item.selectExited.AddListener(OnSecondHandRelease);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (interactorsSelecting.Count > 0 && secondInteractor)
            {
                Vector3 direction = secondInteractor.transform.position - attachTransform.position;
                direction = attachTransform.InverseTransformVector(direction);
                
                //direction.x *= -1;
                //direction.z *= -1;
                //Vector3 angles = Quaternion.
                //Vector3.SignedAngle

                attachTransform.localRotation = Quaternion.Lerp(attachTransform.localRotation, Quaternion.Inverse(Quaternion.LookRotation(direction)), Time.deltaTime * smoothTime);
               
                //attachTransform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
            //attachTransform.localRotation = Quaternion.LookRotation(Vector3.forward);

            base.ProcessInteractable(updatePhase);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            Debug.Log("OnSelectEntered");
            attachTransformRotationOld = attachTransform.localRotation;
            base.OnSelectEntered(args);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            Debug.Log("OnSelectedExited");
            attachTransform.localRotation = attachTransformRotationOld;
            base.OnSelectExited(args);
        }

        void OnSecondHandGrab(SelectEnterEventArgs args)
        {
            Debug.Log("OnSecondHandGrab");
            secondInteractor = args.interactorObject.transform.GetComponent<XRBaseInteractor>();
        }

        void OnSecondHandRelease(SelectExitEventArgs args)
        {
            Debug.Log("OnSecondHandRelease");
            secondInteractor = null;
        }

        public override bool IsSelectableBy(IXRSelectInteractor interactor)
        {
            bool selectable = !this.isSelected || interactorsSelecting[0] == interactor;
            return base.IsSelectableBy(interactor) && selectable;
        }
    }

}
