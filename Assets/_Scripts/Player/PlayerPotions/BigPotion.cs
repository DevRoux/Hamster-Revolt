using UnityEngine;

public class BigPotion : MonoBehaviour
{
    public float scalingFactor = 1.5f;
    public float duration = 4f;
    public float destroyDelay = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterController player = other.GetComponent<CharacterController>();
        if (player != null)
        {
            ScaleCharacter(player.gameObject);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, destroyDelay);
    }

    private void ScaleCharacter(GameObject character)
    {
        character.transform.localScale *= scalingFactor;
        Invoke(nameof(RevertScaling), duration);
    }

    private void RevertScaling()
    {
        CharacterController[] players = FindObjectsOfType<CharacterController>();

        foreach (CharacterController player in players)
        {
            player.transform.localScale /= scalingFactor;
        }
    }
}
