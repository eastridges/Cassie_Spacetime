using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.SceneManagement;

public class RunGame : MonoBehaviour
{
    //So you can write the the debugger screen
    public TextMeshPro debuggerText;

    //So you can get user inputs
    public InputReader inputs;

    public Transform rh;
    public Transform lh;
    public GameObject Planet;
    public Rigidbody Star;
    public Transform StarSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Clear the debugger screen when user presses B button
        if (inputs.ButtonBDown)
        {
            debuggerText.SetText("Debugger Log (Press B to clear):");
        }

        //reload the scene if the user presses x
        if(inputs.ButtonXDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (inputs.RightMainTriggerDown)
        {
            SpawnPlanet();
        }

        var sizeChange = inputs.leftJoystick[1];
        if (StarSize.localScale[0]>1 && sizeChange<0)
        {
            Vector3 scaleChange = new Vector3(sizeChange, sizeChange, sizeChange);
            StarSize.localScale += scaleChange * 0.05f;
            Star.mass=StarSize.localScale[0];
        }
        else if (StarSize.localScale[0]<20 && sizeChange>0)
        {
            Vector3 scaleChange = new Vector3(sizeChange, sizeChange, sizeChange);
            StarSize.localScale += scaleChange * 0.05f;
            Star.mass=StarSize.localScale[0];
        }
        
    }

    void SpawnPlanet()
    {
        GameObject newPlanet=Instantiate(Planet, rh.position, rh.rotation);
        var component = newPlanet.GetComponent<Grabbable>();
        component.Setup(inputs, lh, rh, debuggerText);
        var component2 = newPlanet.GetComponent<PlanetController>();
        component2.Setup(inputs, lh, rh, Star);
    }
}
