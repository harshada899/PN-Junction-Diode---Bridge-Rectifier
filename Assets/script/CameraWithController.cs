using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CameraWithController : MonoBehaviour
{
    public float speed = 5f;
    public float lookSensitivity = 2f;
    public float gravity = -9.81f;

    float rotX = 0f, rotY = 0f;
    Vector3 velocity;
    CharacterController ctrl;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ctrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        LookAround();
        Move();
    }

    void LookAround()
    {
        rotX += Input.GetAxis("Mouse X") * lookSensitivity;
        rotY -= Input.GetAxis("Mouse Y") * lookSensitivity;
        rotY = Mathf.Clamp(rotY, -90f, 90f);
        transform.rotation = Quaternion.Euler(rotY, rotX, 0f);
    }

    void Move()
    {
        if (ctrl.isGrounded && velocity.y < 0) velocity.y = -2f;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal") +
                      transform.forward * Input.GetAxis("Vertical");
        ctrl.Move(dir * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        ctrl.Move(velocity * Time.deltaTime);
    }
}
