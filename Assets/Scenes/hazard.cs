
using System.Collections;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 6f;

    [Header("Big Spike Appearance")]
    [SerializeField] float spikeHeight = 2f;
    [SerializeField] float spikeWidth = 1.5f;
    [SerializeField] Material spikeMaterial;

    [Header("Timing")]
    [SerializeField] float restTime = 0f;
    [SerializeField] float warnTime = 0.7f;
    [SerializeField] float strikeTime = 0.4f;

    [SerializeField] GameObject warning;

    BoxCollider zone;
    bool isActive = true;

    void Start()
    {
        zone = GetComponent<BoxCollider>();

        if (zone == null)
        {
            Debug.LogError("Hazard needs a Box Collider!");
            return;
        }

        zone.isTrigger = true;

        CreateSpike();

        if (restTime > 0f)
        {
            StartCoroutine(SpikeRoutine());
        }
    }

    IEnumerator SpikeRoutine()
    {
        while (true)
        {
            isActive = false;
            zone.enabled = false;

            if (warning != null)
                warning.SetActive(true);

            yield return new WaitForSeconds(restTime);

            if (warning != null)
                warning.SetActive(false);

            zone.enabled = true;
            isActive = true;

            yield return new WaitForSeconds(strikeTime);
        }
    }

    void CreateSpike()
    {
        GameObject oldSpike = transform.Find("BigSpike")?.gameObject;

        if (oldSpike != null)
            Destroy(oldSpike);

        GameObject spike = new GameObject("BigSpike");

        spike.transform.SetParent(transform, false);

        spike.transform.localPosition = new Vector3(
            zone.center.x,
            zone.center.y + zone.size.y / 2f,
            zone.center.z
        );

        MeshFilter mf = spike.AddComponent<MeshFilter>();
        MeshRenderer mr = spike.AddComponent<MeshRenderer>();

        mf.mesh = MakeSpike(spikeWidth, spikeHeight);

        if (spikeMaterial != null)
        {
            mr.material = spikeMaterial;
        }
        else
        {
            Renderer original = GetComponent<Renderer>();

            if (original != null)
                mr.material = original.sharedMaterial;
        }
    }

    Mesh MakeSpike(float width, float height)
    {
        Mesh mesh = new Mesh();

        float half = width / 2f;

        Vector3[] vertices =
        {
            new Vector3(-half, 0, -half),
            new Vector3(half, 0, -half),
            new Vector3(half, 0, half),
            new Vector3(-half, 0, half),
            new Vector3(0, height, 0)
        };

        int[] triangles =
        {
            0, 2, 1,
            0, 3, 2,
            0, 1, 4,
            1, 2, 4,
            2, 3, 4,
            3, 0, 4
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    void OnTriggerStay(Collider other)
    {
        if (!isActive)
            return;

        Health health =
            other.GetComponentInParent<Health>();

        if (health == null)
            return;

        if (!health.TakeDamage(damage))
            return;

        Rigidbody body =
            health.GetComponent<Rigidbody>();

        if (body == null)
            body = other.attachedRigidbody;

        if (body != null)
        {
            Vector3 direction =
                body.position - transform.position;

            direction.y = 0.5f;

            body.AddForce(
                direction.normalized * knockback,
                ForceMode.Impulse
            );
        }

        Debug.Log("Big Spike hit Player!");
    }
}
