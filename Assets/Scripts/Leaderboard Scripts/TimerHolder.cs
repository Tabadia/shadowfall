using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TimerHolder : MonoBehaviour
{

    public float timer = 0;
    float timer_print = 0;
    public Text currentTimeText;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            timer = 0;
            currentTimeText.text = "";
        }

        timer_print = (int)(timer * 100);
        timer_print /= 100;
        currentTimeText.text = (timer_print).ToString();
    }

    public float getTime()
    {
        return timer;
    }
}
