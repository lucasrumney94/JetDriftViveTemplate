using UnityEngine;
using System.Collections;

public delegate void ControllerInputDelegate();

public class ControllerInputTracker : MonoBehaviour {

    private Transform origin;
    private ushort maxPulseLength = 3999;

    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;

    public Vector2 touchpadAxis;
    private Vector2 lastTouchpadAxis;
    public Vector2 touchpadAxisDelta;

    public float touchpadAngle;
    private float lastTouchpadAngle;
    public float touchpadAngleDelta;

    public float dpadDeadzone = 0.25f; //Inside a circle of this radius at the center of the touchpad no direction is registered
    public string dpadDirection = "None";
    private string lastDpadDirection;

    public float triggerStrength;
    private float lastTriggerStrength;
    public float triggerStrengthDelta;

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

    public event ControllerInputDelegate dpadUpTouchedStart;
    public event ControllerInputDelegate dpadDownTouchedStart;
    public event ControllerInputDelegate dpadRightTouchedStart;
    public event ControllerInputDelegate dpadLeftTouchedStart;

    public event ControllerInputDelegate dpadUpTouchedEnd;
    public event ControllerInputDelegate dpadDownTouchedEnd;
    public event ControllerInputDelegate dpadRightTouchedEnd;
    public event ControllerInputDelegate dpadLeftTouchedEnd;

    public event ControllerInputDelegate dpadUpPressedStart;
    public event ControllerInputDelegate dpadDownPressedStart;
    public event ControllerInputDelegate dpadRightPressedStart;
    public event ControllerInputDelegate dpadLeftPressedStart;

    public event ControllerInputDelegate dpadUpPressedEnd;
    public event ControllerInputDelegate dpadDownPressedEnd;
    public event ControllerInputDelegate dpadRightPressedEnd;
    public event ControllerInputDelegate dpadLeftPressedEnd;

    //Events cannot be passed as arguements, so there is no simple way to reference them cleanly

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

    private void OnDpadUpTouchedStart()
    {
        if (dpadUpTouchedStart != null)
        {
            dpadUpTouchedStart();
        }
    }

    private void OnDpadDownTouchedStart()
    {
        if (dpadDownTouchedStart != null)
        {
            dpadDownTouchedStart();
        }
    }

    private void OnDpadRightTouchedStart()
    {
        if (dpadRightTouchedStart != null)
        {
            dpadRightTouchedStart();
        }
    }

    private void OnDpadLeftTouchedStart()
    {
        if (dpadLeftTouchedStart != null)
        {
            dpadLeftTouchedStart();
        }
    }

    private void OnDpadUpTouchedEnd()
    {
        if (dpadUpTouchedEnd != null)
        {
            dpadUpTouchedEnd();
        }
    }

    private void OnDpadDownTouchedEnd()
    {
        if (dpadDownTouchedEnd != null)
        {
            dpadDownTouchedEnd();
        }
    }

    private void OnDpadRightTouchedEnd()
    {
        if (dpadRightTouchedEnd != null)
        {
            dpadRightTouchedEnd();
        }
    }

    private void OnDpadLeftTouchedEnd()
    {
        if (dpadLeftTouchedEnd != null)
        {
            dpadLeftTouchedEnd();
        }
    }

    private void OnDpadUpPressedStart()
    {
        if (dpadUpPressedStart != null)
        {
            dpadUpPressedStart();
        }
    }

    private void OnDpadDownPressedStart()
    {
        if (dpadDownPressedStart != null)
        {
            dpadDownPressedStart();
        }
    }

    private void OnDpadRightPressedStart()
    {
        if (dpadRightPressedStart != null)
        {
            dpadRightPressedStart();
        }
    }

    private void OnDpadLeftPressedStart()
    {
        if (dpadLeftPressedStart != null)
        {
            dpadLeftPressedStart();
        }
    }

    private void OnDpadUpPressedEnd()
    {
        if (dpadUpPressedEnd != null)
        {
            dpadUpPressedEnd();
        }
    }

    private void OnDpadDownPressedEnd()
    {
        if (dpadDownPressedEnd != null)
        {
            dpadDownPressedEnd();
        }
    }

    private void OnDpadRightPressedEnd()
    {
        if (dpadRightPressedEnd != null)
        {
            dpadRightPressedEnd();
        }
    }

    private void OnDpadLeftPressedEnd()
    {
        if (dpadLeftPressedEnd != null)
        {
            dpadLeftPressedEnd();
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

        triggerStrength = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
        triggerStrengthDelta = triggerStrength - lastTriggerStrength;

        touchpadAxis = controller.GetAxis();
        touchpadAxisDelta = touchpadAxis = lastTouchpadAxis;

        touchpadAngle = GetTouchpadAngle(touchpadAxis);
        touchpadAngleDelta = touchpadAngle - lastTouchpadAngle;

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

        //Check for touchpad direction
        if (touchpadPressed == true)
        {
            dpadDirection = GetDPadDirection(touchpadAngle);
        }
        else if (touchpadTouched == true)
        {
            dpadDirection = GetDPadDirection(touchpadAngle);
        }
        else
        {
            dpadDirection = "None";
        }

        if (dpadDirection != lastDpadDirection)
        {
            if (touchpadPressed == true)
            {
                DpadPressEnd(lastDpadDirection);
                DpadPressStart(lastDpadDirection);
            }
            else if (touchpadTouched == true)
            {
                DpadTouchEnd(lastDpadDirection);
                DpadTouchStart(dpadDirection);
            }
        }

        lastTriggerStrength = triggerStrength;
        lastTouchpadAxis = touchpadAxis;
        lastTouchpadAngle = touchpadAngle;
        lastDpadDirection = dpadDirection;
    }

    public void VibrateController(ushort microSeconds)
    {
        ushort duration = microSeconds > maxPulseLength ? maxPulseLength : microSeconds;
        controller.TriggerHapticPulse(duration);
    }

    private float GetTouchpadAngle(Vector2 touchpadAxis)
    {
        if (touchpadAxis == Vector2.zero)
        {
            return 0f;
        }
        float angle = Mathf.Atan2(touchpadAxis.y, touchpadAxis.x) * Mathf.Rad2Deg;
        if (angle < 0f)
        {
            angle += 360f;
        }
        return angle;
    }

    private string GetDPadDirection(float angle)
    {
        if (touchpadAxis.magnitude < dpadDeadzone)
        {
            return "None";
        }
        else if (45f < angle && angle <= 135f)
        {
            return "Up";
        }
        else if (135f < angle && angle <= 225f)
        {
            return "Left";
        }
        else if (225f < angle && angle <= 315f)
        {
            return "Down";
        }
        else
        {
            return "Right";
        }
    }

    private void DpadTouchStart(string direction)
    {
        if (direction == "Up")
        {
            OnDpadUpTouchedStart();
        }
        else if (direction == "Down")
        {
            OnDpadDownTouchedStart();
        }
        else if (direction == "Right")
        {
            OnDpadRightTouchedStart();
        }
        else if (direction == "Left")
        {
            OnDpadLeftTouchedStart();
        }
    }

    private void DpadTouchEnd(string direction)
    {
        if (direction == "Up")
        {
            OnDpadUpTouchedEnd();
        }
        else if (direction == "Down")
        {
            OnDpadDownTouchedEnd();
        }
        else if (direction == "Right")
        {
            OnDpadRightTouchedEnd();
        }
        else if (direction == "Left")
        {
            OnDpadLeftTouchedEnd();
        }
    }

    private void DpadPressStart(string direction)
    {
        if (direction == "Up")
        {
            OnDpadUpPressedStart();
        }
        else if (direction == "Down")
        {
            OnDpadDownPressedStart();
        }
        else if (direction == "Right")
        {
            OnDpadRightPressedStart();
        }
        else if (direction == "Left")
        {
            OnDpadLeftPressedStart();
        }
    }

    private void DpadPressEnd(string direction)
    {
        if (direction == "Up")
        {
            OnDpadUpPressedEnd();
        }
        else if (direction == "Down")
        {
            OnDpadDownPressedEnd();
        }
        else if (direction == "Right")
        {
            OnDpadRightPressedEnd();
        }
        else if (direction == "Left")
        {
            OnDpadLeftPressedEnd();
        }
    }
}
