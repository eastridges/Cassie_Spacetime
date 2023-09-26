using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody rb;
    public Transform RightHand;
    public float initialForce;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        RightHand = GameObject.FindWithTag("RH").transform;
        rb=GetComponent<Rigidbody>();
        direction=RightHand.forward;
        rb.AddForce(initialForce*direction);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
