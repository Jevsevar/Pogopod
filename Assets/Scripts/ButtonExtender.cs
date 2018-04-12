using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonExtender : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public UnityEvent OnClickHold;

    bool buttonDown;
	
    void Start(){
        buttonDown = false;
    }

	// Update is called once per frame
	void Update () {
        if(buttonDown){
            OnClickHold.Invoke();
            Debug.Log("button down");
        }
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("on pointer down");
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("on pointer up");
        buttonDown = false;
    }
}
