using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour {

    public static List<Attractor> Attractors;
    public Rigidbody rb;
    public float density;
    public float startSpeed;
    public float inertia = 1f; 
    private float volume;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * startSpeed;
    }

    void FixedUpdate()
    {
        transform.localScale = new Vector3(Mathf.Pow((3 * rb.mass / (4 * Mathf.PI * density)), 1 / 3f), Mathf.Pow((3 * rb.mass / (4 * Mathf.PI * density)), 1 / 3f), Mathf.Pow((3 * rb.mass / (4 * Mathf.PI * density)), 1 / 3f));

        foreach (Attractor attractor in Attractors)
        {
            if (attractor != this)
            {
                Attract(attractor);
            }
            
        }
    }

    void OnEnable()
    {
        if(Attractors == null)
        {
            Attractors = new List<Attractor>();
        }

        Attractors.Add(this);

    }

    void OnDisable()
    {
        Attractors.Remove(this);
    }

    void Attract(Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rbToAttract.position - rb.position;
        float distance = direction.magnitude;

        if(distance == 0f)
        {
            return;
        }

        float forceMagnitude = (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rb.AddForce(force * inertia);
    }

}
