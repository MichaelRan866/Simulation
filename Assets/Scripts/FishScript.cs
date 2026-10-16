using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FishScript : Creature
{
    [Header("Fish")]
    public Transform sprite;
    private Vector3 defaultSpriteScale;
    public Vector2 startLookForMateTime;
    public float moveSpd;
    public float moveInterval;
    [HideInInspector] public bool lookingForMate;

    private void Start()
    {
        metabolism = Random.Range(5, 10);
        moveInterval *= Random.Range(0.75f, 1.25f);
        moveSpd *= Random.Range(0.75f, 2f);
        Invoke(nameof(Swim), Random.Range(moveInterval * 0.5f, moveInterval * 1.5f));
        Invoke(nameof(LookForMate), Random.Range(startLookForMateTime.x, startLookForMateTime.y));
        defaultSpriteScale = sprite.localScale;
        transform.localScale = Vector3.one;
    }
    float targetScale;
    protected override void Update()
    {
        base.Update();

        targetScale = Mathf.Lerp(1f, 2f, age / maxAge);
        sprite.localScale = Vector3.Lerp(sprite.localScale, defaultSpriteScale, 5f * Time.deltaTime);
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 5f * Time.deltaTime);
        if (rb.linearVelocity.magnitude > 5f)
        {
            float angle = Vector2.SignedAngle(Vector2.right, rb.linearVelocity.normalized);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            transform.localScale = new Vector3(1f, Mathf.Abs(angle) >= 90f ? -1f : 1f, 1f);
        }
        transform.localScale = transform.localScale.normalized * targetScale;
    }
    private void Swim()
    {
        Vector3 dir = Random.onUnitCircle;
        if (stomach < 50)
        {
            List<Creature> jellers = GameManager.creatures[CreatureType.Jeller];
            float minDist = 999;
            Creature closestJeller = null;
            foreach (Creature jeller in jellers)
            {
                float dist = Vector2.Distance(transform.position, jeller.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestJeller = jeller;
                }
            }

            if(closestJeller != null)
                dir = (closestJeller.transform.position - transform.position).normalized * 2f;
        }
        else 
        {
            if (lookingForMate)
            {
                List<Creature> fishes = GameManager.creatures[CreatureType.Fish];
                float minDist = 999;
                Creature closestFish = null;
                foreach (Creature fish in fishes)
                {
                    float dist = Vector2.Distance(transform.position, fish.transform.position);
                    if (dist < minDist && ((FishScript)fish).lookingForMate && fish.gameObject != gameObject)
                    {
                        minDist = dist;
                        closestFish = fish;
                    }
                }

                if(closestFish != null)
                    dir = (closestFish.transform.position - transform.position).normalized * 3f;
            }
        }

        rb.linearVelocity = dir * moveSpd;
        sprite.localScale = new Vector3(defaultSpriteScale.x * 1.5f, defaultSpriteScale.y / 1.5f, defaultSpriteScale.z);

        Invoke(nameof(Swim), Random.Range(moveInterval * 0.5f, moveInterval * 1.5f));
    }
    private void LookForMate() { lookingForMate = true; }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.linearVelocity.magnitude > 5) 
        {
            if (stomach < 50)
            {
                if (collision.transform.TryGetComponent(out JellerFish jeller))
                {
                    stomach += 25;
                    jeller.Die();
                }
            }
            if (lookingForMate && collision.transform.TryGetComponent(out FishScript fish))
            {
                fish.lookingForMate = false;
                lookingForMate = false;

                Invoke(nameof(LookForMate), Random.Range(startLookForMateTime.x, startLookForMateTime.y));
                fish.Invoke(nameof(LookForMate), Random.Range(fish.startLookForMateTime.x, fish.startLookForMateTime.y));

                int spawnCount = Random.Range(1, 3);
                GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
                for (int i = 0; i < spawnCount; i++) 
                {
                    Instantiate(gameManager.spawner.objs[1], collision.contacts[0].point, Quaternion.identity);
                }
            }
        }
    }
}
