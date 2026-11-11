
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveForce = 10f;

    Rigidbody rb;
    Vector3 input;

    [Header("Speed Power-Up")]
    float speedMultiplier = 1f;
    Coroutine boost;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        input = Vector3.ClampMagnitude(
            new Vector3(h, 0f, v), 1f
        );
    }

    void FixedUpdate()
    {
        rb.AddForce(
            input * moveForce * speedMultiplier,
            ForceMode.Force
        );
    }

    public void BoostSpeed(float multiplier, float seconds)
    {
        if (boost != null)
            StopCoroutine(boost);

        Vector3 velocity = rb.linearVelocity;
        Vector3 horizontal = new Vector3(
            velocity.x, 0f, velocity.z
        );

        float ratio = multiplier / speedMultiplier;

        horizontal *= ratio;

        rb.linearVelocity = new Vector3(
            horizontal.x,
            velocity.y,
            horizontal.z
        );

        speedMultiplier = multiplier;

        boost = StartCoroutine(ResetBoost(seconds));
    }

    IEnumerator ResetBoost(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Vector3 velocity = rb.linearVelocity;

        Vector3 horizontal = new Vector3(
            velocity.x, 0f, velocity.z
        );

        horizontal /= speedMultiplier;

        rb.linearVelocity = new Vector3(
            horizontal.x,
            velocity.y,
            horizontal.z
        );

        speedMultiplier = 1f;
        boost = null;

        Debug.Log("Speed boost finished!");
    }

    public void Knockback(Vector3 direction, float force)
    {
        rb.AddForce(
            direction.normalized * force,
            ForceMode.Impulse
        );
    }
}
