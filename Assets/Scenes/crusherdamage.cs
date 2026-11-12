
using UnityEngine;

public class CrusherDamage : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockbackForce = 5f;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Crusher collision with: " + collision.gameObject.name);

        Health health =
            collision.gameObject.GetComponentInParent<Health>();

        if (health == null)
        {
            Debug.Log("Health component not found!");
            return;
        }

        bool damaged = health.TakeDamage(damage);

        if (damaged)
        {
            PlayerController player =
                collision.gameObject.GetComponentInParent<PlayerController>();

            if (player != null)
            {
                player.Knockback(Vector3.up, knockbackForce);
            }

            Debug.Log("Crusher damaged player!");
        }
    }
}
