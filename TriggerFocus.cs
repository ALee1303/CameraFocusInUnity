using UnityEngine;

/// <summary>
/// class for focusing object based on trigger.
/// Player must be tagged with default tag "Player"
/// player's first child must be MainCamera
/// Script must be attached to the trigger.
/// trigger's first child must be empty object with transform facing the zoom view.
/// </summary>
public class TriggerFocus : FocusObject
{
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            returnToPosition();
        base.Update();
    }

    private void OnTriggerStay(Collider other) //if player is in the trigger
    {
        if (other.tag != "Player")
            return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            moveToFocus();
        }
    }
}
