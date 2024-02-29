using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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
        AddMessage("Testing Database XA34IL Connection............\r\nERROR\r\nTesting Main Connection....\r\nERROR\r\nTesting Cryopod ID 6749 Activation Code.....\r\nERROR\r\nManual Assistance Required\r\nCalling Operations Assistant......\r\nERROR\r\nActivating Function Code \"LAST STRAW\"....\r\nSUCCESS\r\nG o o d   M o r n i n g\r\n\r\n");
        StartCoroutine(UpdateConsole());
        //StartCoroutine(BlinkCursor());
       
    }

    IEnumerator UpdateConsole()
    {
        foreach (string message in messageQueue)
        {
            int messageLength = message.Length;

            for (int i = 0; i <= messageLength; i++)
            {
                consoleText.text = message.Substring(0, i) + (cursorVisible ? "_" : "");
                yield return new WaitForSeconds(letterDelay);
            }

            yield return new WaitForSeconds(lineDelay); // Delay between lines
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
