using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    public float moveSpeed = 14f;

    public float minX = -8f;
    public float maxX = 8f;
    public float minZ = -4f;
    public float maxZ = 4f;

    public bool useWASD = true;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (useWASD)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveZ = Input.GetAxisRaw("Vertical");
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;
        }

        movement = new Vector3(moveX, 0f, moveZ).normalized;
    }

    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        rb.MovePosition(newPosition);
    }
}