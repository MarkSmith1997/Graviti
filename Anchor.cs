using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour {

    public Vector3 rotationRate;


    private void Update()
    {
        transform.Rotate(rotationRate * Time.deltaTime);
    }
}
