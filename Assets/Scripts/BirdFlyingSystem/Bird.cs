//#define TEST
using System.Collections;
using System.Collections.Generic;
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

        Vector3 currentVelocity, targetVelocity;

        /// <summary>
        /// This is the whole accelaration given by the sum of all the applied forces 
        /// </summary>
        Vector3 acceleration;

        Vector3 torque;

        float gravityAcceleration = 9.8f;
        float wingMaxSurface = 20;
        float velocitySmoothTime = 10;

        // 0 means you fall down like a rock, 1 you don't fall at all
        float wingsResistanceFactor = 0.8f; 

        // 0 means the wing is closed, 1 is open
        // Actual surface is wingMaxSurface * stretchRatio
        float leftWingStretchRatio = 1;
        float rightWingStretchRatio = 1;


        // Start is called before the first frame update
        void Start()
        {
#if TEST
            currentVelocity = _testVelocity;
#endif

           
        }

        // Update is called once per frame
        void Update()
        {
            //ComputeAcceleration();
            ComputeResistanceAndTorque();

            // If not grounded add gravity
            if (!IsGrounded())
            {
                targetVelocity += Vector3.down * gravityAcceleration * Time.deltaTime;
            }

            // Get current velocity
            //currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, velocitySmoothTime * Time.deltaTime);
            currentVelocity = targetVelocity;
            //Debug.Log("TargetVelocity:" + targetVelocity);
            Debug.Log("CurrentVelocity:" + currentVelocity);
        }

        void ComputeResistanceAndTorque()
        {
            // You must be in movement
            if (currentVelocity.magnitude == 0)
                return;

            Vector3 leftWingRes = Vector3.zero, rightWingRes = Vector3.zero;
            
            // Left wing
            if(leftWingStretchRatio > 0)
            {
                float resFactor = leftWingStretchRatio * wingsResistanceFactor;
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
                float resFactor = rightWingStretchRatio * wingsResistanceFactor;
                // Project the velocity on the wing normal to compute the resistance 
                rightWingRes = Vector3.Project(currentVelocity * resFactor, rightWingPivot.up);

                // We are not upside down
                if (Vector3.Dot(currentVelocity, rightWingRes) > 0)
                    rightWingRes *= -1;

                Debug.Log("RightWingRes:" + rightWingRes);
            }

            // Update target velocity
            targetVelocity += ( leftWingRes + rightWingRes ) * Time.deltaTime;

        }

      
        #region public
        public bool IsGrounded()
        {
            return false;
        }
        #endregion

    }

}
