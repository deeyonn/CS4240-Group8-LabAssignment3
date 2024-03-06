using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefab_holo;
    public GameObject placementIndicator;
    public GameObject placeObject;

    public void whenPressed() {
        ToastNotification.PopUpMessage(prefab.name);
        placeObject.GetComponent<ARTapToPlaceObject>().ChangePrefab(prefab);
        Destroy(placementIndicator.transform.GetChild(0).gameObject);
        Instantiate(prefab_holo, placementIndicator.transform);
    }
}
