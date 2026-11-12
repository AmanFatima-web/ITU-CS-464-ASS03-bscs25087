
using UnityEngine;

public class CrusherMovement : MonoBehaviour
{
    [Header("Crusher Movement")]
    [SerializeField] float height = 3f;
    [SerializeField] float speed = 1.5f;

    Rigidbody rb;
    Vector3 topPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        topPosition = transform.position;

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        float drop = Mathf.PingPong(
            Time.time * speed,
            height
        );

        Vector3 targetPosition =
            topPosition - Vector3.up * drop;

        rb.MovePosition(targetPosition);
    }
}
