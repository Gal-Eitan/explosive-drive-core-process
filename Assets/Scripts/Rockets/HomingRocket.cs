
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RocketExplosion))]
public class HomingRocket : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;
    [SerializeField] private TargetIndicator targetIndicatorPrefab;

    [Header("Timing")]
    [SerializeField] private float trackingTime = 3f;
    [SerializeField] private float lockDelay = 1f;
    [SerializeField] private float fallDuration = 0.7f;

    [Header("Visuals")]
    [SerializeField] private Transform rocketVisual;
    [SerializeField] private float fallStartHeight = 12f;
    [SerializeField] private GameObject explosionEffectPrefab;

    private RocketExplosion explosion;
    private TargetIndicator indicator;

    private bool initialized = false;

    private void Awake()
    {
        explosion = GetComponent<RocketExplosion>();
    }



    private IEnumerator RocketSequence()
    {
        indicator = Instantiate(
            targetIndicatorPrefab,
            player.position,
            Quaternion.identity
        );

        indicator.SetTracking();

        // Phase 1: Follow the player.
        float timer = 0f;

        while (timer < trackingTime)
        {
            if (player == null)
            {
                Cleanup();
                yield break;
            }

            Vector3 target = player.position;
            target.z = indicator.transform.position.z;

            indicator.transform.position = target;

            timer += Time.deltaTime;
            yield return null;
        }

        // Phase 2: Lock onto the last position.
        Vector2 lockedPosition = player.position;

        indicator.transform.position = new Vector3(
            lockedPosition.x,
            lockedPosition.y,
            indicator.transform.position.z
        );

        indicator.SetLocked();

        yield return new WaitForSeconds(lockDelay);

        // Phase 3: Visually drop the rocket.
        Vector3 impactPosition = new Vector3(
            lockedPosition.x,
            lockedPosition.y,
            transform.position.z
        );

        transform.position = impactPosition;

        Vector3 start = impactPosition +
            Vector3.up * fallStartHeight;

        rocketVisual.gameObject.SetActive(true);
        rocketVisual.position = start;

        timer = 0f;

        while (timer < fallDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / fallDuration);

            // Accelerate toward the ground.
            float easedT = t * t;

            rocketVisual.position = Vector3.Lerp(
                start,
                impactPosition,
                easedT
            );

            yield return null;
        }

        rocketVisual.position = impactPosition;

        // Phase 4: Explode.
        explosion.Explode(lockedPosition);

        if (explosionEffectPrefab != null)
        {
            Instantiate(
                explosionEffectPrefab,
                impactPosition,
                Quaternion.identity
            );
        }

        Cleanup();
    }

    private void Cleanup()
    {
        if (indicator != null)
            Destroy(indicator.gameObject);

        Destroy(gameObject);
    }
    public void SetTarget(Transform target)
    {
        if (initialized)
            return;

        player = target;

        if (player == null ||
            targetIndicatorPrefab == null ||
            rocketVisual == null)
        {
            Debug.LogError(
                $"HomingRocket {name} missing references! " +
                $"Player: {player}, " +
                $"Indicator: {targetIndicatorPrefab}, " +
                $"Visual: {rocketVisual}"
            );

            Destroy(gameObject);
            return;
        }

        initialized = true;

        Debug.Log($"HomingRocket initialized with player: {player.name}");

        StartCoroutine(RocketSequence());
    }
}
