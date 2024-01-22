using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// This component is used to dynamically move a FinalIK's Arm Target
    /// </summary>
    public class IKHandTargetMover : MonoBehaviour {

        public Grabber grabber;
        public Transform HandTarget;

        public Vector3 rotationOffset = Vector3.zero;
        public Vector3 positionOffset = Vector3.zero;

        void Update() {
            // Hand Target Update
            if(grabber.HeldGrabbable != null) {
                Transform grabPoint = grabber.HeldGrabbable.GetGrabPoint();

                // Use positioned grab point if found
                if(grabber.HeldGrabbable.GrabMechanic == GrabType.Snap && grabPoint != null) {
                    transform.parent = grabPoint;
                    transform.localPosition = positionOffset;
                    transform.localEulerAngles = rotationOffset;
                }
                // Try finding an Activated Grab Point
                else if (grabber.HeldGrabbable.GrabMechanic == GrabType.Snap && grabber.HeldGrabbable.ActiveGrabPoint != null) {
                    transform.parent = grabber.HeldGrabbable.ActiveGrabPoint.transform;
                    transform.localPosition = positionOffset;
                    transform.localEulerAngles = rotationOffset;
                }
                else {
                    if(grabber.HandsGraphics  != null) {
                        transform.parent = grabber.HandsGraphics;
                    }
                }
            }
            else {
                transform.parent = HandTarget;
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
            }
        }
    }
}

