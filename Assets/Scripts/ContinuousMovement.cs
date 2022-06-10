using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;


namespace Zomp
{
    public class ContinuousMovement : MonoBehaviour
    {
        [SerializeField]
        XRNode inputSource;

        [SerializeField]
        float maxSpeed = 5;

        [SerializeField]
        LayerMask groundMask;

        Vector2 inputAxis;
        CharacterController characterController;
        XROrigin xrOrigin;
        float gravity = -9.8f;
        float ySpeed = 0;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            xrOrigin = GetComponent<XROrigin>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CapsuleFollowHeadset();

            InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

            // Move
            Quaternion rot = Quaternion.Euler(0, xrOrigin.Camera.transform.eulerAngles.y, 0);
            Vector3 direction = rot * new Vector3(inputAxis.x, 0, inputAxis.y);
            characterController.Move(direction * Time.deltaTime * maxSpeed);

            

            // Gravity
            bool isGrounded = CheckIsGrounded();
            ySpeed = !isGrounded ? ySpeed + gravity * Time.deltaTime : 0;
            characterController.Move(Vector3.up * ySpeed * Time.deltaTime);
        }

        bool CheckIsGrounded()
        {
            Vector3 origin = transform.TransformPoint(characterController.center);
            float distance = characterController.center.y + 0.01f;
            RaycastHit hitInfo;
            bool hit = Physics.SphereCast(origin, characterController.radius, Vector3.down, out hitInfo, distance, groundMask);
            return hit;
        }

        void CapsuleFollowHeadset()
        {
            characterController.height = xrOrigin.CameraInOriginSpaceHeight + characterController.skinWidth;
            Vector3 capsuleCenter = transform.InverseTransformPoint(xrOrigin.Camera.transform.position);
            characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2f, capsuleCenter.z);
        }
    }

}
