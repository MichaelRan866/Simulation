using UnityEngine;

public class JellerFish : Creature
{
    public GameObject egg;
    public float jumpSpd;
    public float jumpInterval;
    private int jumpCount;
    private int jumpsPerEgg;
    private Vector3 targetScale;
    private Vector3 defaultScale;

    private void Start()
    {
        float nextJumpDelay = Random.Range(jumpInterval * 0.75f, jumpInterval * 1.25f);
        Invoke(nameof(JumpSquash), nextJumpDelay - 0.1f);
        Invoke(nameof(Jump), nextJumpDelay);

        defaultScale = transform.localScale;
        transform.localScale = defaultScale / 2.5f;

        metabolism = Random.Range(5, 15);
        jumpsPerEgg = Random.Range(15, 25);
    }
    protected override void Update()
    {
        base.Update();

        targetScale = Vector3.Lerp(defaultScale / 2.5f, defaultScale, Mathf.Clamp01(age / maxAge));
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 5f * Time.deltaTime);
        if (rb.linearVelocityY > 0)
            rb.linearVelocityY = Mathf.Lerp(rb.linearVelocityY, 0f, 5f * Time.deltaTime);
        rb.linearVelocityX = Mathf.Lerp(rb.linearVelocityX, 0f, 1f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, -rb.linearVelocityX * 5f);

        if (GameManager.hour <= 12 && !IsInvoking(nameof(PhotoSynthesize)))
            Invoke(nameof(PhotoSynthesize), 0.75f);
    }
    private void PhotoSynthesize() { stomach += 25; }
    private void JumpSquash() { transform.localScale = new Vector3(targetScale.x * 1.25f, targetScale.y / 1.25f, targetScale.z); }
    private void Jump()
    {
        transform.localScale = new Vector3(targetScale.x / 1.25f, targetScale.y * 1.25f, targetScale.z);
        rb.linearVelocity = new Vector2(Random.Range(-1f, 1f) * 0.5f, 1f).normalized * jumpSpd;
        jumpCount++;
        if (jumpCount >= jumpsPerEgg)
        {
            Instantiate(egg, transform.position, Quaternion.identity);
            jumpCount = 0;
        }

        float nextJumpDelay = Random.Range(jumpInterval * 0.75f, jumpInterval * 1.25f);
        Invoke(nameof(JumpSquash), nextJumpDelay - 0.1f);
        Invoke(nameof(Jump), nextJumpDelay);
    }
}
