using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    //一致したカードリストID
    private List<int> mContainCardIdList = new List<int>();

    //カード生成マネージャークラス
    public CardCreateManager CardCreate;

    //時間管理クラス
    public TimerManager timerManager;

    //経過時間
    private float mElapsedTime;

    //スタートステートクラス
    public StartStateManager startStateManager;

    //リザルトステートクラス
    public ResultStateManager resultStateManager;

    //ゲームステート管理
    private EGameState mEGameState;

     void Start()
    {
        //一致したカードIDリストを初期化
        //this.mContainCardIdList.Clear();

        //カードリストを生成する
        //this.CardCreate.CreateCard();

        //時間を初期化
        //this.mElapsedTime = 0f;

        //ゲームステートを初期化
        this.mEGameState = EGameState.START;

        //スタートエリアを表示
        //this.startStateManager.gameObject.SetActive(true);

        //リザルトエリアを非表示
        this.resultStateManager.gameObject.SetActive(false);

        //ゲームのステート管理
        this.mSetGameState();
    }

    /// <summary>
    /// ゲームステートで処理を変更する
    /// </summary>
    private void mSetGameState()
    {
        switch (this.mEGameState)
        {
            //スタート画面
            case EGameState.START:
                this.startStateManager.gameObject.SetActive(true);
                //ゲームスタートの開始
                this.mSetStartState();
                break;
            //ゲーム準備画面
            case EGameState.READY:
                //ゲームの準備ステートを開始する
                this.mSetGameReady();
                break;
            //ゲーム中
            case EGameState.GAME:
                break;
            //結果画面
            case EGameState.RESULT:
                this.resultStateManager.gameObject.SetActive(true);
                this.mSetResultState();
                break;
        }
    }

    private void mSetStartState()
    {
        //テキストの拡大縮小アニメーション
        this.startStateManager.EnlarAnimation();
    }

    ///<summary>
    ///Readyステートに遷移する
    ///</summary>
    public void OnGameStart()
    {
        //スタートエリアを非表示
        this.startStateManager.gameObject.SetActive(false);
        //ゲームステートを初期化
        this.mEGameState = EGameState.READY;
        //ゲームのステート管理
        this.mSetGameState();
    }

    /// <summary>
    /// ゲームの準備ステートを開始する
    /// </summary>
    private void mSetGameReady()
    {
        //一致したカードIDリストを初期化
        this.mContainCardIdList.Clear();

        //カードリストを生成する
        this.CardCreate.CreateCard();

        //カード配布アニメーションが終了した後のコールバック処理を実装する
        this.CardCreate.OnCardAnimeComp = null;
        this.CardCreate.OnCardAnimeComp = () =>
        {
            //ゲームステートをGAME状態に変更する
            this.mEGameState = EGameState.GAME;
            this.mSetGameState();
        };

        //時間を初期化
        this.mElapsedTime = 0f;
    }

    /// <summary>
    /// リザルトステートの設定処理
    /// </summary>
    private void mSetResultState()
    {
        this.resultStateManager.SetTimerText((int)this.mElapsedTime);
    }

    /// <summary>
    /// スタート画面に遷移する
    /// </summary>
    public void OnBackStartState()
    {
        //ResultAreaを非表示にする
        this.resultStateManager.gameObject.SetActive(false);

        //ゲームステートをSTARTに変更
        this.mEGameState = EGameState.START;

        //ゲームのステート管理
        this.mSetGameState();
    }

    void Update()
    {
        //GameStateがGAME状態なら
        if (this.mEGameState == EGameState.GAME)
        {
            this.timerManager.gameObject.SetActive(true);

            this.mElapsedTime += Time.deltaTime;

            this.timerManager.SetText((int)this.mElapsedTime);

            //選択したカードが２枚以上になったら
            if (GameStateController.Instance.SelectedCardIdList.Count >= 2)
            {
                //最初に選択したCardIDを取得する
                int selectedId = GameStateController.Instance.SelectedCardIdList[0];

                //2枚目にあったカードと一緒だったら
                if (selectedId == GameStateController.Instance.SelectedCardIdList[1])
                {
                    Debug.Log($"Contains!{selectedId}");
                    //一致したカードIDを保存する
                    this.mContainCardIdList.Add(selectedId);
                }

                //カードを表示切り替えを行う
                this.CardCreate.HideCardList(this.mContainCardIdList);

                //選択したカードリストを初期化する
                GameStateController.Instance.SelectedCardIdList.Clear();
            }

            //配置した全種類のカードを獲得したら
            if(this.mContainCardIdList.Count >= 6)
            {
                this.timerManager.gameObject.SetActive(false);
                //ゲームをリザルトステートに遷移する
                this.mEGameState = EGameState.RESULT;
                this.mSetGameState();
            }
        }
    }
}
