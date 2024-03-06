using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    public string prefabname;
    public GameObject placementIndicator;
    public GameObject placeObject;

    public void whenPressed() {
        placeObject.GetComponent<ARTapToPlaceObject>().ChangePrefab((GameObject)Resources.Load(prefabname));
        Destroy(placementIndicator.transform.GetChild(0).gameObject);
        GameObject hologram = Instantiate((GameObject)Resources.Load(prefabname), placementIndicator.transform);
        hologram.tag = "Hologram";
        hologram.layer = 7;
    }
}
