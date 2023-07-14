using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{

    public string[] lines;

    bool canActivate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && Input.GetButtonDown("Interact") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines);

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = true;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;
        }

    }
}
