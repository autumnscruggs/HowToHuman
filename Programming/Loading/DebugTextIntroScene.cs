using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextIntroScene : MonoBehaviour
{
    private Text text;
    public Loader loader;
    

    private void Awake()
    {
        text = this.GetComponent<Text>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //text.text = "TimeNullOrLessThan01 - " + loader.timeNullOrLessThan01;
        //text.text += "\n TimeInCoroutine - " + loader.timeInCoroutine;
        text.text = "" + loader.async.progress.ToString("F4");
    }
}
