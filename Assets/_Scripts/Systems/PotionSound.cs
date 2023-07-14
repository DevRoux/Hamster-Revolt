using UnityEngine;

public class PotionSound : MonoBehaviour
{
    public AudioClip collisionSound; // Sound to play on trigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            AudioSource.PlayClipAtPoint(collisionSound, transform.position);
        }
    }
}
