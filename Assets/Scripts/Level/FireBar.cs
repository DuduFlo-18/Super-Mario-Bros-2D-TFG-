using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBar : MonoBehaviour
{
    public float rotationspeed = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationspeed * Time.deltaTime);
    }
}
