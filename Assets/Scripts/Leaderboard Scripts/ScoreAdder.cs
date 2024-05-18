using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreAdder : MonoBehaviour
{

    [SerializeField]
    public GameObject scoreScreen;
    public GameObject thetimer;
   
    public Text scoretext;
    public float currentTime = 0f;
    public int scorenum = 0;
    public ScoreUi scoreManager;



    [Header("Menu")]
    public string _Menu;

    public void Start()
    {
        scoreManager.ListScores("Top", 1000);

    }


    

    // Update is called once per frame
    public void Update()
    {
        currentTime = thetimer.GetComponent<TimerHolder>().timer;
        //if (Input.GetKeyDown(KeyCode.P))
        //{
           // ScoreTime();
       // }
    }


    public void ScoreTime()
    {
        scoreScreen.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

        scorenum = (int)(1000000.0/currentTime);
        scoretext.text = ((int)(100000.0/currentTime)).ToString();


    }

    public void AddTheScore(string s)
    {
        scoreManager.ListScores(s, scorenum);
        scorenum = 0;
        scoreScreen.SetActive(false);
        Time.timeScale = 1f;
        
    }

    
    
}
