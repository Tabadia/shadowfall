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

    public float consoleLetterDelay = 0.1f; // Delay between each letter for consoleText
    public float textObjectLetterDelay = 0.1f; // Delay between each letter for text objects

    public int maxLines = 10; // Maximum number of lines to display
    public float lineDelay = 0.5f; // Delay between each line
    public float cursorBlinkRate = 0.5f; // Rate at which the cursor blinks

    private Queue<string> messageQueue = new Queue<string>(); // Queue to store messages
    private string currentMessage = ""; // Current message being displayed
    private bool cursorVisible = true; // Flag to control cursor visibility

    public bool active = false; // Flag to start the menu animation
    private bool menuAnimationFinished = false; // Flag to track when menu animation has finished

    private string textObject1OriginalText;
    private string textObject2OriginalText;
    private string textObject3OriginalText;

    void Start()
    {
        // Save original text values and clear the text of the three additional TMP_Text objects
        textObject1OriginalText = textObject1.text;
        textObject2OriginalText = textObject2.text;
        textObject3OriginalText = textObject3.text;

        textObject1.text = "";
        textObject2.text = "";
        textObject3.text = "";

        AddMessage(consoleText.text);
        consoleText.text = "";

        // Start the menu animation when active becomes true

    }

    private void Update()
    {

        if (active)
        {
            StartCoroutine("UpdateConsole");
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
        foreach (string message in messageQueue)
        {
            string[] parts = Regex.Split(message, @"(<.*?>)"); // Split the message using color tags as delimiters
            string typedMessage = "";

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].StartsWith("<") && parts[i].EndsWith(">"))
                {
                    // If the part is a color tag, just append it to the typed message
                    typedMessage += parts[i];
                }
                else
                {
                    // If the part is text, type it letter by letter
                    for (int j = 0; j < parts[i].Length; j++)
                    {
                        typedMessage += parts[i][j];
                        consoleText.text = typedMessage + (cursorVisible ? "_" : "");
                        yield return new WaitForSeconds(consoleLetterDelay);
                    }
                }

                // Check if the typed message ends with a newline character
                if (typedMessage.EndsWith("\n"))
                {
                    yield return new WaitForSeconds(lineDelay);
                }
            }

            // Display the complete typed message
            consoleText.text = typedMessage;
            yield return new WaitForSeconds(lineDelay); // Wait for lineDelay after displaying each message
        }

        // Set menuAnimationFinished flag to true when consoleText animation finishes
        menuAnimationFinished = true;
    }

    IEnumerator AnimateOtherTextObjects()
    {
        // Start the animation for textObject1, textObject2, and textObject3 concurrently
        StartCoroutine(AnimateText(textObject1, textObject1OriginalText, textObjectLetterDelay));
        StartCoroutine(AnimateText(textObject2, textObject2OriginalText, textObjectLetterDelay));
        StartCoroutine(AnimateText(textObject3, textObject3OriginalText, textObjectLetterDelay));

        // Wait for all animations to finish
        yield return new WaitForSeconds((textObject1OriginalText.Length + textObject2OriginalText.Length + textObject3OriginalText.Length) * textObjectLetterDelay);
    }

    IEnumerator AnimateText(TMP_Text textObject, string originalText, float letterDelay)
    {
        textObject.text = "";

        foreach (char letter in originalText)
        {
            textObject.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    IEnumerator BlinkCursor()
    {
        while (true)
        {
            cursorVisible = !cursorVisible;
            consoleText.text = currentMessage + (cursorVisible ? "_" : "");
            yield return new WaitForSeconds(cursorBlinkRate);
        }
    }

    public void AddMessage(string message)
    {
        messageQueue.Enqueue(message);

        // Ensure that the queue does not exceed the maximum number of lines
        while (messageQueue.Count > maxLines)
        {
            messageQueue.Dequeue();
        }

        // Update current message to include all messages in the queue
        currentMessage = string.Join("\n", messageQueue.ToArray());
    }
}
