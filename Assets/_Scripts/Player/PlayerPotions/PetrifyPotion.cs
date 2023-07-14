using UnityEngine;
using UnityEngine.SceneManagement;

public class PetrifyPotion : MonoBehaviour
{
    public string sceneName; // Name of the scene to load

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterController player = other.GetComponent<CharacterController>();
        if (player != null)
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneName);
        }

        // Destroy the potion
        Destroy(gameObject);
    }
}
