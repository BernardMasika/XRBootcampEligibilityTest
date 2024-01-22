using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class FollowTransform : MonoBehaviour {

        public Transform FollowTarget;
        public bool MatchRotation = true;

        public float YOffset = 0;

        void Update() {
            if(FollowTarget) {
                transform.position = FollowTarget.position;

                if(YOffset != 0) {
                    transform.position += new Vector3(0, YOffset, 0);
                }

                if(MatchRotation) {
                    transform.rotation = FollowTarget.rotation;
                }
            }
        }
    }
}