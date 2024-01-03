using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotation : MonoBehaviour
{
    public Transform sphere;
    public InputReader inputs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.ButtonYDown)
        {
            Vector3 rotationVector = new Vector3(0,0,3);
            sphere.position = RotateAround(sphere.position, rotationVector, Mathf.PI/6);
        }
    }

    private Vector3 RotateAround(Vector3 point, Vector3 axisVector, float angle)
    {
        axisVector = axisVector.normalized;
        float ux = axisVector.x;
        float uy = axisVector.y;
        float uz = axisVector.z;
        float c = Mathf.Cos(angle);
        float s = Mathf.Sin(angle);
        float newX = point.x*(c+ux*ux*(1-c)) + point.y*(ux*uy*(1-c)-uz*s) + point.z*(ux*uz*(1-c)+uy*s);
        float newY = point.x*(uy*ux*(1-c)+uz*s) + point.y*(c+uy*uy*(1-c)) + point.z*(uy*uz*(1-c)-ux*s);
        float newZ = point.x*(uz*ux*(1-c)-uy*s) + point.y*(uz*uy*(1-c)+ux*s) + point.z*(c+uz*uz*(1-c));
        Vector3 finalPoint = new Vector3(newX,newY,newZ);
        return finalPoint;
    }
}
