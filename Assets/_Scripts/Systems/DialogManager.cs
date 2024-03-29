using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;


    public string[] dialogLines;
    public int currentLine;

    public static DialogManager instance;

    private bool justStarted;

    void Start()
    {
        instance = this;
        // dialogText.text = dialogLines[currentLine];
    }

    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Interact"))
            {
                if (!justStarted)
                {
                    currentLine++;

                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                    else
                    {
                        dialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }

            }
        }

    }

    public void ShowDialog(string[] newlines)
    {
        dialogLines = newlines;

        currentLine = 0;

        dialogText.text = dialogLines[0];
        dialogBox.SetActive(true);

        justStarted = true;
    }

}
