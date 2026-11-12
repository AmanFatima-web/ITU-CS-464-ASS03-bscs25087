using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float bounceForce = 15f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(
                    Vector3.up * bounceForce,
                    ForceMode.Impulse
                );
            }
        }
    }
}
