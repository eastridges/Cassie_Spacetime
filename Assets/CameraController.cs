using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class CameraController : MonoBehaviour
{
    public InputReader Inputs;
    public TextMeshPro debuggerText;
    public Transform RightHand;
    float yAngle;
    float movement;

    public float RotationSpeed;
    public float MovingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //rotation
        yAngle=Inputs.rightJoystick[0];
        this.transform.Rotate(0f,yAngle*RotationSpeed,0f,Space.World);

        //translation
        movement=Inputs.rightJoystick[1];
        this.transform.Translate(RightHand.forward*MovingSpeed*movement*Time.deltaTime, Space.World);

        //If the player presses right joystick, they drop back to the ground and view is levelled
        if (Inputs.RightJoystickButtonDown)
        {
            Vector3 newPosition = this.transform.position;
            newPosition.y=0f;
            this.transform.position=newPosition;
            Vector3 newAngles = this.transform.eulerAngles;
            newAngles.x = 0f;
            this.transform.eulerAngles=newAngles;
        }

        //Need Keyboard method of tilting up and down for debugging purposes, M tilts down, N tilts up
        if (Input.GetKey(KeyCode.M))
        {
            this.transform.Rotate(RotationSpeed, 0f, 0f, Space.Self);
        }
        if (Input.GetKey(KeyCode.N))
        {
            this.transform.Rotate(-RotationSpeed, 0f, 0f, Space.Self);
        }
        

    }
}
