using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{

    // 時間表示
    public Text TimerText;

    /// <summary>
    /// タイマーのテキストの設定
    /// </summary>
    public void SetText(int time)
    {

        this.TimerText.text = "Time : " + time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
