using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

namespace BNG {

    /// <summary>
    /// This class can be used to map finger rotations from one model to another
    /// </summary>
    public class FingerMapping : MonoBehaviour {

        public IK ik;
        [Range(0f, 1f)] public float weight = 1f;

        [System.Serializable]
        public enum Mode {
            FromToRotation,
            MatchRotation
        }

        public Mode mode = Mode.FromToRotation;

        public Finger[] fingers;

        [Header("Shown for Debug : ")]
        public bool ShowGizmos = true;

        void Start() {

            if(ik == null) {
                ik = GetComponentInParent<IK>();
            }
            // Register to get a call from VRIK every time after it updates
            ik.GetIKSolver().OnPostUpdate += AfterVRIK;
        }

        void AfterVRIK() {
            if (weight <= 0f) {
                return;
            }

            if(mode == Mode.FromToRotation) {
                for (int x = 0; x < fingers.Length; x++) {
                    Finger finger = fingers[x];

                    if(finger == null) {
                        continue;
                    }

                    for (int i = 0; i < finger.avatarBones.Length - 1; i++) {
                        // Get the Quaternion to rotate the current finger bone towards the next target bone position.
                        Quaternion f = Quaternion.FromToRotation(finger.avatarBones[i + 1].position - finger.avatarBones[i].position, finger.targetBones[i + 1].position - finger.avatarBones[i].position);

                        // Weight blending
                        if (weight < 1f) {
                            f = Quaternion.Slerp(Quaternion.identity, f, weight);
                        }

                        // Rotate this finger bone
                        finger.avatarBones[i].rotation = f * finger.avatarBones[i].rotation;
                    }
                }
            }
            else if (mode == Mode.MatchRotation) {
                for (int x = 0; x < fingers.Length; x++) {
                    Finger finger = fingers[x];
                    bool orientationMatch = finger.avatarForwardAxis == finger.targetForwardAxis && finger.avatarUpAxis == finger.targetUpAxis;

                    for (int i = 0; i < finger.avatarBones.Length; i++) {
                        Quaternion r = orientationMatch ? finger.targetBones[i].rotation : RootMotion.QuaTools.MatchRotation(finger.targetBones[i].rotation, finger.targetForwardAxis, finger.targetUpAxis, finger.avatarForwardAxis, finger.avatarUpAxis);

                        if (weight < 1f) {
                            r = Quaternion.Slerp(finger.avatarBones[i].rotation, r, weight);
                        }

                        finger.avatarBones[i].rotation = r;
                    }
                }
            }
        }

        void OnDrawGizmos() {
            if (!ShowGizmos || fingers == null) {
                return;
            }

            for (int x = 0; x < fingers.Length; x++) {
                Finger finger = fingers[x];

                for (int i = 0; i < finger.targetBones.Length; i++) {

                    // Target Bones
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(finger.targetBones[i].position, 0.003f);

                    if(i < finger.targetBones.Length - 1) {
                        Gizmos.DrawLine(finger.targetBones[i].position, finger.targetBones[i + 1].position);
                    }
                }

                for (int i = 0; i < finger.avatarBones.Length; i++) {
                    
                    // Reference may have been removed from inspector
                    if(finger.avatarBones[i] == null) {
                        continue;
                    }

                    // Avatar Bones
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(finger.avatarBones[i].position, 0.003f);

                    if (i < finger.avatarBones.Length - 1) {
                        Gizmos.DrawLine(finger.avatarBones[i].position, finger.avatarBones[i + 1].position);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Finger {
        public Transform[] avatarBones = new Transform[0];
        public Transform[] targetBones = new Transform[0];

        public Vector3 avatarForwardAxis = Vector3.forward;
        public Vector3 avatarUpAxis = Vector3.up;
        public Vector3 targetForwardAxis = Vector3.forward;
        public Vector3 targetUpAxis = Vector3.up;
    }
}

