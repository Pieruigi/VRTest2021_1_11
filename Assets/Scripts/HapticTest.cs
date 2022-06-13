using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticTest : MonoBehaviour
{
    XRController xrController;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        xrController = GetComponent<XRController>();
        //StartCoroutine(Haptic());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Haptic()
    {
        yield return new WaitForSeconds(2);
        Debug.Log(xrController.SendHapticImpulse(1, 300));
    }
}
