using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class PlanetController : MonoBehaviour
{
    //So you can get user inputs
    public InputReader inputs;

    public Transform lh;
    public Transform rh;
    public Rigidbody rb;

    public Rigidbody Star;
    public float deltaT=0.1f;

    private Vector3 currentPosition;
    private Vector3 acceleration;
    private Vector3 velocity;
    private float starMass;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.transform.IsChildOf(rh))
        {
            //Get the current position vector
            currentPosition = this.transform.position;
            
            //Get the current mass of the star
            starMass = Star.mass;

            acceleration = new Vector3(-(starMass/Mathf.Pow(Mathf.Pow(currentPosition[0],2f)+Mathf.Pow(currentPosition[1],2f)+Mathf.Pow(currentPosition[2],2f),1.5f)) * currentPosition[0],-(starMass/Mathf.Pow(Mathf.Pow(currentPosition[0],2f)+Mathf.Pow(currentPosition[1],2f)+Mathf.Pow(currentPosition[2],2f),1.5f)) * currentPosition[1],-(starMass/Mathf.Pow(Mathf.Pow(currentPosition[0],2f)+Mathf.Pow(currentPosition[1],2f)+Mathf.Pow(currentPosition[2],2f),1.5f)) * currentPosition[2]); 
            velocity = rb.velocity + acceleration * deltaT;
            rb.velocity = velocity;
            this.transform.position = currentPosition + velocity * deltaT;
        }
    }

    //when an object is spawned, this command must be called to pass the necessary objects from the scene
    public void Setup(InputReader importInputReader, Transform importLeft, Transform importRight, Rigidbody importStar)
    {
        inputs=importInputReader;
        lh=importLeft;
        rh=importRight;
        Star=importStar;
        starMass=Star.mass;
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Star")
        {
            Destroy(gameObject);
        }
    }

    /* The differential equation as vectors: d^2 r/ dt^2 = -(G m / ||r||^2) * (r/||r||)
    Where r is distance from the origin (where the star is located), t is time, G is the gravitational constant (and we will be using units so that G=1), 
    and m is the mass of the star (we will be using units so that the mass is a reasonable number).

    The equation broken down by components gives us a system of second order differential equations:
    ax = d^2 x/ dt^2 = -G m / (x^2 + y^2 + z^2)^(3/2) * x
    ay = d^2 y/ dt^2 = -G m / (x^2 + y^2 + z^2)^(3/2) * y
    az = d^2 z/ dt^2 = -G m / (x^2 + y^2 + z^2)^(3/2) * z
    Note that x, y, and z are all functions of t

    Then, by introducing new variables vx, vy, and vz, we can make this a system of first order differential equations:
    dx / dt = vx
    dy / dt = vy
    dz / dt = vz
    d vx/ dt = d^2 x/ dt^2 = -G m / (x^2 + y^2 + z^2)^(3/2) * x
    d vy/ dt = d^2 y/ dt^2 = -G m / (x^2 + y^2 + z^2)^(3/2) * y
    d vz/ dt = d^2 z/ dt^2 = -G m / (x^2 + y^2 + z^2)^(3/2) * z

    And now, we can use Euler's method or any method of approximating solutions to first order systems of equations we want 
    Note that Runge-Kutta is a much better way to approximate solutions
    Also note that this is not a linear system of differential equations and so an exact solution is unknown, although it can be shown to follow
    an elliptical path (see Trystan's senior project)
    




    */
}
