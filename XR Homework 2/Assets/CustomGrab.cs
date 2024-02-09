using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    public InputActionReference action2;
    bool grabbing = false;
    public Vector3 pos;
    public Quaternion rot;
    public int multiplier = 1;

    private void Start()
    {
        action.action.Enable();
        action2.action.Enable();

        // Find the other hand
        foreach(CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        action2.action.performed += (ctx) => {
            if(multiplier == 1) {multiplier = 2;}
            else {multiplier = 1;}
        };

        grabbing = action.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;

            if (grabbedObject)
            {
                grabbedObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(rot);
                Vector3 deltaPos = transform.position - pos;
                Quaternion rot2 = deltaRot * grabbedObject.rotation;
                grabbedObject.rotation = rot2;
                if(multiplier == 1) {
                    grabbedObject.position += deltaPos + ((deltaRot * (grabbedObject.position - transform.position)) - (grabbedObject.position - transform.position));
                } else {
                    grabbedObject.position += deltaPos + ((deltaRot * (grabbedObject.position - transform.position)) - (grabbedObject.position - transform.position)) * 2;
                }
                //grabbedObject.position = grabbedObject.position + transform.position - pos;
            }
        }
        // If let go of button, release object
        else if (grabbedObject) {
            grabbedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
            grabbedObject = null;
        }

        // Should save the current position and rotation here
        pos = transform.position;
        rot = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}
