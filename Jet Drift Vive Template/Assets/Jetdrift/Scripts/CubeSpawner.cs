using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

    private ControllerInputTracker vrInput;
    public int deviceIndex;

    public GameObject cubePrefab;

    public float initialForce;

    void Start()
    {
        deviceIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = transform.GetComponentInParent<ControllerInputTracker>();

        vrInput.triggerPressedDown += new ControllerInputDelegate(SpawnCube);
    }

    void Update()
    {
        if (vrInput.triggerAxis == 1f)
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
