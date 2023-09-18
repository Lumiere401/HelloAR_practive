using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class ObjectPlacing : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake() {
        planeManager = GetComponent<ARPlaneManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void OnEnable() {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;

    }

    private void FingerDown(EnhancedTouch.Finger finger){
        if (finger.index != 0) return;

        if (raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)){
            foreach(ARRaycastHit hit in hits){
                Pose pose = hit.pose;
                GameObject obj = Instantiate(prefab, pose.position, pose.rotation);
                obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
    }
}
