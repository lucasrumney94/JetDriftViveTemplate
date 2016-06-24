using UnityEngine;
using System.Collections;

public class VRUI : MonoBehaviour
{

    //Define VR Input Controller
    private ControllerInputTracker vrInput;
    public int deviceIndex;


    //Define Canvases
    public Canvas debugCanvas;

    void Start ()
    {
        //Initialize VR Input
        deviceIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = transform.GetComponentInParent<ControllerInputTracker>();

        vrInput.touchpadTouchedDown += new ControllerInputDelegate(ToggleDebugMenu);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void ToggleDebugMenu()
    {
        debugCanvas.enabled = !debugCanvas.enabled;
    }
}
