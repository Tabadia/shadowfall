using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour
{


    List<string> response = new List<string>();
    // Start is called before the first frame update
    public List<string> Interpret(string userInput)
    {


        response.Clear();


        string[] args = userInput.Split();

        if (args[0] == "help")
        {

            response.Add("If you want to use the terminal, type \"boop\" ");
            response.Add("We're testing the potential outputs");

        }
        else
        {
            response.Add("Command not recognized. Type help for a list of commands.");

        }
        return (response);
    }
}
