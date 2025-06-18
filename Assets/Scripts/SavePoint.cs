using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    bool inSavePointArea;

    // トークキャンバスのオブジェクト群を認識できるようにする
    GameObject messageCanvas;
    GameObject messagePanel;
    TextMeshProUGUI messageText;

    bool talking; // 会話発生中かどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        // TalkCanvasを見つける
        messageCanvas = GameObject.FindGameObjectWithTag("Talk");
        // TalkCanvasの子供たちから"TalkPanel"というオブジェクトを探す
        messagePanel = messageCanvas.transform.Find("TalkPanel").gameObject;
        messageText = messagePanel.transform.Find("TalkText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Eキーが押されたら
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Playerが近くにいる
            if (inSavePointArea)
            {
                SaveSystem.SaveGame(); // セーブされる

                GameController.gameState = "talk";

                // トークウィンドウに出すメッセージの作成
                string message = "セーブされました！";
                // UIパネルを表示
                messagePanel.SetActive(true);
                // UIテキストに変数の内容を反映
                messageText.text = message;
                talking = true; // 会話が開始されている
                Time.timeScale = 0f; // ゲーム進行を止める
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 既にトークウィンドウが表示されていればスペースキーでウィンドウを閉じる
            if (talking && GameController.gameState == "talk")
            {
                messagePanel.SetActive(false); // パネルを閉じる
                talking = false; // 会話中フラグをOFF
                GameController.gameState = "playing"; // ゲームステータスを元に戻す
                Time.timeScale = 1f; // 時の流れを元に戻す
            }
        }
    }

    // エリアに侵入したとき
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 相手がPlayerなら
        if (collision.gameObject.CompareTag("Player"))
        {
            inSavePointArea = true; // エリアに入っているというフラグをON
        }
    }

    // エリアから抜けたとき
    void OnTriggerExit2D(Collider2D collision)
    {
        // 相手がPlayerなら
        if (collision.gameObject.CompareTag("Player"))
        {
            inSavePointArea = false; // エリア侵入のフラグをOFF
        }
    }
}
