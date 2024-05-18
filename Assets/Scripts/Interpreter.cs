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

            response.Add("Type \"logs\" to see the logs you have discovered");
            response.Add("Type \"credits\" to see the credits");
            response.Add("Type \"company-secrets\" to see the company secrets");

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
        else if (args[0] == "credits"){
            response.Add("Tanush Sistla - Team Manager & Programmer");
            response.Add("Thalen Abadia - Lead Programmer");
            response.Add("Angela Zhang - Programmer & Writer");
            response.Add("Phoenix Jones - Audio Producer");
            response.Add("Wesley Dean - Writer");
            response.Add("Declan Chamberlain - 2D Artist");
            response.Add("Teo Fine - 3D Modeler");
        }
        else if (args[0] == "company-secrets"){
            response.Add("Your actions against the company have been brought to the attention of Solis Corp's security team.");
            response.Add("You have been marked as a threat to the company.");
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
