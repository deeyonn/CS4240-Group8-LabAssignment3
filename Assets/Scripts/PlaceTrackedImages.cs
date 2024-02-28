using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceTrackedImages : MonoBehaviour
{
    // Reference to the ARTrackedImageManager component.
    private ARTrackedImageManager _ARTrackedImageManager;

    // Reference to the prefabs to be instantiated.
    // Should be named the same as the image name in the ARTrackedImageManager.
    public GameObject[] _ARTrackedImagePrefabs;

    // Dictionary to keep track of the instantiated prefabs.
    private Dictionary<string, GameObject> _ARTrackedImagePrefabInstances =
        new Dictionary<string, GameObject>();

    void Awake()
    {
        // Get the ARTrackedImageManager component.
        _ARTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        // Subscribe to the ARTrackedImageManager's trackedImagesChanged event.
        _ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        // Unsubscribe from the ARTrackedImageManager's trackedImagesChanged event.
        _ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Event handler for the ARTrackedImageManager's trackedImagesChanged event.
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // For each added tracked image.
        foreach (var trackedImage in eventArgs.added)
        {
            // Get the name of the tracked image.
            string name = trackedImage.referenceImage.name;

            // Loop through the prefabs and instantiate the corresponding one.
            for (int i = 0; i < _ARTrackedImagePrefabs.Length; i++)
            {
                // If the name of the tracked image matches the name of the
                // prefab, and the prefab has not been instantiated yet.
                if (
                    name == _ARTrackedImagePrefabs[i].name
                    && !_ARTrackedImagePrefabInstances.ContainsKey(name)
                )
                {
                    // Instantiate the prefab.
                    GameObject prefabInstance = Instantiate(
                        _ARTrackedImagePrefabs[i],
                        trackedImage.transform.position,
                        trackedImage.transform.rotation
                    );

                    // Set the instantiated prefab's parent to the tracked image.
                    prefabInstance.transform.parent = trackedImage.transform;

                    // Add the instantiated prefab to the dictionary.
                    _ARTrackedImagePrefabInstances.Add(name, prefabInstance);
                }
            }
        }

        // For each instantiated prefab, set active if the tracked image is tracked.
        foreach (var trackedImage in eventArgs.updated)
        {
            _ARTrackedImagePrefabInstances[trackedImage.referenceImage.name]
                .SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        // If the tracked image is removed, destroy the corresponding instantiated prefab.
        foreach (var trackedImage in eventArgs.removed)
        {
            // Destroy the instantiated prefab.
            Destroy(_ARTrackedImagePrefabInstances[trackedImage.referenceImage.name]);

            // Remove the instantiated prefab from the dictionary.
            _ARTrackedImagePrefabInstances.Remove(trackedImage.referenceImage.name);
        }
    }
}
