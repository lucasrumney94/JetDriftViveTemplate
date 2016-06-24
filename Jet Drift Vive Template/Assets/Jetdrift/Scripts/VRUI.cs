using UnityEngine;
using System.Collections;

public class VRUI : MonoBehaviour
{

    //Define VR Input Controller
    private ControllerInputTracker vrInput;
    public int deviceIndex;


    //Define Canvases
    public Canvas debugCanvas;
    public Canvas OverlayCanvas;

    void Start ()
    {
        //Initialize VR Input
        deviceIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = transform.GetComponentInParent<ControllerInputTracker>();

        vrInput.menuPressedDown += new ControllerInputDelegate(ToggleDebugMenu);
        vrInput.touchpadTouchedDown += new ControllerInputDelegate(ToggleOverlayMenu);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void ToggleDebugMenu()
    {
        debugCanvas.enabled = !debugCanvas.enabled;
    }
    void ToggleOverlayMenu()
    {
        OverlayCanvas.enabled = !OverlayCanvas.enabled;
    }
}
