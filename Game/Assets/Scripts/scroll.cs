using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Renderer))]
public class scroll : MonoBehaviour
{

    
    public Transform CameraPosition;

    void Start()
    {
        
    }

 
    void Update()
    {
        this.transform.position = new Vector3(CameraPosition.position.x, CameraPosition.position.y, transform.position.z);
        //Vector2 offset = new Vector2(Time.time * speed, 0);
        //Wenns geht GetComponent() nie in update benutzen
        //GetComponent<Renderer>().material.mainTextureOffset = offset;
        //m_renderer.material.mainTextureOffset = offset;
    }
}
