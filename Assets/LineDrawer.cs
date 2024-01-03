using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public InputReader inputs;
    //numer of points on the "line"
    public float numPoints;
    public Transform lh;
    public GameObject Light;
    public Transform EventHorizon;
    public float rs=2;
    
    private LineRenderer drawLight;
    private List<Vector3> linePoints;
    

    // Start is called before the first frame update
    void Start()
    {
        linePoints = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.LeftMainTriggerDown)
        {
            makeNewLightRay();
        }
        if (inputs.LeftMainTrigger)
        {
            linePoints.Clear();
            linePoints=calculatePoints();
            drawLight.positionCount = linePoints.Count;
            drawLight.SetPositions(linePoints.ToArray());
        }
        EventHorizon.localScale=new Vector3(2*rs, 2*rs, 2*rs);
    }

    public void makeNewLightRay()
    {
        GameObject LightRay = Instantiate(Light, lh.position, Quaternion.identity);
        drawLight = LightRay.GetComponent<LineRenderer>();
    }

    private List<Vector3> calculatePoints()
    {
        Vector3 startingPoint = lh.position;
        //float distanceFromStar = Mathf.Pow(Mathf.Pow(startingPoint[0],2) + Mathf.Pow(startingPoint[1],2) + Mathf.Pow(startingPoint[2],2),0.5f);
        //the divided by 3 makes the line continue 3 times as far on the other side of the sun as you are from the sun currently
        //float deltaSpace = distanceFromStar/(numPoints/3);


        //commented old starting point values out
        float h = 0.03f;
        //float x0 = startingPoint.x;
        
        //changed code (did say y0=0)
        //float y0 = startingPoint.y;
        //float y0=0;

        //float z0 = startingPoint.z;

        //rotation code
        Vector3 finalPlaneNormal = Vector3.Cross(startingPoint, lh.forward);
        float angleBetween = (Mathf.PI/180)*Vector3.Angle(finalPlaneNormal,Vector3.up);
        Vector3 rotationAxis = Vector3.Cross(finalPlaneNormal,Vector3.up);
        if (RotateAround(finalPlaneNormal,rotationAxis,angleBetween).normalized!=Vector3.up)
        {
            angleBetween=-angleBetween;
        }
        Vector3 newStart = RotateAround(startingPoint, rotationAxis, angleBetween);
        Vector3 newForward = RotateAround(startingPoint+lh.forward*0.1f, rotationAxis, angleBetween);

        //added code: new values of x0,y0,z0 and x1,y1,z1
        float x0=newStart.x;
        float y0=newStart.y;
        float z0=newStart.z;
        float x1=newForward.x;
        float y1=newForward.y;
        float z1=newForward.z;
        

        float r0 = Mathf.Sqrt(Mathf.Pow(x0,2)+Mathf.Pow(y0,2)+Mathf.Pow(z0,2));
        float theta0 = Mathf.Atan(z0/x0);
        
        //added code
        //float phi0 = Mathf.Acos(y0/r0);
        float phi0 = Mathf.PI/2;
        float initialPhi = Mathf.Acos(y0/r0);

        float t0=0;
        if (x0<0)
        {
            theta0=theta0 + Mathf.PI;
        }

        //commented out old values of x1,y1,z1
        //float x1 = x0+lh.forward.x*0.1f;
        
        //changed code (did say y1=0)
        //float y1 = y0 + lh.forward.y*0.1f;
        //float y1=0;

        //float z1 = z0+lh.forward.z*0.1f;
        float theta1 = Mathf.Atan(z1/x1);
        float r1 = Mathf.Sqrt(Mathf.Pow(x1,2)+Mathf.Pow(y1,2)+Mathf.Pow(z1,2));

        //added code
        //float phi1 = Mathf.Acos(y1/r1);
        float phi1 = Mathf.PI/2;

        //added code
        float dphi = phi1-phi0;

        if (x1<0)
        {
            theta1=theta1 + Mathf.PI;
        }
        
        float dr = r1-r0;
        float dtheta = theta1-theta0;
        //this is to fix the problem of when theta1 and theta0 are on opposite sides of the line where theta=3pi/2
        if (dtheta>6)
        {
            dtheta = dtheta-Mathf.PI*2;
        }
        else if (dtheta < -6)
        {
            dtheta = dtheta+Mathf.PI*2;
        }

        //changed code (added a term in the numerator)
        float dt = Mathf.Sqrt((Mathf.Pow(1-rs/r0,-1)*dr*dr + r0*r0*dtheta*dtheta + r0*r0*Mathf.Sin(theta0)*Mathf.Sin(theta0)*dphi*dphi)/(1-rs/r0));

        //float ddr = 0;
        float ddt = 0;
        float ddtheta = 0;

        //added code
        float ddphi = 0;

        float sign = -1;
        if (dr>0)
        {
            sign=1;
        }


        List<Vector3> points = new List<Vector3>();
        for (int i=0; i<numPoints; i++)
        {
            if (r0<=rs)
            {
                break;
            }

            //euler's method goes here
            ddt = -(rs/(r0*r0-r0*rs))*dt*dr;
            //ddr = (1/2)*(rs/Mathf.Pow(r0,2) - Mathf.Pow(rs,2)/Mathf.Pow(r0,3))*dt*dt + (1/2)*(rs/(r0*rs - Mathf.Pow(r0,2)))*dr*dr + (rs-r0)*dtheta*dtheta;
            
            //changed code (didn't have the last term)
            ddtheta = -(2/r0)*dr*dtheta + Mathf.Sin(theta0)*Mathf.Cos(theta0)*dphi*dphi;

            //added code
            ddphi = -(2/r0)*dr*dphi - 2*(Mathf.Cos(theta0)/Mathf.Sin(theta0))*dphi*dtheta;

            if (Mathf.Abs(dr)<.001)
            {
                sign = 1;
            }

            //changed code (added the dphi^2 term in the numerator)
            dr = sign*Mathf.Sqrt(((1-rs/r0)*dt*dt - r0*r0*dtheta*dtheta - r0*r0*Mathf.Sin(theta0)*Mathf.Sin(theta0)*dphi*dphi)*(1-rs/r0));

            //euler's update
            r0=r0+h*dr;
            t0=t0+h*dt;
            theta0=theta0+h*dtheta;

            //added code
            phi0 = phi0 + h*dphi;

            dt = dt + h*ddt;
            //dr = dr + h*ddr;
            dtheta = dtheta + h*ddtheta;

            //added code
            dphi = dphi + h*ddphi;
            
            //if r0 became NaN, exit the for loop
            if (r0 != r0)
            {
                break;
            }
            
            //changed code (used to have y=0 and x and z just polar conversions)
            Vector3 cartPoint = new Vector3(r0*Mathf.Sin(phi0)*Mathf.Cos(theta0), r0*Mathf.Cos(phi0), r0*Mathf.Sin(phi0)*Mathf.Sin(theta0));

            //added code
            cartPoint = RotateAround(cartPoint, rotationAxis, -angleBetween);
            //Vector3 rotationVector = lh.forward - Vector3.Project(lh.forward, lh.position);
            //cartPoint = RotateAround(cartPoint, rotationVector, Mathf.PI/2-initialPhi);
            

            points.Add(cartPoint);
        }
        return points;
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
