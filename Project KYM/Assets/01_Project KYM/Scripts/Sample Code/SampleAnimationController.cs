using UnityEditor.Build;
using UnityEngine;

public class SampleAnimationController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 1f;
    public bool crouch = false;

    public SampleDropItemSensor dropItemSensor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        dropItemSensor = GetComponent<SampleDropItemSensor>();
    }

    private void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 inputMove = new Vector2(horizontal, vertical);

        // Toggle Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl)) { crouch = !crouch; }

        // Running
        bool running = Input.GetKey(KeyCode.LeftShift) && inputMove.magnitude > 0.1f;
        moveSpeed = crouch ? 0.5f : (running ? 3f : 1f);

        // Move the character
        transform.position += new Vector3(inputMove.x, 0, inputMove.y) * moveSpeed * Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 1번키
        {
            animator.SetTrigger("Action Trigger");
            animator.SetInteger("Action Index", 1);
            dropItemSensor.OverlapItemDestroy();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("Action Trigger");
            animator.SetInteger("Action Index", 2);
        }

        animator.SetFloat("Magnitude", inputMove.magnitude);
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Running", running ? 1f : 0f);
        animator.SetFloat("Crouch", crouch ? 1f : 0f);
    }
}
