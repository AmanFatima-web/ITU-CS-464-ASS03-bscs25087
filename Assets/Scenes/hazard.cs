
using System.Collections;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 6f;

    [SerializeField] float restTime = 0f;
    [SerializeField] float warnTime = 0.7f;
    [SerializeField] float strikeTime = 0.4f;

    [SerializeField] GameObject warning;

    IEnumerator Start()
    {
        if (restTime <= 0f)
            yield break;

        Collider zone = GetComponent<Collider>();

        if (zone == null || warning == null)
        {
            Debug.LogError("Hazard needs a Collider and Warning object!");
            yield break;
        }

        while (true)
        {
            zone.enabled = false;

            yield return new WaitForSeconds(restTime);

            warning.SetActive(true);

            yield return new WaitForSeconds(warnTime);

            warning.SetActive(false);
            zone.enabled = true;

            yield return new WaitForSeconds(strikeTime);
        }
    }


    void OnTriggerStay(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health == null)
            return;

        if (!health.TakeDamage(damage))
            return;

        Rigidbody body = health.GetComponent<Rigidbody>();

        if (body == null)
            body = other.attachedRigidbody;

        if (body == null)
            return;

        Vector3 direction = body.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            body.AddForce(
                direction.normalized * knockback,
                ForceMode.Impulse
            );
        }
    }

}
