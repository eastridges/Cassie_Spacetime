using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public InputReader inputs;
    public Transform rh;
    public Transform lh;
    public Rigidbody rb;
    public float throwSpeed;

    private bool rightHandTouch;
    private bool leftHandTouch;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        leftHandTouch=false;
        rightHandTouch=false;
        rb=this.GetComponent<Rigidbody>();
        throwSpeed=10000;
    }

    // Update is called once per frame
    void Update()
    {
        //To allow the block to be picked up by the left hand
        if (leftHandTouch && inputs.LeftGrip)
        {
            rb.useGravity=false;
            rb.isKinematic=true;
            this.transform.SetParent(lh);
            lastPosition=lh.position;
        }
        if (inputs.LeftGripUp && leftHandTouch)
        {
            rb.useGravity=true;
            rb.isKinematic=false;
            this.transform.SetParent(null);
            Vector3 force = throwSpeed*(lh.position-lastPosition);
            rb.AddForce(force);
        }

        
        //to allow the block to be picked up by the right hand
        if (rightHandTouch && inputs.RightGrip)
        {
            rb.useGravity=false;
            rb.isKinematic=true;
            this.transform.SetParent(rh);
            lastPosition=rh.position;
        }
        if (inputs.RightGripUp && rightHandTouch)
        {
            rb.useGravity=true;
            rb.isKinematic=false;
            this.transform.SetParent(null);
            Vector3 force = throwSpeed*(rh.position-lastPosition);
            rb.AddForce(force);
        }
        


    }

    public void Setup(InputReader importInputReader, Transform importLeft, Transform importRight)
    {
        inputs=importInputReader;
        lh=importLeft;
        rh=importRight;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "RH")
        {
            rightHandTouch=true;
        }
        else if (col.gameObject.tag == "LH")
        {
            leftHandTouch=true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "RH")
        {
            rightHandTouch=false;
        }
        else if (col.gameObject.tag == "LH")
        {
            leftHandTouch=false;
        }
    }
}
