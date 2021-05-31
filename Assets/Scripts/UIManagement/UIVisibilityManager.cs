using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisibilityManager : MonoBehaviour
{
    public GameObject[] interfaceElements;
    bool elementsHidden;

    void Update()
    {
        if (!elementsHidden && !HealthManager.playerAlive)
        {
            foreach (GameObject uiElement in interfaceElements)
            {
                uiElement.SetActive(false);
            }
            elementsHidden = true;
        }
        else if (elementsHidden && HealthManager.playerAlive)
        {
            foreach (GameObject uiElement in interfaceElements)
            {
                uiElement.SetActive(true);
            }
            elementsHidden = false;
        }
    }
}
