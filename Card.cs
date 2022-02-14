using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    //カードのID
    public int Id;

    //表示するカードの画像
    public Image CardImage;

    //透過処理用
    public CanvasGroup CanGroup;

    //選択されているか判定
    private bool mIsSelected = false;
    public bool IsSelected => this.mIsSelected;

    //カード情報
    private CardData mData;

    //座標情報
    public RectTransform mRt;

    //カードの設定
    public void Set(CardData data)
    {
        //カード情報を設定
        this.mData = data;

        //IDを設定する
        this.Id = data.Id;

        //表示する画像を設定する
        //this.CardImage.sprite = data.ImgSprite;
        //初回は全て裏面表示とする
        this.CardImage.sprite = Resources.Load<Sprite>("Image/red");

        //選択判定フラグを初期化する
        this.mIsSelected = false;

        //アルファ値を1に設定
        this.CanGroup.alpha = 1;

        //座標情報を取得しておく
        this.mRt = this.GetComponent<RectTransform>();
    }

    ///<summary>
    ///選択された時の処理
    ///</summary>
    public void OnClick()
    {
        //カードが表面になっていた場合は無効
        if (this.mIsSelected)
        {
            return;
        }

        Debug.Log("OnClick");

        //Dotweenで回転処理を行う
        //this.mRt.DORotate(new Vector3(0f, 90f, 0f), 0.2f)
        //回転処理を行う
        this.onRotate(() =>
        {
            //回転終了後
            //.OnComplete(() => {
                //選択判定フラグを有効する
                this.mIsSelected = true;

                //カードを表面にする
                this.CardImage.sprite = this.mData.ImgSprite;

            //Y座標を元に戻す
            this.onReturnRotate(() =>
            {
                    //選択したCardIdを保存しよう！
                    GameStateController.Instance.SelectedCardIdList.Add(this.mData.Id);
            });
        });
    }

    ///<summary>
    ///カードを90度に回転する
    ///</summary>
    private void onRotate(Action onComp)
    {
        this.mRt.DORotate(new Vector3(0f, 90f, 0f), 0.2f)
            //回転終了後
            .OnComplete(() =>
            {
                if(onComp != null)
                {
                    onComp();
                }
            });
    }

    ///<summary>
    ///カードの回転軸を元に戻す
    ///</summary>
    private void onReturnRotate(Action onComp)
    {
        this.mRt.DORotate(new Vector3(0f, 0f, 0f), 0.2f)
            //回転終了後
            .OnComplete(() =>
            {
                if(onComp != null)
                {
                    onComp();
                }
                //選択したCardIdを保存しよう！
                //GameStateController.Instance.SelectedCardIdList.Add(this.mData.Id);
            });
    }

    ///<summary>
    ///カードを背面表記にする
    ///</summary>
    public void SetHide()
    {
        //90度回転する
        this.onRotate(() =>
        {
            //選択判定フラグを初期化する
            this.mIsSelected = false;

            //カードを背面表示にする
            this.CardImage.sprite = Resources.Load<Sprite>("Image/red");

            //角度を元に戻す
            this.onReturnRotate(() =>
            {
                Debug.Log("onhide");
            });
        });
    }

    ///<summary>
    ///カードを非表示にする
    ///</summary>
    public void SetInvisible()
    {
        //選択済設定にする
        this.mIsSelected = true;

        //アルファ値を0に設定(非表示)
        this.CanGroup.alpha = 0;
    }
}

///<summary>
///カードの情報クラス
///</summary>
public class CardData
{
    //カードID
    public int Id { get; private set; }

    //画像
    public Sprite ImgSprite { get; private set; }

    public CardData(int _id, Sprite _sprite)
    {
        this.Id = _id;
        this.ImgSprite = _sprite;
    }
}