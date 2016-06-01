using UnityEngine;
using System.Collections;

public class ControllerInputTracker : MonoBehaviour {

    public Vector3 position;
    public Quaternion rotation;

    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerDown;
    public bool triggerUp;
    public bool triggerHeld;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private SteamVR_TrackedObject trackedObject;

    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        position = transform.position;
        rotation = transform.rotation;
        triggerDown = controller.GetPressDown(trigger);
        if (triggerDown)
        {
            Debug.Log("Trigger was pressed!");
        }
    }
}
