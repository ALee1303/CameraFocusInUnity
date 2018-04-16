using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for focusing object based on trigger.
/// Player must be tagged with default tag "Player"
/// player's first child must be MainCamera
/// player' second child must be empty object with transform position in camera position.
/// Script must be attached to the trigger.
/// trigger's first child must be empty object with transform facing the zoom view.
/// </summary>
public class TriggerFocus : FocusObject
{
    private void OnTriggerStay(Collider other) //if player is in the trigger
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && !isOnFocus() && !isReturning && !isFocusing) // boolean check for checking initial input and disabling input while moving
            {
                isFocusing = true; // start the movement of camera
                previousRotation = playerCamera.rotation; // stores the current location.
                controller.enabled = false; //disable FPS controller
            }
        }
    }
}
