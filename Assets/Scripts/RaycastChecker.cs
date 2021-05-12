using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastChecker : MonoBehaviour
{
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    GraphicRaycaster graphicRaycaster;

    void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (Inventory.inventoryOpened)
            CheckRaycastHits();
    }

    void CheckRaycastHits()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.tag == "Slot")
            {
                result.gameObject.GetComponent<SlotManager>().ManageMouseInput(result.gameObject.name.Split('_')[1]);
            }
        }
    }
}
