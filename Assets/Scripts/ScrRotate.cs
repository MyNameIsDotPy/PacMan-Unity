using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrRotate : MonoBehaviour
{
    
    public float rotationSpeed = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0f, 0f, 90f), Time.deltaTime * rotationSpeed);
    }
}
