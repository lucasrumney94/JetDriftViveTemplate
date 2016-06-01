using UnityEngine;
using System.Collections;

public class VRInputManager : MonoBehaviour {

    public ControllerInputTracker controller1;
    public ControllerInputTracker controller2;

    public bool triggerDown
    {
        get
        {
            return trigger1Down || trigger2Down;
        }
    }

    public bool trigger1Down
    {
        get
        {
            return controller1.triggerDown;
        }
    }

    public bool trigger2Down
    {
        get
        {
            return controller2.triggerDown;
        }
    }

    public Vector3 controller1Position
    {
        get
        {
            return controller1.position;
        }
    }

    public Vector3 controller2Position
    {
        get
        {
            return controller2.position;
        }
    }

    public Quaternion controller1Rotation
    {
        get
        {
            return controller1.rotation;
        }
    }

    public Quaternion controller2Rotation
    {
        get
        {
            return controller2.rotation;
        }
    }
}
