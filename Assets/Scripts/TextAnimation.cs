using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextAnimation : MonoBehaviour
{
    public TMP_Text consoleText;
    public int maxLines = 10; // Maximum number of lines to display
    public float letterDelay = 0.1f; // Delay between each letter
    public float lineDelay = 0.5f; // Delay between each line
    public float cursorBlinkRate = 0.5f; // Rate at which the cursor blinks

    private Queue<string> messageQueue = new Queue<string>(); // Queue to store messages
    private string currentMessage = ""; // Current message being displayed
    private bool cursorVisible = true; // Flag to control cursor visibility

    void Start()
    {
        /*AddMessage("Testing Database XA34IL Connection:\n" +
            "<color=red>ERROR</color>\n" +
            "Testing Main Connection:\n" +
            "<color=red>ERROR</color>\n" +
            "Testing Cryopod ID 6749 Activation Code:\n" +
            "<color=red>ERROR</color>\n" +
            "Manual Assistance Required\n" +
            "Calling Operations Assistant:\n" +
            "<color=red>ERROR</color>\n" +
            "Activating Emergency Function Code AWAKE:\n" +
            "<color=green>SUCCESS</color>\n" +
            "G o o d   M o r n i n g\n");*/

        AddMessage(consoleText.text);
        StartCoroutine(UpdateConsole());
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
                        yield return new WaitForSeconds(letterDelay);
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

        // Restart the UpdateConsole coroutine to display the updated messages
        StopCoroutine("UpdateConsole");
        StartCoroutine("UpdateConsole");
    }
}
