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
        float deltaSpace = 0.5f;
        List<Vector3> points = new List<Vector3>();
        for (int i=0; i<numPoints; i++)
        {
            points.Add(startingPoint+lh.forward.normalized * deltaSpace * i);
        }
        return points;
    }
}
