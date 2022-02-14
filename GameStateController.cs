using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{

    //選択されたカードのIDリスト
    public List<int> SelectedCardIdList = new List<int>();

    //シングルトンの生成
    public static GameStateController mInstance;

    public static GameStateController Instance
    {
        get
        {
            //インスタンスが生成されていない場合、自動生成する
            if(mInstance == null)
            {
                GameObject obj = new GameObject("GameStateController");
                mInstance = obj.AddComponent<GameStateController>();
            }
            return mInstance;
        }
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
