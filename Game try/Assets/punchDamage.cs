using UnityEngine;

public class punchDamage : MonoBehaviour
{
    public int damage = 10;               // same as your ranged
    public Vector2 ownerPosition;         // attack source

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HPMechanics hp = collision.GetComponent<HPMechanics>();

            if (hp != null)
            {
                hp.takeDamangeRanged(ownerPosition);
            }
        }
    }
}
