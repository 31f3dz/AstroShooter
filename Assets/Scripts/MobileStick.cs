using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobileStick : MonoBehaviour
{
    public float MaxLength = 70; //タブが動く最大距離
    PlayerController player; //PlayerControllerスクリプトの情報

    Vector2 defPos; //モバイルスティックの初期座標
    Vector2 downPos; //タップした時の座標

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーについているコンポネントを取得
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //モバイルスティックの初期座標
        defPos = GetComponent<RectTransform>().localPosition;
    }

    //タップした時用のイベント
    public void PadDown()
    {
        //マウスでクリック（タップ）した座標
        downPos = Input.mousePosition;
    }

    //ドラッグイベント
    public void PadDrag()
    {
        //ズラした段階でのタップの座標
        Vector2 mousePosition = Input.mousePosition;
        //新しいタブの位置を求める（どれだけずらしたか）
        Vector2 newTabPos = mousePosition - downPos;
        //方向ベクトルを作成
        Vector2 axis = newTabPos.normalized; //正規化 1にする
        //newTabPos.y = 0; //縦には動かないようにする


        //パッドが移動限界で留まるようにする
        //2者間の距離を求める
        float len = Vector2.Distance(defPos, newTabPos);
        if (len > MaxLength)
        {
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }

        //実際にスティックを移動させる
        GetComponent<RectTransform>().localPosition = newTabPos;

        //プレイヤーのaxisHをモバイル側から決めるためのメソッド
        player.MobileAxis(axis.x,axis.y);
    }

    //指を話した時
    public void PadUp()
    {
        //指が離れたら最初の場所に戻る
        GetComponent<RectTransform>().localPosition = defPos;
        //プレイヤーのaxisHをモバイル側から決めるためのメソッド
        player.MobileAxis(0,0);
    }

    public void Attack()
    {
        RifleShooter rifle = player.GetComponent<RifleShooter>();
        rifle.Attack();
    }

}
