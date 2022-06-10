using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{

    Vector3 attachTransformDefaultPosition;
    Quaternion attachTransformDefaultRotation;

    protected override void Awake()
    {
        base.Awake();

        if (!attachTransform)
        {
            // Create a new attach point
            attachTransform = new GameObject("Attach Pivot").transform;
            attachTransform.SetParent(transform, false);
        }

        attachTransformDefaultPosition = attachTransform.position;
        attachTransformDefaultRotation = attachTransform.rotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
       
        if(args.interactorObject is XRDirectInteractor)
        {
            Debug.Log("IsGrabInteractable");
            attachTransform.position = args.interactorObject.transform.position;
            attachTransform.rotation = args.interactorObject.transform.rotation;
        }

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if(args.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = attachTransformDefaultPosition;
            attachTransform.rotation = attachTransformDefaultRotation;
        }

        base.OnSelectExited(args);
    }
}
