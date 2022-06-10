using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVector : MonoBehaviour
{
    [SerializeField]
    Transform a;

    [SerializeField]
    Transform b;

    Vector3 aInitialForward;

    // Start is called before the first frame update
    void Start()
    {
        aInitialForward = a.forward; // Wrong, it depends by the parent quaternion

        a.rotation = Quaternion.Inverse(a.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("--------------------------------------------------------");

            Vector3 aToB = (b.position - a.position).normalized;
            Vector3 cross = Vector3.Cross(aToB, a.forward);
            
            Debug.Log("A.Default:" + aInitialForward);
            Debug.Log("A:" + a.forward);
            Debug.Log("B:" + b.forward);
            Debug.Log("(A->B):" + aToB);
            Debug.Log("Angle:" + Vector3.Angle(aInitialForward, aToB));
            Debug.Log("Cross(A->B, A):" + cross);
            Debug.Log("SignedAngle(A->B):" + Vector3.SignedAngle(aInitialForward, aToB, cross));

            // Rotates A accordingly
            
            float hAngle = -Vector3.Angle(Vector3.ProjectOnPlane(aInitialForward, Vector3.up) , Vector3.ProjectOnPlane(aToB, Vector3.up)) * cross.y;
            Debug.Log("HAngle:" + hAngle);
            Quaternion rotH = Quaternion.AngleAxis(hAngle, Vector3.up);
            a.transform.rotation = rotH;
        }
        
    }
}
