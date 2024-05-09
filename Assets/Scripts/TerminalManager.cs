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
    public float verticalPadScale = 27.0f;
    public TMP_InputField terminalInput;
    public GameObject userInputLine;
    public ScrollRect sr;
    public GameObject msgList;

    Interpreter interpreter;


    private void Start()
    {
        interpreter = GetComponent<Interpreter>();
    }


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

            //Add the interpretation line
            int lines = AddInterpreterLines(interpreter.Interpret(userInput));

            ScrollToBottom(lines);
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
        msgList.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(msgListSize.x, msgListSize.y + verticalPadScale);


        //Instantiate the directory line
        GameObject msg = Instantiate(directoryLine, msgList.transform);

        //Set child index
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);

        //Set the text of the new game object
        msg.GetComponentsInChildren<TMP_Text>()[1].text = userInput;




    }

    int AddInterpreterLines(List<string> interpretation)
    {
        for (int i = 0; i < interpretation.Count; i++)
        {
            //Instantiate the response line
            GameObject res = Instantiate(responseLine, msgList.transform);

            res.transform.SetAsLastSibling();

            //Get the size of the message list + resize
            Vector2 listSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(listSize.x, listSize.y + 27.0f);


            res.GetComponentInChildren<TMP_Text>().text = interpretation[i];



        }

        return interpretation.Count;
    
    }

    void ScrollToBottom(int lines)
    {
        if (lines > 4)
        {

            sr.velocity = new Vector2(0, 450);
        }
        else
        {
            sr.verticalNormalizedPosition = 0;
        
        }
    }

}
