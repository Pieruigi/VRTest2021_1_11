using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Zomp
{
    public class XRInputDeviceManager : MonoBehaviour
    {
        [SerializeField]
        InputDeviceCharacteristics characteristics;

        InputDevice device;

        XRController xrController;
        GameObject handModel;
        Animator handAnimator;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            List<InputDevice> devices = new List<InputDevice>();
            
            InputDevices.GetDevicesWithCharacteristics(characteristics,  devices);

            if(devices.Count > 0)
            {
                device = devices[0];
                Debug.LogFormat("{0} {1}", device.name, device.characteristics);

                xrController = GetComponent<XRController>();
                //handModel = xrController.model.gameObject;
                //handAnimator = handModel.GetComponent<Animator>();
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHandAnimation();

            //if(device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
            //{
            //    Debug.LogFormat("{0} - {1}:{2}", device.name, CommonUsages.primaryButton.name, primaryButtonValue);
            //}

            //if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonvalue) && secondaryButtonvalue)
            //{
            //    Debug.LogFormat("{0} - {1}:{2}", device.name, CommonUsages.secondaryButton.name, secondaryButtonvalue);
            //}

            //if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
            //{
            //    Debug.LogFormat("{0} - {1}:{2}", device.name, CommonUsages.trigger.name, triggerValue);
            //}

            //if (device.TryGetFeatureValue(CommonUsages.grip, out float gripValue) && gripValue > 0.1f)
            //{
            //    Debug.LogFormat("{0} - {1}:{2}", device.name, CommonUsages.grip.name, gripValue);
            //}
        }

        void UpdateHandAnimation()
        {
            if (!handModel)
            {
                handModel = xrController.model.gameObject;
                handAnimator = handModel.GetComponent<Animator>();
            }

            if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                handAnimator.SetFloat("Trigger", triggerValue);
            }
            else
            {
                handAnimator.SetFloat("Trigger", 0);
            }
            if (device.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                handAnimator.SetFloat("Grip", gripValue);
            }
            else
            {
                handAnimator.SetFloat("Grip", 0);
            }
        }
    }

}
