using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;

    [SerializeField]
    private Transform playerPointer;

    [SerializeField]
    private float speed;
    private float finalSpeed, speedPower;

    [SerializeField]
    private Animator animator;

    bool movement;

    void Start()
    {
        joystick = GameObject.Find("Floating Joystick").GetComponent<Joystick>();
        //playerPointer = transform.Find("Sphere");
        animator = transform.Find("ybot").GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        speedPower = new Vector2(joystick.Horizontal, joystick.Vertical).magnitude;
        finalSpeed = speed * speedPower * speedPower;

        animator.SetFloat("Speed", speedPower);

        if (joystick.Horizontal > 0 || joystick.Horizontal < 0 || joystick.Vertical > 0 || joystick.Vertical < 0)
        {
            playerPointer.position = new Vector3(joystick.Horizontal + transform.position.x, playerPointer.position.y, joystick.Vertical + transform.position.z);

            transform.LookAt(new Vector3(playerPointer.position.x, 0, playerPointer.position.z));

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            transform.Translate(Vector3.forward * Time.deltaTime * finalSpeed);

            if (!animator.GetBool("Walking"))
            {
                animator.SetBool("Walking", true);
            }

            movement = true;
        }
        else if (movement)
        {
            animator.SetBool("Walking", false);

            movement = false;
        }

        // Reset position in case of falling
        if (transform.position.y <= -8)
        {
            transform.position = Vector3.zero;
        }
    }

    public void ToggleController(bool status)
    {
        joystick.Reset();
        animator.SetBool("Walking", false);
        speed = 0;
        joystick.gameObject.SetActive(status);
    }
}
