using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    [SerializeField]
    private GameObject placementIndicator;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    [SerializeField]
    private GameObject objectToPlace;

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (CheckCollider())
        {
            if (
                placementPoseIsValid
                && Input.touchCount > 0
                && Input.GetTouch(0).phase == TouchPhase.Began
            )
            {
                if (
                    !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
                    && objectToPlace != null
                )
                {
                    PlaceObject();
                }
            }
        }
    }

    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(
                PlacementPose.position,
                PlacementPose.rotation
            );
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        Instantiate(
            objectToPlace,
            PlacementPose.position,
            PlacementPose.rotation * objectToPlace.transform.rotation
        );
    }

    public void ChangePrefab(GameObject gameObject)
    {
        objectToPlace = gameObject;
    }

    private bool CheckCollider()
    {
        Collider objectCollider = objectToPlace.GetComponent<BoxCollider>();
        if (objectCollider == null)
        {
            ToastNotification.PopUpMessage("Object does not have a BoxCollider component.");
            return false;
        }

        Bounds objectBounds = objectCollider.bounds;
        ToastNotification.PopUpMessage(objectBounds.ToString());

        // Check for overlaps with other colliders in the scene.
        Collider[] colliders = Physics.OverlapBox(
            objectBounds.center + PlacementPose.position,
            objectBounds.extents,
            objectToPlace.transform.rotation
        );

        ToastNotification.PopUpMessage(colliders.Length.ToString());

        foreach (Collider collider in colliders)
        {
            // Check if the collided object has the tag "Furniture".
            if (collider.CompareTag("Furniture"))
            {
                ToastNotification.PopUpMessage("Cannot place object, it will collide.");
                return false;
            }
        }
        return true;
    }
}
