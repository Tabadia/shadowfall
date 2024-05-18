using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject canvas;

    public void Open()
    {
        canvas = GameObject.Find("ScoreCanvas");
        canvas.SetActive(true);
    }
}
