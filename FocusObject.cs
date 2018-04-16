using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public abstract class FocusObject : MonoBehaviour
{
    //adjust the movement speed of camera
    [SerializeField]
    protected float Movespeed;

    //change these to true to get the camera moving
    protected bool isFocusing;
    protected bool isReturning;

    //fields for holding game objects
    protected GameObject player;
    protected FirstPersonController controller;
    protected Transform playerCamera;
    //location information
    protected Quaternion previousRotation; // used to rotate back to previous position
    private Transform zoomLoc; // location of the focus point
    private Transform playerCameraLoc; //used to move back to previous position

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); //gets the playerObject by Unity Default Tag "Player".
        controller = player.GetComponent<FirstPersonController>(); //gets the fps controller from the player object.
        zoomLoc = transform.GetChild(0); //gets the focus point transform by accesing the first child of the object.
        playerCamera = player.transform.GetChild(0); //gets the player camera by accesing the first child of the player.
        playerCameraLoc = player.transform.GetChild(1); // gets the position to return to by accesing the 2nd child of the player.
        previousRotation = new Quaternion(); //initializes the previous rotation to a heap data. Must be set on derived class when activated.
    }

    ///<summary>
    ///update is checking the player's status at live.
    ///It does not take in input untill player actually zooms in.
    ///Thus it usually wont be necessary to override or change this function.
    ///However, if necessary, inherit the class and override the function, then call the base.Update() after your adjustment.
    ///</summary>
    protected virtual void Update()
    {
        returnToPosition();
        focus();
    }

    ///<summary>
    ///this function only activates when isFocusing or isReturning is changed by the player input or zoomCheck().
    ///only needs to be called in update.
    ///</summary>
    private void focus()
    {
        if (isFocusing) //if focusing in to object
        {
            // slowly move the camera position toward the destination
            playerCamera.position = Vector3.MoveTowards
                (playerCamera.position, zoomLoc.position, Movespeed * Time.deltaTime);
            playerCamera.rotation = Quaternion.Lerp
                (playerCamera.rotation, zoomLoc.rotation, Movespeed * Time.deltaTime);
            if (isOnFocus())
                isFocusing = false;
        }
        if (isReturning) // if retruning to position
        {
            //slowly move back to the player position
            playerCamera.position = Vector3.MoveTowards
                (playerCamera.position, playerCameraLoc.position, Movespeed * Time.deltaTime);
            playerCamera.rotation = Quaternion.Lerp
                (playerCamera.rotation, previousRotation, Movespeed * Time.deltaTime);
            if (isOnPlayer())// when finished returning
            {
                //enable controller and raycast again.
                controller.enabled = true;
                player.GetComponent<RayCastInteractable>().enabled = true;
                isReturning = false;
            }
        }
    }

    private void returnToPosition()
    {
        ///<summary> 
        ///Handles input for returning the camera to previous position.
        ///only used and called in update.
        ///</summary>
        if (isOnFocus() && Input.GetKeyDown(KeyCode.E))
            isReturning = true; // returns the camera
    }

    ///<summary>
    ///boolean for checking wether camera has finished traveling.
    ///either one is true and one is false, both cannot equal.
    ///</summary>
    protected bool isOnFocus()
    {
        if (playerCamera.position == zoomLoc.position &&
            Quaternion.Angle(playerCamera.rotation, zoomLoc.rotation) < 1) // fix: cant(playerCamera.rotation == zoomLoc.rotation)
            return true;
        return false;
    }
    protected bool isOnPlayer()// Only true the moment camera goes back to the player.
    {
        if (playerCamera.position == playerCameraLoc.position &&
            playerCamera.rotation == previousRotation)
            return true;
        return false;
    }
}
