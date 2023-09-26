using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class RightHandController : MonoBehaviour
{

    public InputReader Inputs;
    public TextMeshPro debuggerText;
    public Transform RightHand;


    // Start is called before the first frame update
    void Start()
    {
        this.transform.forward = new Vector3(RightHand.forward.x + 90f, RightHand.forward.y, RightHand.forward.z);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = new Vector3(RightHand.forward.x + 90f, RightHand.forward.y, RightHand.forward.z);
        if (Inputs.ButtonADown)
        {
            debuggerText.SetText(debuggerText.text + '\n' + this.transform.forward.ToString() + RightHand.forward.ToString());
        }
    }
}
