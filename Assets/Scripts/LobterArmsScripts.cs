using UnityEngine;

public class LobterArmsScripts : MonoBehaviour
{
    public LobterScript lobter;
    public float initShakeAmt;
    private float shakeAmt;
    private BoxCollider2D box;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        shakeAmt = initShakeAmt;

        Collider2D[] cols = Physics2D.OverlapBoxAll(box.bounds.center, box.bounds.size, 0f);
        foreach (Collider2D col in cols)
        {
            if (col.TryGetComponent(out Creature creature))
            {
                if (creature.creatureType == Creature.CreatureType.Jeller)
                    lobter.stomach += 10;
                else if(creature.creatureType == Creature.CreatureType.Fish)
                    lobter.stomach += 25;

                if (creature.creatureType != Creature.CreatureType.Lobter)
                    creature.Die();
            }
        }
    }
    private void Update()
    {
        transform.localPosition = Random.onUnitCircle * initShakeAmt;
        shakeAmt = Mathf.Lerp(shakeAmt, 0f, 10f * Time.deltaTime);
    }
}
