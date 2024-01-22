using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG {
    public class ArmSwingLocomotion : MonoBehaviour {

        // Use this to move the character
        [Tooltip("This is used to move the character. Will be disabled on Start")]
        public CharacterController characterController;

        [Tooltip("Speed = Hand Controller Speed * SpeedModifier")]
        public float SpeedModifier = 5f;

        [Tooltip("Direction to move player. Will default to this transform if none provided")]
        public Transform ForwardDirection;

        [Tooltip("Minimum Velocity to allow movement. Increase this to avoid unwanted movement at slow speeds")]
        public float MinInput = 0.1f;

        public bool MustBeHoldingLeftTrigger = true;
        public bool MustBeHoldingRightTrigger = true;

        public bool MustBeHoldingLeftGrip = false;
        public bool MustBeHoldingRightGrip = false;

        public float VelocitySum {
            get {
                return leftVelocity + rightVelocity;
            }
        }
        float leftVelocity;
        float rightVelocity;

        void Start() {
            // Manually assign if not set in the inspector
            if(characterController == null) {
                characterController = GetComponentInChildren<CharacterController>();
            }

            // Default to our own transform if none specified
            if(ForwardDirection == null) {
                ForwardDirection = transform;
            }
        }

        // Update is called once per frame
        void Update() {
            UpdateVelocities();
            UpdateMovement();
        }

        public virtual void UpdateMovement() {
            if (characterController && VelocitySum > MinInput) {
                characterController.Move(ForwardDirection.forward * VelocitySum * SpeedModifier * Time.deltaTime);
            }
        }

        public void UpdateVelocities() {
            if (LeftInputReady()) {
                leftVelocity = InputBridge.Instance.GetControllerVelocity(ControllerHand.Left).magnitude;
            }
            else {
                leftVelocity = 0;
            }

            if (RightInputReady()) {
                rightVelocity = InputBridge.Instance.GetControllerVelocity(ControllerHand.Right).magnitude;
            }
            else {
                rightVelocity = 0;
            }
        }

        public virtual bool LeftInputReady() {

            // Check for negatives since we default to true
            if(MustBeHoldingLeftGrip && InputBridge.Instance.LeftGrip < 0.1f) {
                return false;
            }

            if (MustBeHoldingLeftTrigger && InputBridge.Instance.LeftTrigger < 0.1f) {
                return false;
            }

            // Default to ready
            return true;
        }

        public virtual bool RightInputReady() {

            // Check for negatives since we default to true
            if (MustBeHoldingRightGrip && InputBridge.Instance.RightGrip < 0.1f) {
                return false;
            }

            if (MustBeHoldingRightTrigger && InputBridge.Instance.RightTrigger < 0.1f) {
                return false;
            }

            // Default to ready
            return true;
        }
    }
}