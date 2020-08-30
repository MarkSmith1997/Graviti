using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMotor: MonoBehaviour {

    public float minDistance;
    public float force;
    public List<float> forces;
    public GameObject dominantBody;
    public float lerpTime;

    private void Start()
    {
        dominantBody = Attractor.Attractors[0].gameObject;
    }

    void Update()
    {
        for (int i = 0; i < Attractor.Attractors.Count; i++)
        {
            if (Attractor.Attractors[i].gameObject != gameObject)
            {
                forces.Add((GetComponent<Rigidbody>().mass * Attractor.Attractors[i].gameObject.GetComponent<Rigidbody>().mass) / Mathf.Pow((Attractor.Attractors[i].transform.position - transform.position).magnitude, 2));
            }
        }
        float maxValue = forces.Max();
        int maxIndex = forces.ToArray().ToList().IndexOf(maxValue);
        dominantBody = Attractor.Attractors[maxIndex].gameObject;
        forces.Clear();


        Vector3 tarDirection = (transform.position - dominantBody.transform.position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, tarDirection) * transform.rotation, lerpTime);
    }
}
