using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //DOScaleを使用できる
using UnityEngine.UI;

public class StartStateManager : MonoBehaviour
//Startを名称に使用する際はvoid Startは使えない
{
    // ゲームの開始テキストの座標
    public RectTransform GameStartTextRt;

    /// <summary>
    /// テキストの拡大アニメーション
    /// </summary>
    public void EnlarAnimation()
    {

        this.GameStartTextRt.DOScale(Vector3.one * 1.5f, 0.5f)
            .OnComplete(() =>
            {
                // テキストの拡大アニメーション
                this.mShrinkAnimation();
            });
    }

    /// <summary>
    /// テキストの縮小アニメーション
    /// </summary>
    private void mShrinkAnimation()
    {

        this.GameStartTextRt.DOScale(Vector3.one * 0.5f, 0.5f)
            .OnComplete(() =>
            {
                // テキストの縮小アニメーション
                this.EnlarAnimation();
            });
    }
}