using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    public int healingAmount = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterController player = other.GetComponent<CharacterController>();
        if (player != null)
        {
            player.Heal(healingAmount);
        }

        Destroy(gameObject);
    }
}
