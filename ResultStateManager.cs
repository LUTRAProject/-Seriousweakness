using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultStateManager : MonoBehaviour
{
    //時間を表示するテキスト
    public Text TimerText;

    //<summary>
    //ゲームで経過した時間を表示する
    //</summary>
    public void SetTimerText(int timer)
    {
        this.TimerText.text = timer.ToString();
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
