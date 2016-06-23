using UnityEngine;
using System.Collections;

public delegate void ControllerInputDelegate();

public class ControllerInputTracker : MonoBehaviour {

    private Transform origin;

    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;

    public Vector2 touchpadAxis;

    public float triggerAxis;

    public bool triggerTouched; //Light pull
    public bool triggerPressed; //Heavy pull, triggers directly before 'click' on trigger is heard
    public bool gripPressed;
    public bool menuPressed;
    public bool touchpadTouched;
    public bool touchpadPressed;

    public event ControllerInputDelegate triggerTouchedDown;
    public event ControllerInputDelegate triggerTouchedUp;

    public event ControllerInputDelegate triggerPressedDown;
    public event ControllerInputDelegate triggerPressedUp;

    public event ControllerInputDelegate gripPressedDown;
    public event ControllerInputDelegate gripPressedUp;

    public event ControllerInputDelegate menuPressedDown;
    public event ControllerInputDelegate menuPressedUp;

    public event ControllerInputDelegate touchpadTouchedDown;
    public event ControllerInputDelegate touchpadTouchedUp;

    public event ControllerInputDelegate touchpadPressedDown;
    public event ControllerInputDelegate touchpadPressedUp;

    //possibly add events for each quadrant of touchpad?

    private void OnTriggerTouchedDown()
    {
        if (triggerTouchedDown != null)
        {
            triggerTouchedDown();
        }
    }

    private void OnTriggerTouchedUp()
    {
        if (triggerTouchedUp != null)
        {
            triggerTouchedUp();
        }
    }

    private void OnTriggerPressedDown()
    {
        if (triggerPressedDown != null)
        {
            triggerPressedDown();
        }
    }

    private void OnTriggerPressedUp()
    {
        if (triggerPressedUp != null)
        {
            triggerPressedUp();
        }
    }

    private void OnGripPressedDown()
    {
        if (gripPressedDown != null)
        {
            gripPressedDown();
        }
    }

    private void OnGripPressedUp()
    {
        if (gripPressedUp != null)
        {
            gripPressedUp();
        }
    }

    private void OnMenuPressedDown()
    {
        if (menuPressedDown != null)
        {
            menuPressedDown();
        }
    }

    private void OnMenuPressedUp()
    {
        if (menuPressedUp != null)
        {
            menuPressedUp();
        }
    }

    private void OnTouchpadTouchedDown()
    {
        if (touchpadTouchedDown != null)
        {
            touchpadTouchedDown();
        }
    }

    private void OnTouchpadTouchedUp()
    {
        if (touchpadTouchedUp != null)
        {
            touchpadTouchedUp();
        }
    }

    private void OnTouchpadPressedDown()
    {
        if (touchpadPressedDown != null)
        {
            touchpadPressedDown();
        }
    }

    private void OnTouchpadPressedUp()
    {
        if (touchpadPressedUp != null)
        {
            touchpadPressedUp();
        }
    }

    private ulong triggerMask = SteamVR_Controller.ButtonMask.Trigger;
    private ulong gripMask = SteamVR_Controller.ButtonMask.Grip;
    private ulong menuMask = SteamVR_Controller.ButtonMask.ApplicationMenu;
    private ulong touchpadMask = SteamVR_Controller.ButtonMask.Touchpad;

    //private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    //public bool triggerDown;
    //public bool triggerUp;
    //public bool triggerHeld;

    //private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
    //public bool gripDown;
    //public bool gripUp;
    //public bool gripHeld;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private SteamVR_TrackedObject trackedObject;
    public int index;

    void Start()
    {
        triggerMask = SteamVR_Controller.ButtonMask.Trigger;
        gripMask = SteamVR_Controller.ButtonMask.Grip;
        menuMask = SteamVR_Controller.ButtonMask.ApplicationMenu;
        touchpadMask = SteamVR_Controller.ButtonMask.Touchpad;

        trackedObject = GetComponent<SteamVR_TrackedObject>();
        index = (int)trackedObject.index;
        origin = trackedObject.origin ? trackedObject.origin : transform.parent;
    }

    void Update()
    {
        position = origin.TransformPoint(transform.localPosition);
        rotation = transform.rotation;
        velocity = origin.TransformVector(controller.velocity);
        angularVelocity = origin.TransformVector(controller.angularVelocity);

        touchpadAxis = controller.GetAxis();
        triggerAxis = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;

        if (controller.GetTouchDown(triggerMask))
        {
            triggerTouched = true;
            OnTriggerTouchedDown();
        }
        if (controller.GetTouchUp(triggerMask))
        {
            triggerTouched = false;
            OnTriggerTouchedUp();
        }

        if (controller.GetPressDown(triggerMask))
        {
            triggerPressed = true;
            OnTriggerPressedDown();
        }
        if (controller.GetPressUp(triggerMask))
        {
            triggerPressed = false;
            OnTriggerPressedUp();
        }

        if (controller.GetPressDown(gripMask))
        {
            gripPressed = true;
            OnGripPressedDown();
        }
        if (controller.GetPressUp(gripMask))
        {
            gripPressed = false;
            OnGripPressedUp();
        }

        if (controller.GetPressDown(menuMask))
        {
            menuPressed = true;
            OnMenuPressedDown();
        }
        if (controller.GetPressUp(menuMask))
        {
            menuPressed = false;
            OnMenuPressedUp();
        }

        if (controller.GetTouchDown(touchpadMask))
        {
            touchpadTouched = true;
            OnTouchpadTouchedDown();
        }
        if (controller.GetTouchUp(touchpadMask))
        {
            touchpadTouched = false;
            OnTouchpadTouchedUp();
        }

        if (controller.GetPressDown(touchpadMask))
        {
            touchpadPressed = true;
            OnTouchpadPressedDown();
        }
        if (controller.GetPressUp(touchpadMask))
        {
            touchpadPressed = false;
            OnTouchpadPressedUp();
        }

        //triggerDown = controller.GetPressDown(trigger);
        //triggerUp = controller.GetPressUp(trigger);
        //triggerHeld = controller.GetPress(trigger);
        //triggerTouched = controller.GetTouch(trigger);

        //gripDown = controller.GetPressDown(grip);
        //gripUp = controller.GetPressUp(grip);
        //gripHeld = controller.GetPress(grip);
    }
}
