using UnityEditor.Build;
using UnityEngine;

public class SampleAnimationController : MonoBehaviour
{
    public Animator animatior;
    public float moveSpeed = 1f;
    public bool crouch = false;

    private void Awake()
    {
        animatior = GetComponent<Animator>();
    }

    private void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 inputMove = new Vector2(horizontal, vertical);

        animatior.SetFloat("Magnitude", inputMove.magnitude);
        animatior.SetFloat("Horizontal", horizontal);
        animatior.SetFloat("Vertical", vertical);

        // Toggle Crouch
        crouch = Input.GetKey(KeyCode.LeftControl) ? !crouch : crouch;
        animatior.SetFloat("Crouch", crouch ? 1f : 0f);

        // Running
        bool running = Input.GetKey(KeyCode.LeftShift) && inputMove.magnitude > 0.1f;
        moveSpeed = running ? 3f : 1f;
        animatior.SetFloat("Running", running ? 1f : 0f);

        // Move the character
        transform.position += new Vector3(inputMove.x, 0, inputMove.y) * moveSpeed * Time.deltaTime;


        // Action
        bool action = Input.GetMouseButtonDown(0); // Left mouse button
        animatior.SetFloat("Action", action ? 1f : 0f);
    }

}
