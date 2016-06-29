using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchpadMenuNavigator : MonoBehaviour {

    public GameObject currentSelected; //TODO: automatically assign currentSelected
    public EventSystem eventSystem; //TODO: automaticall assign eventSystem

    public ScrollRect scrollRect; //Optional parameter to help navigate menus inside a scrollable area

    private ControllerInputTracker inputTracker;

    void OnEnable()
    {
        inputTracker = transform.GetComponentInParent<ControllerInputTracker>();

        inputTracker.dpadUpTouchedStart += new ControllerInputDelegate(NavigateMenuUp);
        inputTracker.dpadDownTouchedStart += new ControllerInputDelegate(NavigateMenuDown);
        inputTracker.triggerPressedDown += new ControllerInputDelegate(MenuSelect);
    }

    void OnDisable()
    {
        inputTracker.dpadUpTouchedStart -= new ControllerInputDelegate(NavigateMenuUp);
        inputTracker.dpadDownTouchedStart -= new ControllerInputDelegate(NavigateMenuDown);
        inputTracker.triggerPressedDown -= new ControllerInputDelegate(MenuSelect);
    }

    private void NavigateMenuUp()
    {
        eventSystem.SetSelectedGameObject(currentSelected);
        AxisEventData axisData = new AxisEventData(eventSystem);
        axisData.moveDir = MoveDirection.Up;
        ExecuteEvents.Execute(currentSelected, axisData, ExecuteEvents.moveHandler);
        currentSelected = eventSystem.currentSelectedGameObject;
    }

    private void NavigateMenuDown()
    {
        eventSystem.SetSelectedGameObject(currentSelected);
        AxisEventData axisData = new AxisEventData(eventSystem);
        axisData.moveDir = MoveDirection.Down;
        ExecuteEvents.Execute(currentSelected, axisData, ExecuteEvents.moveHandler);
        currentSelected = eventSystem.currentSelectedGameObject;
    }

    private void MenuSelect()
    {
        eventSystem.SetSelectedGameObject(currentSelected);
        BaseEventData baseData = new BaseEventData(eventSystem);
        currentSelected = eventSystem.currentSelectedGameObject;
        baseData.selectedObject = currentSelected;
        ExecuteEvents.Execute(currentSelected, baseData, ExecuteEvents.submitHandler);
    }
}
