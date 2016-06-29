using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrimitiveSpawner : VRTool
{

    private ControllerInputTracker vrInput;

    public List<GameObject> customPrimitives;
    public List<float> customPrimitiveScales;

    public float initialForce;
    public float primitiveScale = 1f;
    public bool RepeatSpawn;

    public int currentPrimitive;

    void OnEnable()
    {
        vrInput = transform.GetComponentInParent<ControllerInputTracker>();

        vrInput.triggerPressedDown += new ControllerInputDelegate(SpawnCurrentPrimitive);

        vrInput.touchpadPressedDown += new ControllerInputDelegate(NextPrimitive);

        InitializeOptions();
    }

    void OnDisable()
    {
        vrInput.triggerPressedDown -= new ControllerInputDelegate(SpawnCurrentPrimitive); //Remove tool controls from event list when disabled
    }

    public override void InitializeOptions()
    {
        toolOptions = new Option[3];
        toolOptions[0] = new Option(ref initialForce);
        toolOptions[1] = new Option(ref primitiveScale);
        toolOptions[2] = new Option(ref RepeatSpawn);
    }

    void Update()
    {
        if (RepeatSpawn && vrInput.triggerStrength == 1.0f && (Time.time%1.0==0))
        {
            SpawnCurrentPrimitive();
        }
    }

    public void SpawnCurrentPrimitive()
    {
        if (currentPrimitive == 0)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            temp.transform.position = vrInput.transform.position + vrInput.transform.forward * (primitiveScale * 1.2f); 
            temp.transform.localScale *= primitiveScale;
            temp.AddComponent<Rigidbody>().drag = 1.0f;
            temp.GetComponent<Rigidbody>().AddForce(initialForce * vrInput.transform.forward);

        }
        else if (currentPrimitive == 1)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            temp.transform.position = vrInput.transform.position + vrInput.transform.forward * (primitiveScale * 1.2f);
            temp.transform.localScale *= primitiveScale;
            temp.AddComponent<Rigidbody>().drag = 1.0f;
            temp.GetComponent<Rigidbody>().AddForce(initialForce * vrInput.transform.forward);
        }
        else if (currentPrimitive == 2)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            temp.transform.position = vrInput.transform.position + vrInput.transform.forward * (primitiveScale * 1.2f);
            temp.transform.localScale *= primitiveScale;
            temp.AddComponent<Rigidbody>().drag = 1.0f;
            temp.GetComponent<Rigidbody>().AddForce(initialForce * vrInput.transform.forward);
        }
        else if (currentPrimitive == 3)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Plane);
            temp.transform.position = vrInput.transform.position + vrInput.transform.forward * (primitiveScale * 1.2f);
            temp.transform.localScale *= primitiveScale;
            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Rigidbody>().drag = 1.0f;
            temp.GetComponent<Rigidbody>().AddForce(initialForce * vrInput.transform.forward);
        }
        else if (currentPrimitive == 4)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Quad);
            temp.transform.position = vrInput.transform.position + vrInput.transform.forward * (primitiveScale * 1.2f);
            temp.transform.localScale *= primitiveScale;
            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Rigidbody>().drag = 1.0f;
            temp.GetComponent<Rigidbody>().AddForce(initialForce * vrInput.transform.forward);
        }
        else if (currentPrimitive == 5)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            temp.transform.position = vrInput.transform.position + vrInput.transform.forward * (primitiveScale * 1.2f);
            temp.transform.localScale *= primitiveScale;
            temp.AddComponent<Rigidbody>().drag = 1.0f;
            temp.GetComponent<Rigidbody>().AddForce(initialForce * vrInput.transform.forward);
        }
        else if (currentPrimitive >= 6)
        {
            GameObject newCustomPrimitive = Instantiate(customPrimitives[currentPrimitive - 6], vrInput.position, vrInput.rotation) as GameObject;
            newCustomPrimitive.transform.position += newCustomPrimitive.transform.forward * customPrimitiveScales[currentPrimitive-6] * 1.2f;
            newCustomPrimitive.transform.localScale *= customPrimitiveScales[currentPrimitive - 6];
            newCustomPrimitive.AddComponent<Rigidbody>().drag = 1.0f;
            newCustomPrimitive.GetComponent<Rigidbody>().AddForce(initialForce * vrInput.transform.forward);
        }


    }

    public void NextPrimitive()
    {
        currentPrimitive++;
        if (currentPrimitive > 5 + customPrimitives.Count) //5 because 5 basic prims
        {
            currentPrimitive = 0;
        }
    }

}
