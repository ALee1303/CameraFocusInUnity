using UnityEngine;

/// <summary>
/// class for focusing object based on RayCast interaction.
/// Player must be tagged with default tag "Player"
/// player's first child must be MainCamera
/// Script must be attached to the object being focused.
/// object's first child must be empty object with transform representing the focus point.
/// </summary>
public class RaycastFocus : FocusObject, IInteractable
{

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            returnToPosition();
        base.Update();
    }

    public void Activate() //activated by player input 'E'
    {
        moveToFocus();
    }
}
