﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Teleporter : VRTool
{
    private ControllerInputTracker inputTracker;
    private PlayerPositionTracker positionTracker;
    private LineRenderer beamRenderer;

    public float maxTeleportDistance = 10f;
    public Vector3 hitPosition; //Worldspace position of beam endpoint
    public bool validTeleport = false; //Is hitPosition a valid location to teleport to?
    public bool castingBeam = false;

    void OnEnable()
    {
        InitializeOptions();

        inputTracker = transform.GetComponentInParent<ControllerInputTracker>();
        positionTracker = transform.GetComponentInParent<PlayerPositionTracker>();

        inputTracker.triggerPressedDown += new ControllerInputDelegate(StartCastingBeam);
        inputTracker.triggerPressedUp += new ControllerInputDelegate(StopCastingBeam);
    }

    void OnDisable()
    {
        inputTracker.triggerPressedDown -= new ControllerInputDelegate(StartCastingBeam); //Remove tool controls from event list when disabled
        inputTracker.triggerPressedUp -= new ControllerInputDelegate(StopCastingBeam);
    }

    void Start()
    {
        beamRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (castingBeam)
        {
            RaycastBeam();
        }
    }

    private void StartCastingBeam()
    {
        castingBeam = true;
    }

    private void RaycastBeam()
    {
        Ray beamRay = new Ray(transform.position, transform.forward);
        RaycastHit beamHit = new RaycastHit();

        if (Physics.Raycast(beamRay, out beamHit, maxTeleportDistance))
        {
            beamRenderer.enabled = true;
            hitPosition = beamHit.point;
            validTeleport = true;

            beamRenderer.SetPosition(0, transform.position);
            beamRenderer.SetPosition(1, hitPosition);
        }
        else
        {
            beamRenderer.enabled = false;
            hitPosition = Vector3.zero;
            validTeleport = false;
        }
    }

    private void StopCastingBeam()
    {
        if (castingBeam) //Here to keep function from triggering when tool is selected
        {
            castingBeam = false;
            beamRenderer.enabled = false;
            TryTeleport();
        }
    }

    private void TryTeleport()
    {
        if (validTeleport)
        {
            Debug.Log("Teleport is a valid location!");
            TeleportToHitPosition();
        }
        else
        {
            Debug.Log("Not a valid teleport location!");
        }
    }

    private void TeleportToHitPosition()
    {
        positionTracker.CameraRigPosition = hitPosition;
        SteamVR_Fade.Start(Color.black, 0.1f);
    }
}