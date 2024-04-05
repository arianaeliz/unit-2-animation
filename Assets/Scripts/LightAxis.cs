using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAxis : MonoBehaviour
{

    //set the speed of rotation
    public float rotSpeed = 80;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 1 * rotSpeed * Time.deltaTime, 0);
    }
}
