using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    public GameObject selectedObject;

    Material[] matList;
    Color[] defaultEmissionColors;

    public float translateSpeed = .1f;
    public float highlightIntensity = 0.5f;
    public Color highlightColor = Color.yellow;

    //bounds information
    public float minX, maxX, minY, maxY, minZ, maxZ;
	
	// Update is called once per frame
	void Update () {
        if(selectedObject != null){
            Highlight();
        }
	}

    /// <summary>
    /// Highlight currently selected object
    /// </summary>
    void Highlight(){
        //if first frame selecting
        if(matList == null){
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            matList = renderer.materials;

            // save default emission colors so that the selected object can return to its default values after being unselected
            defaultEmissionColors = new Color[matList.Length];

            for (int i = 0; i < matList.Length; i++){
                defaultEmissionColors[i] = matList[i].GetColor("_EmissionColor");
            }
        }
        //loop through each material and change them accordingly
        float emission = Mathf.PingPong(Time.time, highlightIntensity);
        Color baseColor = highlightColor; //Replace this with whatever you want for your base color at emission level '1'

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        foreach(Material mat in matList){
            mat.SetColor("_EmissionColor", finalColor);
        }
    }
    /// <summary>
    /// Change the selected object's materials back to default values
    /// </summary>
    public void HighlightToOrignalMaterial(){
        //change back to original materil
        for (int i = 0; i < defaultEmissionColors.Length; i++)
        {
            Color currColor = defaultEmissionColors[i];
            matList[i].SetColor("_EmissionColor", currColor);
        }
        // empty the lists
        matList = null;
        defaultEmissionColors = null;
    }

    /// <summary>
    /// rotates an object about the y axis
    /// </summary>
    /// <param name="direction">"left" or "right"</param>
    public void Rotate (string direction){
        if(direction == "left"){
            selectedObject.transform.Rotate(0f, -10f, 0f);
        }
        else if(direction == "right"){
            selectedObject.transform.Rotate(0f, 10f, 0f);
        }
    }

    /// <summary>
    /// Scales an object
    /// </summary>
    /// <param name="scaleDirection">"up" or "down"</param>
    public void Scale (string scaleDirection){
        if(scaleDirection == "up"){
            selectedObject.transform.localScale *= 1.05f;
        }
        else if (scaleDirection == "down")
        {
            selectedObject.transform.localScale *= .95f;
        }
    }

    /// <summary>
    /// Moves object in the x Direction
    /// </summary>
    /// <param name="direction"></param>
    public void MoveX(string direction){
        if (direction == "+")
        {
            if(CheckBounds("posX")){
                selectedObject.transform.Translate(Vector3.right * translateSpeed);
            }
        }
        else
        {
            if(CheckBounds("negX")){
                selectedObject.transform.Translate(Vector3.left * translateSpeed);
            }
        }
    }

    /// <summary>
    /// moves object in the y direction
    /// </summary>
    /// <param name="direction"></param>
    public void MoveY(string direction)
    {
        if (direction == "+")
        {
            if (CheckBounds("posY"))
            {
                selectedObject.transform.Translate(Vector3.up * translateSpeed);
            }
        }
        else
        {
            if(CheckBounds("negY")){
                selectedObject.transform.Translate(Vector3.down * translateSpeed);
            }
        }
    }

    /// <summary>
    /// moves object in the z direction
    /// </summary>
    /// <param name="direction"></param>
    public void MoveZ(string direction)
    {
        if (direction == "+")
        {
            if(CheckBounds("posZ")){
                selectedObject.transform.Translate(Vector3.forward * translateSpeed);
            }
        }
        else
        {
            if(CheckBounds("negZ")){
                selectedObject.transform.Translate(Vector3.back * translateSpeed);
            }
        }
    }

    /// <summary>
    /// removes the current object from the scene.
    /// </summary>
    public void RemoveObject(){
        Destroy(selectedObject);
    }

    //TODO: Check bounds only works if object is at default rotation. If object changes rotation the bounds checker no longer works.

    /// <summary>
    /// checks object bounds to see if they are within the room bounds
    /// </summary>
    /// <returns><c>true</c>, if within bounds, <c>false</c> otherwise.</returns>
    /// <param name="directionToCheck">Direction to check.</param>
    bool CheckBounds(string directionToCheck){
        Collider selectedObjectCollider = selectedObject.GetComponent<Collider>();

        if(directionToCheck == "posX"){
            if (selectedObjectCollider.bounds.max.x < maxX)
            {
                return true;
            }
            else{
                return false;
            }
        }
        else if (directionToCheck == "negX")
        {
            if (selectedObjectCollider.bounds.min.x > minX)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (directionToCheck == "posY")
        {
            if (selectedObjectCollider.bounds.max.y < maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (directionToCheck == "negY")
        {
            if (selectedObjectCollider.bounds.min.y > minY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (directionToCheck == "posZ")
        {
            if (selectedObjectCollider.bounds.max.z < maxZ)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (directionToCheck == "negZ")
        {
            if (selectedObjectCollider.bounds.min.z > minZ)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else{
            Debug.LogWarning("Did not input correct parameter into the CheckBounds method");
            return false;
        }
    }
}
