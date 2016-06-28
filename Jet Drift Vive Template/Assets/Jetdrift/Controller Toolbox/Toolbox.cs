using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public struct Option
{
    public Type type;

    public bool boolValue;
    public float floatValue;

    public Option(ref float newFloat)
    {
        type = typeof(float);
        boolValue = false;
        floatValue = newFloat;
    }

    public Option(ref bool newBool)
    {
        type = typeof(bool);
        boolValue = newBool;
        floatValue = 0f;
    }
}

public class Toolbox : MonoBehaviour {

    private ControllerInputTracker inputTracker;

    public Canvas selectionCanvas;
    public Canvas toolOptionsCanvas;

    public VRTool[] toolPrefabs;

    private VRTool activeTool;
    private VRTool[] toolInstances; //When a tool is spawned cache a reference to it, and activate/deactivate that instance instead of creating and destroying instances

    void Start()
    {
        inputTracker = transform.GetComponentInParent<ControllerInputTracker>();

        inputTracker.menuPressedDown += new ControllerInputDelegate(StartListeningForLongPress);
        inputTracker.triggerTouchedDown += new ControllerInputDelegate(SelectHighlighted);
    }

    void Update()
    {
        ListenForLongPress();
    }

    #region Press length detection

    private bool listeningForLongPress = false;
    private float timeOfLastMenuDown;

    private void StartListeningForLongPress()
    {
        listeningForLongPress = true;
    }

    private void StopListeningForLongPress()
    {
        listeningForLongPress = false;
    }

    private void ListenForLongPress()
    {
        if (listeningForLongPress)
        {
            if (Time.time - timeOfLastMenuDown < 1f)
            {
                if (inputTracker.menuPressed == false)
                {
                    //Stop Listneing
                    StopListeningForLongPress();
                    //Do short press action
                    ToggleToolOptions();
                }
            }
            else
            {
                //Stop listening
                StopListeningForLongPress();
                //Do long press action
                ToggleToolboxMenu();
            }
        }
    }

    #endregion

    #region Toolbox menu

    public void ToggleToolboxMenu()
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

    private void OpenToolboxMenu()
    {
        //Temporarily disable active tool
        activeTool.gameObject.SetActive(false);

        //Open selection canvas
        selectionCanvas.gameObject.SetActive(true);

        //Populate canvas with a mneu item for eash tool
    }

    private void CloseToolboxMenu()
    {
        //Reeanble active tool
        activeTool.gameObject.SetActive(true);

        //Disable canvas
        selectionCanvas.gameObject.SetActive(false);
    }

    #endregion

    #region Tool Options Menu

    private void ToggleToolOptions()
    {

    }

    private void OpenToolOptions()
    {
        activeToolOptions = activeTool.toolOptions;

    }

    private void CloseToolOptions()
    {

    }

    #endregion
}
