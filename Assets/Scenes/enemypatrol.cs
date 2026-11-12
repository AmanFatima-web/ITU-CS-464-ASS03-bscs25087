
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] points;

    [SerializeField] float speed = 2f;
    [SerializeField] float turnSpeed = 360f;

    int index = 1;
    int direction = 1;

    void Update()
    {
        if (points == null || points.Length < 2)
            return;

        if (index >= points.Length || index < 0)
            index = 0;

        Transform target = points[index];

        if (target == null)
            return;

        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y;

        Vector3 directionToTarget = targetPosition - transform.position;

        // Rotate enemy
        if (directionToTarget.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(directionToTarget);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotation,
                turnSpeed * Time.deltaTime
            );
        }

        // Move enemy
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // Change direction
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            if (index == points.Length - 1)
                direction = -1;

            else if (index == 0)
                direction = 1;

            index += direction;
        }
    }
}
