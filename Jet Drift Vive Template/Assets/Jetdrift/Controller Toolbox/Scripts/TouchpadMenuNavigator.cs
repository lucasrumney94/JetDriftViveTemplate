﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchpadMenuNavigator : MonoBehaviour {

    public GameObject currentSelected;
    public EventSystem eventSystem; //TODO: automatically assign eventSystem

    public ScrollRect scrollRect; //Optional parameter to help navigate menus inside a scrollable area

    private ControllerInputTracker inputTracker;

    private RectTransform[] content;

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

    void Start()
    {
        content = transform.GetComponentsInChildren<RectTransform>();
    }

    void Update()
    {
        if (currentSelected == null)
        {
            currentSelected = scrollRect.content.transform.GetChild(0).gameObject;
        }
    }

    private void NavigateMenuUp()
    {
        eventSystem.SetSelectedGameObject(currentSelected);
        AxisEventData axisData = new AxisEventData(eventSystem);
        axisData.moveDir = MoveDirection.Up;
        ExecuteEvents.Execute(currentSelected, axisData, ExecuteEvents.moveHandler);
        currentSelected = eventSystem.currentSelectedGameObject;
        ScrollToActive();
    }

    private void NavigateMenuDown()
    {
        eventSystem.SetSelectedGameObject(currentSelected);
        AxisEventData axisData = new AxisEventData(eventSystem);
        axisData.moveDir = MoveDirection.Down;
        ExecuteEvents.Execute(currentSelected, axisData, ExecuteEvents.moveHandler);
        currentSelected = eventSystem.currentSelectedGameObject;
        ScrollToActive();
    }

    private void MenuSelect()
    {
        eventSystem.SetSelectedGameObject(currentSelected);
        BaseEventData baseData = new BaseEventData(eventSystem);
        currentSelected = eventSystem.currentSelectedGameObject;
        baseData.selectedObject = currentSelected;
        ExecuteEvents.Execute(currentSelected, baseData, ExecuteEvents.submitHandler);
    }

    private void ScrollToActive()
    {
        if (currentSelected != null)
        {
            RectTransform currentTransform = currentSelected.GetComponent<RectTransform>();
            RectTransform content = scrollRect.content;

            if (Mathf.Abs(currentTransform.anchoredPosition.y) < Mathf.Abs(content.offsetMax.y) || Mathf.Abs(currentTransform.anchoredPosition.y) > Mathf.Abs(content.rect.height + content.offsetMin.y))
            {
                Vector2 normalizedPosition = new Vector2(currentTransform.anchoredPosition.x / content.rect.width, currentTransform.anchoredPosition.y / content.rect.height);
                scrollRect.verticalNormalizedPosition = 1f - Mathf.Abs(normalizedPosition.y);
            }
        }
    }
}