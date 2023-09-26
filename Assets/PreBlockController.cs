using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBlockController : MonoBehaviour
{
    public Transform LeftHand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = LeftHand.position + 0.5f*LeftHand.forward;
        transform.rotation = LeftHand.rotation;
    }
}
