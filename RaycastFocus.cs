using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFocus : FocusObject, IInteractable
{ 
    /// <summary>
    /// class for focusing object based on RayCast interaction.
    /// Player must be tagged with default tag "Player"
    /// player's first child must be MainCamera
    /// player' second child must be empty object with transform position in camera position.
    /// Script must be attached to the object being focused.
    /// object's first child must be empty object with transform facing the zoom view.
    /// </summary>
    public void Activate() //activated by player input 'E'
    {
        isFocusing = true; // starts to move the camera position
        previousRotation = playerCamera.rotation; //stores current rotation of the camera to previousRotation.
        player.GetComponent<RayCastInteractable>().enabled = false; //disables Raycast.
        controller.enabled = false; //disables FPS controller.
    }
}
