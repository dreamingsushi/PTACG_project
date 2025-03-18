using UnityEngine;

public class PoisonBottle : MonoBehaviour
{
    public GameObject poisonVFX;
    public float vfxLifetime = 3f;
    public float rotationSpeed = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag != "Player")
        {
            GameObject vfxInstance = Instantiate(poisonVFX, transform.position, Quaternion.identity);
            Destroy(vfxInstance, vfxLifetime);
            Destroy(gameObject);
        }
    }
}
