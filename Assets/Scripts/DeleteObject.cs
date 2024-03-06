using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DeleteObject : MonoBehaviour
{
    private ARRaycastManager aRRaycastManager;

    public void OnDeleteButtonPressed()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.AllTypes);
        foreach(ARRaycastHit hit in hits) {
            ToastNotification.PopUpMessage(hit.ToString());
        }
    }
}
