using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    [SerializeField]
    private GameObject placementIndicator;

    private Material canPlace;
    private Material cannotPlace;

    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private bool placeable = true;

    [SerializeField]
    private GameObject objectToPlace;
    [SerializeField]
    private LayerMask layerMask;

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        canPlace = (Material)Resources.Load<Material>("Hologreen");
        cannotPlace = (Material)Resources.Load<Material>("Holored");
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementIndicator.transform.GetChild(0).tag == "Hologram" && CheckCollider())
        {
            placeable = true;
            UpdateHoloColour();
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

    private void UpdateHoloColour() {
        Transform child = placementIndicator.transform.GetChild(0);
        if (child.GetComponent<MeshRenderer>() != null) {
            child.GetComponent<MeshRenderer>().material = GetMaterial();
        }
        for (int i = 0; i < child.transform.childCount; i++) {
            child.transform.GetChild(i).GetComponent<MeshRenderer>().material = GetMaterial();
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

    public Material GetMaterial() {
        if (placeable) {
            return canPlace;
        } else {
            return cannotPlace;
        }
    }

    private bool CheckCollider()
    {
        Collider objectCollider = placementIndicator.transform.GetChild(0).GetComponent<BoxCollider>();
        if (objectCollider == null)
        {
            return false;
        }

        Bounds objectBounds = objectCollider.bounds;

        // Check for overlaps with other colliders in the scene.
        Collider[] colliders = Physics.OverlapBox(
            objectBounds.center,
            objectBounds.extents,
            objectToPlace.transform.rotation,
            layerMask
        );

        foreach (Collider collider in colliders)
        {
            placeable = false;
            UpdateHoloColour();
            return false;
        }
        return true;
    }
}
