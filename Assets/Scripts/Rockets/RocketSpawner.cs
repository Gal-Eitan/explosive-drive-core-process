
using UnityEngine;
using System.Collections;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] private HomingRocket rocketPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnInterval = 4f;
    [SerializeField] private float maxSpawnInterval = 7f;

    private void Start()
{
    Debug.Log("RocketSpawner started!");
    StartCoroutine(SpawnRockets());
}

private IEnumerator SpawnRockets()
{
    while (true)
    {
        float delay = Random.Range(
            minSpawnInterval,
            maxSpawnInterval
        );

        Debug.Log($"Next rocket in {delay} seconds");

        yield return new WaitForSeconds(delay);

        if (player == null || rocketPrefab == null)
        {
            Debug.LogError(
                $"Spawner missing references! " +
                $"Player: {player}, Rocket: {rocketPrefab}"
            );
            continue;
        }

        HomingRocket rocket = Instantiate(
            rocketPrefab,
            player.position,
            Quaternion.identity
        );

        Debug.Log("Rocket instantiated!");

        rocket.SetTarget(player);
        Debug.Log($"Spawner assigned {player.name} to {rocket.name}");
    }
}
}
