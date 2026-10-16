using UnityEngine;

public class GodrayScript : MonoBehaviour
{
    public float moveSpd;
    public float maxX;
    public SpriteRenderer[] sprites;
    public float sizeOscillation = 0.25f;
    private int dir;

    private void Start()
    {
        dir = Random.Range(0, 10) < 5 ? -1 : 1;
        moveSpd *= Random.Range(0.5f, 1.5f);
        transform.position = new Vector3(Random.Range(-1f, 1f) * maxX, transform.position.y, 0f);
    }

    private void Update()
    {
        transform.position += Vector3.right * moveSpd * dir * Time.deltaTime;
        transform.localScale = new Vector3(1f + Mathf.Sin(Time.time) * sizeOscillation, transform.localScale.y, 1f);
        foreach (SpriteRenderer s in sprites)
            s.color = new Color(s.color.r, s.color.g, s.color.b, GameManager.sunIntensity * (5f / 255f));

        if (Mathf.Abs(transform.position.x) >= maxX)
            dir *= -1;
    }
}
