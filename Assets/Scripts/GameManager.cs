using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //script responsible for handling all object changes (rotation, translation, etc.)
    public ObjectController objectController; 

    //placeable prefabs list (prefabs need to be tagged "ScaleableObject", be on the "Furniture" layer, and have collider
    public List<GameObject> furniturePrefabs; 

    //used to know when the user pressed the trigger
    public SteamVR_TrackedController tController;

    //tracks the right hand position
    public Transform rightControllerTransform;

    //used to know the first frame the player presses the button
    private bool triggerDown;

    //track the point where the right hand raycast hits the floor
    private Vector3 lastHitPoint;

    //layer and layermask used to have raycast only detect "floor" layer
    private int layer1;
    private LayerMask layermask;

    private void Start()
    {
        layer1 = 9;
        layermask = ((1 << layer1)); // ONLY "Floor" layer
    }


    /// <summary>
    /// Moves the selected furniture object using a raycast from the right hand. 
    /// Uses a raycast that ignores all layers except the "Floor" layer.
    /// NOTE: this method does not move an object in the y direction, so this method is not very effective for placing paintings up/down on a wall
    /// </summary>
    void MoveFurnitureWithRaycast()
    {
        //raycast from the right hand that can only hit the floor
        RaycastHit hit;
        if (Physics.Raycast(rightControllerTransform.position, rightControllerTransform.forward, out hit, 15f, layermask))
        {
            //keep track of last hit point
            lastHitPoint = hit.point;
        }

        //position to move the selected object to
        Vector3 targetPosition = new Vector3(lastHitPoint.x,
                            objectController.selectedObject.transform.position.y,
                            lastHitPoint.z);

        //move the selected object to the target position
        objectController.selectedObject.transform.position = targetPosition;
    }

    /// <summary>
    /// Used to select an object, select a new object, deselect an object, and ignore moving an object if selecting UI
    /// </summary>
    /// <param name="hit">raycast hit information</param>
    void HandleSelectingObject(RaycastHit hit)
    {
        if (hit.transform.CompareTag("ScaleableObject"))
        {
            //if there is not currently selectedOjbect then set one
            if (objectController.selectedObject == null)
            {
                objectController.selectedObject = hit.transform.gameObject;
            }
            //change from one selectedObject to another
            else
            {
                objectController.HighlightToOrignalMaterial();
                objectController.selectedObject = hit.transform.gameObject;
            }
        }
        // ignore UI raycasts
        else if (hit.transform.CompareTag("UI"))
        {
            return;
        }
        //set the selectedObject to null (used when the user clicks "off" a furniture item
        else
        {
            if (objectController.selectedObject != null)
            {
                objectController.HighlightToOrignalMaterial();
                objectController.selectedObject = null;
            }
        }
    }

    void Update()
    {
        //if right controller trigger is down
        if (tController.triggerPressed)
        {
            //if trigger is down for the first time check if we have a currently selected object    
            RaycastHit hit;
            if (Physics.Raycast(rightControllerTransform.position, rightControllerTransform.forward, out hit, 15f))
            {
                //ignore other operations if we are hitting a UI menu
                if (hit.transform.CompareTag("UI"))
                {
                    return;
                }
                // if this is the first frame that the trigger is down
                if (!triggerDown)
                {
                    triggerDown = true;
                    HandleSelectingObject(hit);
                    /*
                    if (hit.transform.CompareTag("ScaleableObject"))
                    {
                        //if there is not currently selectedOjbect then set one
                        if (objectController.selectedObject == null)
                        {
                            objectController.selectedObject = hit.transform.gameObject;
                        }
                        //change from one selectedObject to another
                        else
                        {
                            objectController.HighlightToOrignalMaterial();
                            objectController.selectedObject = hit.transform.gameObject;
                        }
                    }
                    // ignore UI raycasts
                    else if (hit.transform.CompareTag("UI"))
                    {
                        return;
                    }
                    //set the selectedObject to null (used when the user clicks "off" a furniture item
                    else
                    {
                        if (objectController.selectedObject != null)
                        {
                            objectController.HighlightToOrignalMaterial();
                            objectController.selectedObject = null;
                        }
                    }
                    */
                }
            }

            if (objectController.selectedObject != null && triggerDown)
            {
                MoveFurnitureWithRaycast();
                /*
                if (Physics.Raycast(rightControllerTransform.position, rightControllerTransform.forward, out hit, 15f, layermask))
                {
                    //keep track of last hit point
                    lastHitPoint = hit.point;
                    Debug.Log("hitting " + hit.transform.name);
                }

                Vector3 targetPosition = new Vector3(lastHitPoint.x,
                                    objectController.selectedObject.transform.position.y,
                                    lastHitPoint.z);
                objectController.selectedObject.transform.position = targetPosition;
                */
            }
        }
        //trigger up
        else
        {
            //reset the bool
            triggerDown = false;
        }
    }
    
    /// <summary>
    /// Adds a furniture item from the list at the origin
    /// </summary>
    /// <param name="furniturePrefabsListIndex">index of the object to instantiate</param>
    public void AddFurniture(int furniturePrefabsListIndex){
        Instantiate(furniturePrefabs[furniturePrefabsListIndex],Vector3.zero,Quaternion.identity);
    }
}
