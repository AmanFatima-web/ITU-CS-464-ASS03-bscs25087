
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Heart : MonoBehaviour
{
    [SerializeField] float protection = 1f;

    void Start()
    {
        // Get original pink material
        MeshRenderer original = GetComponent<MeshRenderer>();

        Material pinkMaterial = original != null
            ? original.sharedMaterial
            : null;

        // Hide original ball
        if (original != null)
            original.enabled = false;

        // Create proper heart shape
        CreateHeart(pinkMaterial);

        // Setup pickup trigger
        Collider pickupCollider = GetComponent<Collider>();
        pickupCollider.isTrigger = true;

        SphereCollider sphere = pickupCollider as SphereCollider;

        if (sphere != null)
            sphere.radius = 0.85f;

        // Add Rigidbody for trigger detection
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void CreateHeart(Material material)
    {
        GameObject heart = new GameObject("Heart Shape");

        heart.transform.SetParent(transform, false);
        heart.transform.localPosition = Vector3.zero;
        heart.transform.localRotation = Quaternion.identity;
        heart.transform.localScale = Vector3.one;

        Mesh mesh = new Mesh();
        mesh.name = "Heart Mesh";

        int points = 80;

        Vector3[] vertices = new Vector3[points * 2 + 2];

        // Create front and back heart outline
        for (int i = 0; i < points; i++)
        {
            float t = 2f * Mathf.PI * i / points;

            float x = 16f * Mathf.Pow(Mathf.Sin(t), 3);

            float y =
                13f * Mathf.Cos(t)
                - 5f * Mathf.Cos(2f * t)
                - 2f * Mathf.Cos(3f * t)
                - Mathf.Cos(4f * t);

            float heartX = x * 0.045f;
            float heartY = (y + 2.5f) * 0.045f;

            vertices[i] = new Vector3(
                heartX, heartY, 0.08f
            );

            vertices[points + i] = new Vector3(
                heartX, heartY, -0.08f
            );
        }

        int frontCenter = points * 2;
        int backCenter = points * 2 + 1;

        vertices[frontCenter] = new Vector3(0f, 0f, 0.08f);
        vertices[backCenter] = new Vector3(0f, 0f, -0.08f);

        int[] triangles = new int[points * 12];

        int index = 0;

        for (int i = 0; i < points; i++)
        {
            int next = (i + 1) % points;

            // Front surface
            triangles[index++] = frontCenter;
            triangles[index++] = next;
            triangles[index++] = i;

            // Back surface
            triangles[index++] = backCenter;
            triangles[index++] = points + i;
            triangles[index++] = points + next;

            // Side surface
            triangles[index++] = i;
            triangles[index++] = next;
            triangles[index++] = points + next;

            triangles[index++] = i;
            triangles[index++] = points + next;
            triangles[index++] = points + i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshFilter filter = heart.AddComponent<MeshFilter>();
        MeshRenderer renderer = heart.AddComponent<MeshRenderer>();

        filter.mesh = mesh;

        if (material != null)
        {
            renderer.sharedMaterial = material;
        }
        else
        {
            Material pink = new Material(
                Shader.Find("Standard")
            );

            pink.color = new Color(1f, 0.1f, 0.5f);
            renderer.sharedMaterial = pink;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health == null)
            return;

        // Don't collect if health is already full
        if (health.Current >= health.Max)
        {
            Debug.Log("Health is already full!");
            return;
        }

        // Add one health
        health.RestoreOne(protection);

        Debug.Log("Heart collected! Health restored.");

        Destroy(gameObject);
    }
}
