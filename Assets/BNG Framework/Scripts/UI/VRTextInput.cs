using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class VRTextInput : MonoBehaviour {

        UnityEngine.UI.InputField thisInputField;

        public bool AttachToVRKeyboard = true;

        public bool ActivateKeyboardOnSelect = true;
        public bool DeactivateKeyboardOnDeselect = false;

        public VRKeyboard AttachedKeyboard;

        bool isFocused, wasFocused = false;

        float lastActivatedTime = 0;

        void Awake() {
            thisInputField = GetComponent<UnityEngine.UI.InputField>();
            
            if(thisInputField && AttachedKeyboard != null) {
                AttachedKeyboard.AttachToInputField(thisInputField);
            }
        }

        void Update() {

            isFocused = thisInputField != null && thisInputField.isFocused;

            // Check if our input field is now focused
            if(isFocused == true && wasFocused == false) {
                OnInputSelect();
            }
            else if (isFocused == false && wasFocused == true) {
                OnInputDeselect();
            }

            wasFocused = isFocused;
        }

        public void OnInputSelect() {

            // Enable keyboard if disabled
            if (ActivateKeyboardOnSelect && AttachedKeyboard != null && !AttachedKeyboard.gameObject.activeInHierarchy) {
                AttachedKeyboard.gameObject.SetActive(true);

                AttachedKeyboard.AttachToInputField(thisInputField);

                lastActivatedTime = Time.time;
            }
        }

        public void OnInputDeselect() {
            // Deslect if valid keyboard is found, and did not recently activate
            if (DeactivateKeyboardOnDeselect && AttachedKeyboard != null && AttachedKeyboard.gameObject.activeInHierarchy && Time.time - lastActivatedTime >= 0.1f) {
                AttachedKeyboard.gameObject.SetActive(false);
            }
        }

        // Assign the AttachedKeyboard variable when adding the component to a GameObject for the first time
        void Reset() {
            var keyboard = GameObject.FindObjectOfType<VRKeyboard>();
            if(keyboard) {
                AttachedKeyboard = keyboard;
                Debug.Log("Found and attached Keyboard to " + AttachedKeyboard.transform.name);
            }
        }
    }
}


