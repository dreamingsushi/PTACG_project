using System.Collections;
using UnityEngine;

public class IceRockSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectToDrop;
    public Transform spawnArea;
    public float dropInterval = 2f;
    public Vector3 spawnSize = new Vector3(4f, 4f, 4f);

    [Header("Drop Settings")]
    public float dropForce = 5f;
    public Vector3 forceDirection = Vector3.down;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true) // Loop forever
        {
            SpawnObject();
            yield return new WaitForSeconds(dropInterval);
        }
    }

    void SpawnObject()
    {
        Vector3 localSpawnPos = new Vector3(
            Random.Range(-spawnSize.x / 2, spawnSize.x / 2),
            Random.Range(-spawnSize.y / 2, spawnSize.y / 2),
            Random.Range(-spawnSize.z / 2, spawnSize.z / 2)
        );

        Vector3 worldSpawnPos = spawnArea.TransformPoint(localSpawnPos);
        GameObject obj = Instantiate(objectToDrop, worldSpawnPos, Quaternion.identity);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 worldForce = spawnArea.TransformDirection(forceDirection.normalized) * dropForce;
            rb.velocity = worldForce;
        }
    }
}
