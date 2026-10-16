using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum CreatureType 
    {
        Jeller, 
        Fish, 
        Lobter
    }
    public CreatureType creatureType;
    public Vector2 lifespanRange;
    public Vector2 ageSpdRange;
    [HideInInspector] public int stomach = 100;
    [HideInInspector] public int metabolism = 5;
    [HideInInspector] public int HP = 100;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public float age;
    protected float maxAge;
    private float ageSpd;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        age = 0f;
        ageSpd = Random.Range(ageSpdRange.x, ageSpdRange.y);
        maxAge = Random.Range(lifespanRange.x, lifespanRange.y);
        Invoke(nameof(DrainStomach), 0.5f);

        GameManager.creatures[creatureType].Add(this);
    }
    private void OnDestroy()
    {
        GameManager.creatures[creatureType].Remove(this);
    }
    protected virtual void Update() 
    {
        age += Time.deltaTime * ageSpd;
        if (age >= maxAge)
            Die();
    }
    private void DrainStomach()
    {
        stomach -= metabolism;
        Invoke(nameof(DrainStomach), 0.5f);

        if (stomach <= 0 && !IsInvoking(nameof(Starve)))
            Starve();
    }
    private void Starve() 
    { 
        HP -= 10;
        Invoke(nameof(Starve), 1f);

        if (HP <= 0)
            Die();
    }
    public virtual void Die() 
    {
        Destroy(gameObject);
    }
}
