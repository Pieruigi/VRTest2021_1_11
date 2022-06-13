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
        float wingsFallResistanceFactor = 0.4f; 

        // 0 means the wing is closed, 1 is open
        // Actual surface is wingMaxSurface * stretchRatio
        float leftWingStretchRatio = 0;
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
            ComputeAcceleration();

            // If not grounded add gravity
            if (!IsGrounded())
            {
                targetVelocity += Vector3.down * gravityAcceleration * Time.deltaTime;
            }

            // Get current velocity
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, velocitySmoothTime * Time.deltaTime);
            Debug.Log("TargetVelocity:" + targetVelocity);
            Debug.Log("CurrentVelocity:" + currentVelocity);
        }

        void ComputeAcceleration()
        {
            // Planar acceleration
            // If the bird is falling we need to calculate the acceleration depending of the wings orientation
            // We only take into account the vertical component of the current velocity
            float vSpeed = currentVelocity.y;
            if(vSpeed < 0)
            {
                // The bird is falling, so new need to compute che planar acceleration
                // Create a vector to rapresent the vertical fall
                Vector3 fallVelocity = Vector3.up * vSpeed; // This vector looks down

                Vector3 leftGlide, rightGlide;
                Vector3 leftFallResistance, rightFallResistance;
                Vector3 glide = Vector3.zero;
                Vector3 fallResistance = Vector3.zero;

                // No glide with closed wings or upside down flying
                if(leftWingStretchRatio > 0 && leftWingPivot.up.y > 0)
                {
                    // The surface normal is the pivot Y coordinate
                    Vector3 wingNormal = leftWingPivot.up;
                    // The resistance magnitude
                    float resistance = leftWingStretchRatio * wingsFallResistanceFactor;
                    Debug.Log("Resistance:" + resistance);
                    // Glide acceleration is the projection of the resistance on the plane XZ
                    // The acceleration is computed locally ( so we need to apply it taking into account 
                    // the bird fwd projected on the XZ plane )
                    leftGlide = Vector3.ProjectOnPlane(wingNormal * resistance, Vector3.up);
                    leftGlide = leftGlide * Mathf.Abs(vSpeed) * Time.deltaTime;
                    // The fall resistance is the projection of the resistance on the Y axis
                    leftFallResistance = Vector3.Project(wingNormal * resistance, Vector3.up);
                    Debug.Log("LeftFallResistance_1:" + leftFallResistance);
                    leftFallResistance = leftFallResistance * Mathf.Abs(vSpeed) * Time.deltaTime;

                    Debug.Log("LeftGlide:" + leftGlide);
                    Debug.Log("LeftFallResistance.Y:" + leftFallResistance.y);

                    // Add
                    glide = leftGlide;
                    fallResistance = leftFallResistance;
                }

                // No glide with closed wings or upside down flying
                if (rightWingStretchRatio > 0 && rightWingPivot.up.y > 0)
                {
                    // The surface normal is the pivot Y coordinate
                    Vector3 wingNormal = rightWingPivot.up;
                    // The resistance magnitude
                    float resistance = rightWingStretchRatio * wingsFallResistanceFactor;
                    Debug.Log("Resistance:" + resistance);
                    // Glide acceleration is the projection of the resistance on the plane XZ
                    // The acceleration is computed locally ( so we need to apply it taking into account 
                    // the bird fwd projected on the XZ plane )
                    rightGlide = Vector3.ProjectOnPlane(wingNormal * resistance, Vector3.up);
                    rightGlide = rightGlide * Mathf.Abs(vSpeed) * Time.deltaTime;
                    // The fall resistance is the projection of the resistance on the Y axis
                    rightFallResistance = Vector3.Project(wingNormal * resistance, Vector3.up);
                    Debug.Log("RightFallResistance_1:" + rightFallResistance);
                    rightFallResistance = rightFallResistance * Mathf.Abs(vSpeed) * Time.deltaTime;

                    Debug.Log("RightGlide:" + rightGlide);
                    Debug.Log("RightFallResistance.Y:" + rightFallResistance.y);

                    // Add
                    glide = rightGlide;
                    fallResistance = rightFallResistance;
                }

                Debug.Log("TargetVelocity.Y:" + targetVelocity.y);
                // Adjust target velocity
                targetVelocity += fallResistance + glide;

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
