using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class BNGIKPlayerScale : MonoBehaviour {

        public VRIK ik;
        public float scaleMlp = 1f;

        public ControllerBinding ScalePlayerInput = ControllerBinding.AButtonDown;

        void LateUpdate() {
            if (InputBridge.Instance.GetControllerBindingValue(ScalePlayerInput)) {
                // Compare the height of the head target to the height of the head bone, multiply scale by that value.
                float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / (ik.references.head.position.y - ik.references.root.position.y);
                ik.references.root.localScale *= sizeF * scaleMlp;
            }
        }
    }
}

