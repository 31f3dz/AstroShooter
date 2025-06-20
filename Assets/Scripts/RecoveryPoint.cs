using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecoveryPoint : MonoBehaviour
{
    //接触ではなく領域に入ったかどうかのフラグ
    bool inSavePointArea;

    //トークキャンバスのオブジェクト達を認識できるようにする
    GameObject messageCanvas;
    GameObject messagePanel;
    TextMeshProUGUI messageText;

    bool talking; //会話発生中かどうか

    [TextArea]
    public string message;

    // Start is called before the first frame update
    void Start()
    {
        //TalkCanvasを見つける
        messageCanvas = GameObject.FindGameObjectWithTag("Talk");
        //TalkCanvasの子どもから"TalkPanel"というオブジェクトを探す
        messagePanel = messageCanvas.transform.Find("TalkPanel").gameObject;
        messageText = messagePanel.transform.Find("TalkText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Eキーがおされたら
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Playerが近くにいる
            if (inSavePointArea)
            {
                GameController.gameState = "talk";

                PlayerController.hp = 5; //全回復

                //UIパネルを表示
                messagePanel.SetActive(true);
                //UIテキストに変数の内容を反映
                messageText.text = message;
                talking = true; //会話が開始されている
                Time.timeScale = 0f; //ゲーム進行を止める
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //すでにトークウィンドウが表示されているならばスペースキーでウィンドウを閉じる
            if (talking && GameController.gameState == "talk")
            {
                messagePanel.SetActive(false); //パネルを閉じる
                talking = false; //会話中フラグをOFF
                GameController.gameState = "playing"; //ゲームステータスを元に戻す
                Time.timeScale = 1f; //時の流れを元に戻す
            }
        }
    }

    //エリアに侵入した時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //相手がPlayerなら
        if (collision.gameObject.CompareTag("Player"))
        {
            inSavePointArea = true; //エリアに入っているというフラグをON
            //GameController.investigate = true; //調べるパネルON
        }
    }

    //エリアから抜けた時
    private void OnTriggerExit2D(Collider2D collision)
    {
        //相手がPlayerなら
        if (collision.gameObject.CompareTag("Player"))
        {
            inSavePointArea = false; //エリア侵入のフラグをOFF
            //GameController.investigate = false; //調べるパネルOFF
        }
    }
}
