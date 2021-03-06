﻿using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public abstract class FocusObject : MonoBehaviour
{
    //adjust the movement speed of camera
    [SerializeField]
    private float Movespeed;

    //change these to true to get the camera moving
    private bool isFocusing;
    private bool isReturning;

    //fields for holding game objects attached to player
    private InteractWithSelectedObject raycast;
    private FirstPersonController controller;
    private Transform playerCamera;
    //location information
    private Quaternion previousRotation; // used to rotate back to previous position
    private Vector3 previousPosition;
    private Transform zoomLoc; // location of the focus point

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player"); //gets the playerObject by Unity Default Tag "Player".
        controller = player.GetComponent<FirstPersonController>(); //gets the fps controller from the player object.
        raycast = player.GetComponentInChildren<InteractWithSelectedObject>();
        zoomLoc = transform.GetChild(0); //gets the focus point transform by accesing the first child of the object.
        playerCamera = player.transform.GetChild(0); //gets the player camera by accesing the first child of the player.
        previousPosition = new Vector3();
        previousRotation = new Quaternion(); //initializes the previous rotation to a heap data. Must be set on derived class when activated.
    }

    /// <summary>
    /// focus() should always be called on Update().
    /// </summary>
    protected virtual void Update()
    {
        focus();
    }

    ///<summary>
    /// Must be called on update.
    ///</summary>
    protected void focus()
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
                (playerCamera.position, previousPosition, Movespeed * Time.deltaTime);
            playerCamera.rotation = Quaternion.Lerp
                (playerCamera.rotation, previousRotation, Movespeed * Time.deltaTime);
            if (isOnPlayer())// when finished returning
            {
                //enable controller and raycast again.
                controller.enabled = true;
                raycast.enabled = true;
                isReturning = false;
            }
        }
    }

    /// <summary>
    /// start moving to the focus point
    /// can be called in derived class to move to specified position
    /// </summary>
    protected void moveToFocus()
    {
        if (isReturning || isFocusing || isOnFocus())
            return;
        previousPosition = playerCamera.transform.position; //stores current position of the camera to previousRotation.
        previousRotation = playerCamera.rotation; //stores current rotation of the camera to previousRotation.
        raycast.enabled = false; //disables Raycast.
        controller.enabled = false; //disables FPS controller.
        isFocusing = true; // starts to move the camera position
    }
    ///<summary> 
    /// start returning the camera to previous position.
    ///</summary>
    protected void returnToPosition()
    {
        if (!isOnFocus())
            return; // exit if its not on focus
        // if on focus,
        isReturning = true; // returns the camera
    }

    //boolean for checking whether camera has finished traveling.
    private bool isOnFocus()
    {
        if (playerCamera.position == zoomLoc.position &&
            Quaternion.Angle(playerCamera.rotation, zoomLoc.rotation) < 1) // fix: cant(playerCamera.rotation == zoomLoc.rotation)
            return true;
        return false;
    }
    /// <summary>
    /// property for checking if camera's on focus
    /// </summary>
    public bool IsOnFocus { get => isOnFocus(); }

    //boolean for checking whether camera is back to the position
    private bool isOnPlayer()// Only true the moment camera goes back to the player.
    {
        if (playerCamera.position == previousPosition &&
            playerCamera.rotation == previousRotation)
            return true;
        return false;
    }
    /// <summary>
    /// property for checking if camera's back to player
    /// </summary>
    public bool IsOnPlayer { get => isOnPlayer(); }
}
