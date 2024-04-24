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
        else if (args[0] == "ping")
        {
            response.Add("pong!");
        }
        else if (args[0] == "boop")
        {
            response.Add("boop!");
        }
        else if (args[0] == "logs"){
            string[] logs = {};//PlayerPrefs.GetString("logs").Split(',');
            if(logs.Length < 1){
                response.Add("You have discovered 0 logs");
            }
            else{
                foreach(string log in logs){
                    response.Add(log);
                }
            }
        }
        else if (args[0] == "shutdown /s")
        {
            response.Add("Goodbye!");
            Application.Quit();
        }
        else
        {
            response.Add("Command not recognized. Type help for a list of commands.");

        }
        return (response);
    }
}
