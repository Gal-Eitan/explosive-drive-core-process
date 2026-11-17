
using UnityEngine;
using System.Collections.Generic;

public class RocketExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float explosionForce = 20f;
    [SerializeField] private float spinOffset = 0.4f;
    [SerializeField] private int explosionDamage = 1;

    [Header("Physics Layers")]
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private LayerMask coverLayers;

    public void Explode(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            position,
            explosionRadius,
            affectedLayers
        );

        Debug.Log($"Explosion detected {hits.Length} colliders!");

        HashSet<Rigidbody2D> processed = new();
        HashSet<HealthManager> damagedCars = new();

        foreach (Collider2D hit in hits)
        {
            Debug.Log($"Explosion hit: {hit.name}");

            Rigidbody2D rb = hit.attachedRigidbody;

            if (rb == null || !processed.Add(rb))
                continue;

            if (rb.bodyType != RigidbodyType2D.Dynamic)
                continue;

            Vector2 target = rb.worldCenterOfMass;
            Vector2 difference = target - position;

            float distance = difference.magnitude;

            // Check whether a wall blocks the explosion.
            if (IsBehindCover(position, target))
                continue;

            float falloff = 1f -
                Mathf.Clamp01(distance / explosionRadius);

            Vector2 direction = distance > 0.001f
                ? difference / distance
                : Random.insideUnitCircle.normalized;

            if (direction.sqrMagnitude < 0.001f)
                direction = Vector2.up;

            // Apply knockback and rotation.
            Vector2 forcePoint =
                target + Vector2.Perpendicular(direction) * spinOffset;

            rb.AddForceAtPosition(
                direction * explosionForce * falloff,
                forcePoint,
                ForceMode2D.Impulse
            );

            HealthManager health = rb.GetComponentInParent<HealthManager>();

            if (health != null)
            {
                health.ChangeHealth(-explosionDamage);

                Debug.Log(
                $"Rocket damaged {rb.name}. Remaining HP: {health.CurrentHealth}"
                );
            }
            else
            {
                Debug.LogWarning($"No HealthManager found on {rb.name}");
            }
        }
    }

    private bool IsBehindCover(
        Vector2 explosionPosition,
        Vector2 targetPosition)
    {
        Vector2 difference =
            targetPosition - explosionPosition;

        float distance = difference.magnitude;

        if (distance < 0.001f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            explosionPosition,
            difference / distance,
            distance,
            coverLayers
        );

        Debug.DrawRay(
            explosionPosition,
            difference,
            hit.collider != null ? Color.green : Color.red,
            2f
        );

        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }

}

