using UnityEngine;

public class LobterScript : Creature
{
    [Header("The big lob")]
    public GameObject arms;
    public float atkInterval;
    public float moveSpd;

    private void Start()
    {
        transform.localScale = Vector3.one * 0.1f;
    }
    protected override void Update()
    {
        base.Update();
        transform.localScale = Vector3.one * Mathf.Lerp(0.1f, 0.25f, age / maxAge);
        if (!arms.activeInHierarchy)
            rb.linearVelocityX = moveSpd;
        else
            rb.linearVelocityX = 0f;

        if (!IsInvoking(nameof(Atk)))
            Invoke(nameof(Atk), atkInterval * Random.Range(1f, 2f));
    }

    private void Atk()
    {
        arms.SetActive(true);

        Invoke(nameof(StopAtk), 0.5f);
    }
    private void StopAtk() 
    {
        arms.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpd *= -1;
    }
    public override void Die()
    {
        base.Die();

        int spawnCount = Random.Range(0, 3);
        GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(gameManager.spawner.objs[2], transform.position, Quaternion.identity);
        }
    }
}
