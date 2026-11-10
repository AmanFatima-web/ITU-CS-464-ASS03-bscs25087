
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] float protection = 1f;

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health != null)
        {
            if (health.Current >= health.Max)
            {
                return;
            }

            health.RestoreOne(protection);

            Destroy(gameObject);
        }
    }
}
