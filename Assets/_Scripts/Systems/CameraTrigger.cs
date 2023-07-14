using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public float cameraMoveAmount = 16.75f;
    public Camera mainCamera;
    private bool hasPlayerEntered = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPlayerEntered)
        {

            mainCamera.transform.Translate(new Vector3(cameraMoveAmount, 0f, 0f));

            // Set the flag to true to prevent the effect from happening again
            hasPlayerEntered = true;
            gameObject.SetActive(false);
        }
    }
}
