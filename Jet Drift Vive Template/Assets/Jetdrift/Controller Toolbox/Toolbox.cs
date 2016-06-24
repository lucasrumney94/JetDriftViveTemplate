using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Toolbox : MonoBehaviour {

    private ControllerInputTracker inputTracker;

    public Canvas selectionCanvas;

    public GameObject[] toolPrefabs;

    void Start()
    {
        inputTracker = transform.GetComponentInParent<ControllerInputTracker>();

        inputTracker.menuPressedDown += new ControllerInputDelegate(ToggleToolbocMenu);
    }

    public void ToggleToolbocMenu()
    {
        if (selectionCanvas.enabled)
        {
            CloseToolboxMenu();
        }
        else if (selectionCanvas.enabled == false)
        {
            OpenToolboxMenu();
        }
    }

    public void OpenToolboxMenu()
    {
        //Temporarily disable active tool

        //Several possibilities:
        //Enable UI canvas to select new tool that can be navigated with the touchpad (prefered option)
        //Spawn 3d object containing a representation of all tools, which is destoryed when one is selected by grabbing (would be cool, but unnecissary)

        //In either case, some object is enabled which has a reference to all the available tool prefabs

        selectionCanvas.enabled = true;

        //Populate canvas with a mneu item for eash tool
    }

    public void CloseToolboxMenu()
    {
        //Reeanble active tool

        //Disable canvas
        selectionCanvas.enabled = false;
    }
}
