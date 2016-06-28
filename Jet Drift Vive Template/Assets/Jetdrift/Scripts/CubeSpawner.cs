using UnityEngine;
using System.Collections;

public class CubeSpawner : VRTool {

    private ControllerInputTracker vrInput;
    public int deviceIndex;

    public GameObject cubePrefab;

    public float initialForce;
    public float scale;
    public bool spawnEveryFrame;

    void Start()
    {
        deviceIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = transform.GetComponentInParent<ControllerInputTracker>();

        vrInput.triggerPressedDown += new ControllerInputDelegate(SpawnCube);
    }

    void OnEnable()
    {
        InitializeOptions();
    }

    public override void InitializeOptions()
    {
        toolOptions = new Option[3];
        toolOptions[0] = new Option(ref initialForce);
        toolOptions[1] = new Option(ref scale);
        toolOptions[2] = new Option(ref spawnEveryFrame);
    }

    void Update()
    {
        if (vrInput.triggerStrength == 1f)
        {
            SpawnCube();
        }
    }

    public void SpawnCube()
    {
        GameObject newCube = Instantiate(cubePrefab, vrInput.position, vrInput.rotation) as GameObject;
        newCube.transform.position += newCube.transform.forward * 0.1f;
        newCube.GetComponent<Rigidbody>().velocity = newCube.transform.forward * initialForce;
    }
}
