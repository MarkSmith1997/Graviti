using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    public float speed;
    public float normalSpeed;
    public float shootSpeed;
    public float jumpAmount;
    public float verticalLookRotation;
    public float lookSensitivity;
    public bool grounded = true;
    public Rigidbody rb;
    public GameObject myCamera;
    public bool jump = false;
    public bool ampJump = false;
    public bool actMenu = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        normalSpeed = speed;
        shootSpeed = 0.25f * speed;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButton("Shift"))
        {
            ampJump = true;
        }
        else
        {
            ampJump = false;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            actMenu = Util.Toggle(actMenu);
        }
        if (!actMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (actMenu)
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }

    void FixedUpdate() {


        if (actMenu)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 velHoriz = transform.right * x;
        Vector3 velVirt = transform.forward * z;
        Vector3 velocity = (velHoriz + velVirt).normalized * speed;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);
        verticalLookRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
        myCamera.transform.localEulerAngles = Vector3.right * verticalLookRotation;


        jumpAmount = 9 * (rb.mass * GetComponent<PlayerMotor>().dominantBody.GetComponent<Rigidbody>().mass) / Mathf.Pow((GetComponent<PlayerMotor>().dominantBody.transform.position - transform.position).magnitude, 2);
        if (ampJump)
        {
            jumpAmount = 45 * (rb.mass * GetComponent<PlayerMotor>().dominantBody.GetComponent<Rigidbody>().mass) / Mathf.Pow((GetComponent<PlayerMotor>().dominantBody.transform.position - transform.position).magnitude, 2);
        }
        if (jump & grounded)
        {
            rb.AddForce(transform.up * jumpAmount);
            grounded = false;
            jump = false;
        }
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }
}
