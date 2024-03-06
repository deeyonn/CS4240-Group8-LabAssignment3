using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Relocate : MonoBehaviour
{
    [SerializeField]
    private GameObject bottomBar;
    [SerializeField]
    private GameObject relocateButton;
    [SerializeField]
    private GameObject editBar;
    [SerializeField]
    private GameObject interaction;
    [SerializeField]
    private GameObject placementIndicator;
    [SerializeField]
    private GameObject indicatorPlane;
    [SerializeField]
    private LayerMask layerMask;

    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private GameObject movingObject;
    private bool isRelocating;

    private void Start() {
        editBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0
            && Input.touches[0].phase == TouchPhase.Began
            && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
            && isRelocating
            && movingObject == null
            ) {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10, layerMask)) {
                relocateButton.SetActive(false);
                editBar.SetActive(true);
                movingObject = hit.collider.gameObject;
                previousPosition = hit.collider.gameObject.transform.position;
                previousRotation = hit.collider.gameObject.transform.rotation;
                Destroy(placementIndicator.transform.GetChild(0).gameObject);
                movingObject.transform.parent = placementIndicator.transform;
                movingObject.transform.localPosition = Vector3.zero;
                movingObject.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
        }
    }

    public void RelocateToPosition() {
        if (CheckCollision()) {
            movingObject.transform.parent = null;
            movingObject.transform.position = placementIndicator.transform.position;
            movingObject.transform.rotation = placementIndicator.transform.rotation * Quaternion.Euler(-90, 0, 0);
            
            Reset();
        }
    }

    public void ReturnToPrevious() {
        movingObject.transform.parent = null;
        movingObject.transform.position = previousPosition;
        movingObject.transform.rotation = previousRotation;

        Reset();
    }

    public void DeleteObject() {
        Destroy(movingObject);
        Reset();
    }

    private void Reset() {
        Instantiate(indicatorPlane, placementIndicator.transform);
        isRelocating = false;

        bottomBar.SetActive(true);
        editBar.SetActive(false);
        relocateButton.SetActive(true);
        movingObject = null;
    }

    public void RelocationButton() {
        if (isRelocating) {
            isRelocating = false;
            bottomBar.SetActive(true);
        } else {
            isRelocating = true;
            bottomBar.SetActive(false);
            Destroy(placementIndicator.transform.GetChild(0).gameObject);
            Instantiate(indicatorPlane, placementIndicator.transform);
        }
    }

    public bool CheckCollision() {
        Collider objectCollider = movingObject.GetComponent<BoxCollider>();
        if (objectCollider == null) {
            return false;
        }

        Bounds objectBounds = objectCollider.bounds;

        // Check for overlaps with other colliders in the scene.
        Collider[] colliders = Physics.OverlapBox(
            objectBounds.center,
            objectBounds.extents,
            movingObject.transform.rotation,
            layerMask
        );

        foreach (Collider collider in colliders) {
            if (collider.gameObject != movingObject) {
                return false;
            }
        }
        return true;
    }
}
