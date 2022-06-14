//#define TEST
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Zomp
{
    /// <summary>
    /// Bird flying physics implementation
    /// </summary>

    public class Bird : MonoBehaviour
    {
#if TEST
        public Vector3 _testVelocity;

#endif


        [SerializeField]
        Transform leftWingPivot;

        [SerializeField]
        Transform rightWingPivot;

        [SerializeField]
        Transform body;

        Vector3 currentVelocity;//, targetVelocity;
        float currentRollSpeed = 0, targetRollSpeed = 0;
        float rollSmoothTime = 10f;
        float maxRollSpeed = 30;
        //float rollRecoilSpeed = 10;

        float gravityAcceleration = 9.8f;

        // 0 means you fall down like a rock, 1 you don't fall at all
        float wingsResistanceFactor = 0.008f; 

        // 0 means the wing is closed, 1 is open
        // Actual surface is wingMaxSurface * stretchRatio
        float leftWingStretchRatio = 1;
        float rightWingStretchRatio = 1;

        Transform cameraOffset;
        CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        // Start is called before the first frame update
        void Start()
        {
#if TEST
            currentVelocity = _testVelocity;
#endif

            cameraOffset = GetComponent<XROrigin>().CameraFloorOffsetObject.transform;
            //transform.rotation *= Quaternion.AngleAxis(30, Vector3.forward);
        }

        // Update is called once per frame
        void Update()
        {
            //ComputeAcceleration();
            

            // Recoil from roll
            //Quaternion target = Quaternion.Euler(Vector3.forward * -cameraOffset.transform.localEulerAngles.z);
            //cameraOffset.localRotation = Quaternion.Lerp(cameraOffset.localRotation, target, rollRecoilSpeed * Time.deltaTime);

            // If not grounded add gravity
            if (!IsGrounded())
            {
                currentVelocity += Vector3.down * gravityAcceleration * Time.deltaTime;
            }
            Debug.Log("CurrentVelocity Before:" + currentVelocity.y);
            ComputeResistanceAndTorque();
            Debug.Log("CurrentVelocity After:" + currentVelocity.y);
            // Get current velocity
            //currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, velocitySmoothTime * Time.deltaTime);
            //currentVelocity = targetVelocity;
            currentRollSpeed = Mathf.MoveTowards(currentRollSpeed, targetRollSpeed, rollSmoothTime * Time.deltaTime);
            //Debug.Log("TargetVelocity:" + targetVelocity);
            Debug.Log("CurrentVelocity:" + currentVelocity);
            cameraOffset.rotation = Quaternion.AngleAxis(cameraOffset.localEulerAngles.z + currentRollSpeed * Time.deltaTime, cameraOffset.forward);

            //characterController.Move(currentVelocity * Time.deltaTime);
            //characterController.Move(Vector3.forward * 5 * Time.deltaTime);
        }

        void ComputeResistanceAndTorque()
        {
            // Computing resistance and torque depending on the current velocity
            if (true)//currentVelocity.magnitude > 0)
            {
                Vector3 leftWingRes = Vector3.zero, rightWingRes = Vector3.zero;
                Vector3 leftTorque = Vector3.zero, rightTorque = Vector3.zero;

                // Left wing
                if (leftWingStretchRatio > 0)
                {
                    float resFactor = leftWingStretchRatio * wingsResistanceFactor / 2f;
                    // Project the velocity on the wing normal to compute the resistance 
                    leftWingRes = Vector3.Project(currentVelocity * resFactor, leftWingPivot.up);

                    // We are not upside down
                    if (Vector3.Dot(currentVelocity, leftWingRes) > 0)
                        leftWingRes *= -1;

                    Debug.Log("LeftWingRes:" + leftWingRes);
                }

                // Right wing
                if (rightWingStretchRatio > 0)
                {
                    float resFactor = rightWingStretchRatio * wingsResistanceFactor / 2f;
                    // Project the velocity on the wing normal to compute the resistance 
                    rightWingRes = Vector3.Project(currentVelocity * resFactor, rightWingPivot.up);

                    // We are not upside down
                    if (Vector3.Dot(currentVelocity, rightWingRes) > 0)
                        rightWingRes *= -1;

                    Debug.Log("RightWingRes:" + rightWingRes);
                }

                // Update target velocity
                Vector3 res = (leftWingRes + rightWingRes);
                if (res.magnitude > currentVelocity.magnitude)
                    res = res.normalized * currentVelocity.magnitude;
                    
                currentVelocity += res;

                // Compute torque
                leftTorque = Vector3.Project(leftWingRes, body.up);
                rightTorque = Vector3.Project(rightWingRes, body.up);
                float leftSign = Mathf.Sign(Vector3.Dot(leftTorque, body.up));
                float rightSign = Mathf.Sign(Vector3.Dot(rightTorque, body.up));

                targetRollSpeed = (rightTorque.magnitude * rightSign - leftTorque.magnitude * leftSign) * 100;
                targetRollSpeed = Mathf.Clamp(targetRollSpeed, -maxRollSpeed, maxRollSpeed);

            }

          

        }

      
        #region public
        public bool IsGrounded()
        {
            return false;
        }
        #endregion

    }

}
