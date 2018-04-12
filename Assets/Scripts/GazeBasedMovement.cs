using UnityEngine;
/// <summary>
/// unused
/// </summary>
public class GazeBasedMovement : MonoBehaviour {

    public ObjectController objectController;

    public SteamVR_TrackedController tController;
    public Transform rightControllerTransform;

    public GameObject spherePrefab;

    bool triggerDown;
    Vector3 lastHitPoint;


    void Update()
    {
        if (tController.triggerPressed)
        {
            //if (Input.GetMouseButtonDown(0))
            //{

            triggerDown = true;
            RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            var layer1 = 8;
            var layermask = ~((1 << layer1)); // NOT "Furniture" layer

            if (Physics.Raycast(rightControllerTransform.position, rightControllerTransform.forward, out hit, 15f, layermask))
            {
                lastHitPoint = hit.point;
                Debug.Log("hitting " + hit.transform.name);
            }
            Vector3 targetPosition = new Vector3(lastHitPoint.x,
                                objectController.selectedObject.transform.position.y,
                                lastHitPoint.z);

            objectController.selectedObject.transform.position = targetPosition;

        }
        else if (triggerDown)
        {
            triggerDown = false;
            //Instantiate(spherePrefab, lastHitPoint, Quaternion.identity);

            if(objectController.selectedObject != null)
            {
                Vector3 targetPosition = new Vector3(lastHitPoint.x,
                                objectController.selectedObject.transform.position.y,
                                lastHitPoint.z);

                objectController.selectedObject.transform.position = targetPosition;
            }
            
        }
        
    }
}
