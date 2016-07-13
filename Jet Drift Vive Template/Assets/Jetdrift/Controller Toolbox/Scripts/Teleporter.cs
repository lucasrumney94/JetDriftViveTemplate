using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer), typeof(ParabolicRaycaster))]
public class Teleporter : VRTool
{
    private ControllerInputTracker inputTracker;
    private PlayerPositionTracker positionTracker;
    private LineRenderer beamRenderer;
    private ParabolicRaycaster raycaster;

    public int raycastSegments;
    public float raycastTravelDistance;
    public float raycastVelocity;
    
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
        raycaster = GetComponent<ParabolicRaycaster>();
    }

    void Update()
    {
        if (castingBeam)
        {
            RaycastBeam();
        }
    }

    public override void InitializeOptions()
    {
        toolOptions = new Option[2];
        toolOptions[0] = new Option(new ReferenceValue<float>(() => raycastTravelDistance, v => { raycastTravelDistance = v; }), "\'Travel time\' of arc", 1f, 5f);
        toolOptions[1] = new Option(new ReferenceValue<float>(() => raycastVelocity, v => { raycastVelocity = v; }), "Arc initial velocity", 5f, 50f);
    }

    private void StartCastingBeam()
    {
        castingBeam = true;
    }

    private void RaycastBeam()
    {
        Ray beamRay = new Ray(transform.position, transform.forward);
        RaycastHit beamHit;

        if (raycaster.ParabolicRaycast(out beamHit, transform.position, transform.eulerAngles, raycastTravelDistance, raycastVelocity, raycastSegments))
        {
            beamRenderer.enabled = true;
            hitPosition = beamHit.point;
            validTeleport = true;

            beamRenderer.SetVertexCount(raycastSegments + 2);
            beamRenderer.SetPositions(ParabolicRaycaster.ParabolicPointsList(transform.position, transform.eulerAngles, beamHit.distance, raycastVelocity, raycastSegments));
            beamRenderer.SetPosition(raycastSegments + 1, hitPosition);
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
        positionTracker.SetHeadPosition(hitPosition);
        SteamVR_Fade.Start(Color.black, 0.1f);
    }
}
