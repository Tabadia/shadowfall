using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject directoryLine;
    public GameObject responseLine;

    public TMP_InputField terminalInput;
    public GameObject userInputLine;
    public ScrollRect sr;
    public GameObject msgList;


    private void OnGUI()
    {
        if (terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            //Store user input as a string
            string userInput = terminalInput.text;

            //Clear the input field
            ClearInputField();

            //Instantiate a game object with a director prefix
            AddDirectoryLine(userInput);

            //Move the user input line to the end
            userInputLine.transform.SetAsLastSibling();

            //Refocus the input field

            terminalInput.ActivateInputField();
            terminalInput.Select();


        }
    }

    void ClearInputField()
    {
        terminalInput.text = "";

    }

    void AddDirectoryLine(string userInput)
    {
        //Resizing the command line container so ScrollRect doesn't crash
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(msgListSize.x, msgListSize.y + 27.0f);


        //Instantiate the directory line
        GameObject msg = Instantiate(directoryLine, msgList.transform);

        //Set child index
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);

        //Set the text of the new game object
        msg.GetComponentsInChildren<TMP_Text>()[1].text = userInput;

        StartCoroutine(ScrollToBottom());



    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        sr.gameObject.SetActive(true);
        sr.verticalNormalizedPosition = 0f;
    }

}
