using UnityEngine;

public class JellerEggScript : MonoBehaviour
{
    private int spawnCount;
    public Vector2Int spawnRange;
    public float driftSpd;
    public float driftFactor;
    public GameObject jeller;
    private Rigidbody2D rb;

    private void Start()
    {
        spawnCount = Random.Range(spawnRange.x, spawnRange.y);
        driftSpd /= spawnCount;

        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.linearVelocityX = Mathf.Sin(driftSpd * Time.time) * driftFactor;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Creature jellerSpawn = Instantiate(jeller, transform.position + (Vector3)Random.insideUnitCircle * 0.1f, Quaternion.identity).GetComponent<Creature>();
                jellerSpawn.rb.AddForce(Random.insideUnitCircle * 100f);
            }
            Destroy(gameObject);
        }
        else
        {
            driftFactor *= -1f;
        }
    }
}
