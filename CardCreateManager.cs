using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System; //Guid使用するのに必要
using DG.Tweening;
using UnityEngine.UI; //GridLayoutGroupを使用するのに使用

public class CardCreateManager : MonoBehaviour
{
    //生成するCardオブジェクト
    public Card CardPrefab;

    //「カード」を生成する親オブジェクト
    public RectTransform CardCreateParent;

    //生成したカードオブジェクトを保存する
    public List<Card> CardList = new List<Card>();

    //カード情報の順位をランダムに変更したリスト
    private List<CardData> mRandomCardDataList = new List<CardData>();

    //GridLayoutGroup
    public GridLayoutGroup GridLayout;

    //カードの生成アニメーションが終わった時
    public Action OnCardAnimeComp;

    //カード配列のインデックス
    private int mIndex;

    //カードを生成するときの高さインデックス
    private int mHelghtIdx;

    //カードを生成するときの幅インデックス
    private int mWidthIdx;

    //カードの生成アニメーションのアニメーション時間
    private readonly float DEAL_CARD_TIME = 0.2f;

    ///<summary>
    ///カードを生成する
    ///</summary>
    public void CreateCard()
    {
        foreach (Transform child in this.CardCreateParent)
        {
            //GameObjectを全て削除
            Destroy(child.gameObject);
            //Listを初期化(Elementを削除)
            CardList.Clear();
        }

            //カード情報リスト
            List<CardData> cardDataList = new List<CardData>();

            //表示するカード画像情報のリスト
            List<Sprite> imgList = new List<Sprite>();

            //Resources/Imageフォルダ内にある画像を取得する
            imgList.Add(Resources.Load<Sprite>("Image/redd"));
            imgList.Add(Resources.Load<Sprite>("Image/ao"));
            imgList.Add(Resources.Load<Sprite>("Image/gr"));
            imgList.Add(Resources.Load<Sprite>("Image/ye"));
            imgList.Add(Resources.Load<Sprite>("Image/or"));
            imgList.Add(Resources.Load<Sprite>("Image/pl"));

            //forを回す回数を取得する
            int loopCnt = imgList.Count;

            for (int i = 0; i < loopCnt; i++)
            {
                //カード情報を生成する
                CardData carddata = new CardData(i, imgList[i]);
                cardDataList.Add(carddata);
            }

            this.mIndex = 0;
            this.mHelghtIdx = 0;
            this.mWidthIdx = 0;

            //生成したカードリストを2つ分のリストを生成する
            List<CardData> SumCardDataList = new List<CardData>();
            SumCardDataList.AddRange(cardDataList);

            //ランダムリストの初期化
            this.mRandomCardDataList.Clear();

            //リストの中身をランダムに再配置する
            //List<CardData>
            this.mRandomCardDataList = SumCardDataList.OrderBy(a => Guid.NewGuid()).ToList();
            //List<CardData>
            this.mRandomCardDataList.AddRange(SumCardDataList.OrderBy(a => Guid.NewGuid()).ToList());

            //GridLayoutを無効
            this.GridLayout.enabled = false;

            //カードを配るアニメーション処理
            this.mSetDealCardAnime();

            //カードオブジェクトを生成する
            //foreach (var _cardData in mRandomCardDataList)
            //{
            //InstantiateでCardオブジェクトを生成
            //Card card = Instantiate<Card>(this.CardPrefab, this.CardCreateParent);

            //データを設定する
            //card.Set(_cardData);

            //生成しかカードオブジェクトを保存する
            //this.CardList.Add(card);
            //}
        }

    ///<summary>
    ///カードを配るアニメーション処理
    ///</summary>
    private void mSetDealCardAnime()
    {
        var _cardData = this.mRandomCardDataList[this.mIndex];
        //InstantiateでCardオブジェクトを生成
        Card card = Instantiate<Card>(this.CardPrefab, this.CardCreateParent);
        //データを設定する
        card.Set(_cardData);
        //カードの初期化を設定(画面外にする)
        card.mRt.anchoredPosition = new Vector2(1900, 0f);
        //サイズをGridLayoutのCellSizeに設定
        card.mRt.sizeDelta = this.GridLayout.cellSize;

        //カードの移動先を設定
        float posX = (this.GridLayout.cellSize.x * this.mWidthIdx) + (this.GridLayout.spacing.x * (this.mWidthIdx + 1));
        float posY = ((this.GridLayout.cellSize.y * this.mHelghtIdx) + (this.GridLayout.spacing.y * this.mHelghtIdx)) * -1f;

        //DOAnchorPosでアニメーションを行う
        card.mRt.DOAnchorPos(new Vector2(posX, posY), this.DEAL_CARD_TIME)
            //アニメーションが終了したら
            .OnComplete(() =>
            {
                //生成したカードオブジェクトを保存する
                this.CardList.Add(card);

                //生成するカードデータリストのインデックスを更新
                this.mIndex++;
                this.mWidthIdx++;

                //生成インデックスがリストの最大値を迎えたら
                if (this.mIndex >= this.mRandomCardDataList.Count)
                {
                    //GridLayoutを有効にし、生成処理を終了する
                    this.GridLayout.enabled = true;

                    //アニメーション終了時の関数を宣言する
                    if (this.OnCardAnimeComp != null)
                    {
                        this.OnCardAnimeComp();
                    }
                }
                else
                {
                    //GridLayoutの折り返し地点に来たら
                    if (this.mIndex % this.GridLayout.constraintCount == 0)
                    {
                        //高さの生成箇所を更新
                        this.mHelghtIdx++;
                        this.mWidthIdx = 0;
                    }
                    //アニメーション処理を再帰処理する
                    this.mSetDealCardAnime();
                }
            });
    }

    ///<summary>
    ///取得していないカードを背面にする
    ///</summary>
    public void HideCardList(List<int> containCardIdList)
    {
        foreach (var _card in this.CardList)
        {
            //既に獲得したカードIDの場合、非表示にする
            if (containCardIdList.Contains(_card.Id))
            {
                //カードを非表示にする
                _card.SetInvisible();
            }
            //カードが表面&&獲得していないカードは裏面表示にする
            else if (_card.IsSelected)
            {
                //カードを裏面表示にする
                _card.SetHide();
            }
        }
    }
}