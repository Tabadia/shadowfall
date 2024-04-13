using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MenuAnimation : MonoBehaviour
{
    public TMP_Text consoleText;
    public TMP_Text textObject1;
    public TMP_Text textObject2;
    public TMP_Text textObject3;

    public float consoleCharPerSecond = 10.0f;
    public float textObjectDuration = 10.0f; // Delay between each letter for text objects

    public int maxLines = 10; // Maximum number of lines to display
    public float lineDelay = 0.5f; // Delay between each line


    public bool active = false; // Flag to start the menu animation
    public bool canActive = true;
    private bool menuAnimationFinished = false; // Flag to track when menu animation has finished

    private string textObject1OriginalText;
    private string textObject2OriginalText;
    private string textObject3OriginalText;
    private string consoleOriginalText;

    void Start()
    {
        // Save original text values and clear the text of the three additional TMP_Text objects
        textObject1OriginalText = textObject1.text;
        textObject2OriginalText = textObject2.text;
        textObject3OriginalText = textObject3.text;
        consoleOriginalText = consoleText.text;
        textObject1.text = "";
        textObject2.text = "";
        textObject3.text = "";
       consoleText.text = "";

        // Start the menu animation when active becomes true

    }

    private void Update()
    {

        if (active && canActive)
        {
            StartCoroutine("UpdateConsole");
            canActive = false;
        }

        if (menuAnimationFinished)
        {
            // Start the animation for other text objects once menu animation completes
            StartCoroutine("AnimateOtherTextObjects");
            menuAnimationFinished = false;
        }
    }


    IEnumerator UpdateConsole()
    {
        consoleText.text = "_";

        for (int i = 0; i < consoleOriginalText.Length;)
        {
            for (int j = 0; j < consoleCharPerSecond * Time.deltaTime; j++)
            {
                if (i < consoleOriginalText.Length)
                {
                    consoleText.text = consoleText.text.Remove(consoleText.text.Length - 1, 1);
                    consoleText.text += consoleOriginalText[i];
                    consoleText.text += "_";
                    i++;
                }

               
            }
            yield return null;

        }
        menuAnimationFinished = true;
      
    }

    


    IEnumerator AnimateOtherTextObjects()
    {
        // Start the animation for textObject1, textObject2, and textObject3 concurrently
        StartCoroutine(AnimateText(textObject1, textObject1OriginalText ));
        StartCoroutine(AnimateText(textObject2, textObject2OriginalText ));
        StartCoroutine(AnimateText(textObject3, textObject3OriginalText));

        // Wait for all animations to finish
        yield return null;
    }

    IEnumerator AnimateText(TMP_Text textObject, string originalText)
    {
        for (int i = 0; i < originalText.Length;)
        {
            for (int j = 0; j < (originalText.Length/textObjectDuration)* Time.deltaTime; j++)
            {
                if (i < originalText.Length)
                {
                    textObject.text += originalText[i];
                    i++;
                }


            }
            yield return null;

        }
    }


}
