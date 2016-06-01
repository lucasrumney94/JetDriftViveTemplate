using UnityEngine;
using System.Collections;

public class ControllerInputTracker : MonoBehaviour {

    private Transform origin;

    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;

    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerDown;
    public bool triggerUp;
    public bool triggerHeld;

    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
    public bool gripDown;
    public bool gripUp;
    public bool gripHeld;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private SteamVR_TrackedObject trackedObject;
    public int index;

    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        index = (int)trackedObject.index;
        origin = trackedObject.origin ? trackedObject.origin : transform.parent;
    }

    void Update()
    {

        position = origin.TransformPoint(transform.position);
        rotation = transform.rotation;
        velocity = origin.TransformVector(controller.velocity);
        angularVelocity = origin.TransformVector(controller.angularVelocity);

        triggerDown = controller.GetPressDown(trigger);
        triggerUp = controller.GetPressUp(trigger);
        triggerHeld = controller.GetPress(trigger);

        gripDown = controller.GetPressDown(grip);
        gripUp = controller.GetPressUp(grip);
        gripHeld = controller.GetPress(grip);
    }
}
