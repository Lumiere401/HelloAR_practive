using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class ObjectPlacing : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Camera arCamera;
    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Transform highlight;
    private Vector2 touchPosition;

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

    // private void Update() {
    //     if(Input.touchCount > 0){
    //         Touch touch = Input.GetTouch(0);
    //         touchPosition = touch.position;
    //         if (touch.phase == TouchPhase.Began){
    //             Ray ray = arCamera.ScreenPointToRay(touch.position);
    //             RaycastHit hitObject;
    //             if (Physics.Raycast(ray, out hitObject)){
    //                 highlight = hitObject.transform;
    //                 if (highlight.CompareTag("selectable")){
    //                     Outline outline = highlight.AddComponent<Outline>();
    //                     outline.enabled = true;
    //                     highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
    //                     highlight.gameObject.GetComponent<Outline>().OutlineWidth = 5.0f;
    //                 }

    //             }
    //         }
    //     }
    // }
}
