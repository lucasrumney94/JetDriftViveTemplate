using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class VRControllerInputModule : PointerInputModule
{
    [SerializeField]
    private float m_InputInteractionsPerSecond;

    public float inputActionsPerSecond
    {
        get { return m_InputInteractionsPerSecond; }
        set { m_InputInteractionsPerSecond = value; }
    }

    public override bool ShouldActivateModule()
    {
        if (!base.ShouldActivateModule())
        {
            return false;
        }

        bool shouldActivate = false; //Need to set to true when trigger is pressed

        return shouldActivate;
    }

    public override void Process()
    {
        throw new NotImplementedException();
    }
}
