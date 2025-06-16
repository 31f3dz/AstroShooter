using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isGoldDoor; // 金の扉にするかどうか
    public Sprite goldDoorImage; // 金の扉の絵

    public bool isEkey; // ボタンを押して反応するイベント
    bool inDoorArea; // ドアの領域
    bool isDelete; // 削除すべきかどうか

    // トークキャンバスのオブジェクト群を認識できるようにする
    GameObject messageCanvas;
    GameObject messagePanel;
    TextMeshProUGUI messageText;

    bool talking; // 会話発生中かどうかのフラグ

    // セーブデータ用の識別ID
    public int arrangeId;

    // Start is called before the first frame update
    void Start()
    {
        if (isGoldDoor) // もしも金の扉のフラグがあれば
        {
            // 見た目を変える
            GetComponent<SpriteRenderer>().sprite = goldDoorImage;
        }

        // TalkCanvasを見つける
        messageCanvas = GameObject.FindGameObjectWithTag("Talk");
        // TalkCanvasの子供たちから"TalkPanel"というオブジェクトを探す
        messagePanel = messageCanvas.transform.Find("TalkPanel").gameObject;
        messageText = messagePanel.transform.Find("TalkText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // キーイベントのフラグがONでDoorの近くにいる場合
            if (isEkey && inDoorArea && !talking)
            {
                // 金の扉
                if (isGoldDoor && GameController.hasGoldKey > 0)
                {
                    GameController.hasGoldKey--; // 鍵を消費
                    isDelete = true;
                    messagePanel.SetActive(true); // メッセージパネル表示
                    messageText.text = "金の鍵を使った！";
                    talking = true; // 会話モードON
                    GameController.gameState = "talk";
                    Time.timeScale = 0; // ゲーム進行を止める
                }
                // 銀の扉
                else if (!isGoldDoor && GameController.hasSilverKey > 0)
                {
                    GameController.hasSilverKey--; // 鍵を消費
                    isDelete = true;
                    messagePanel.SetActive(true); // メッセージパネル表示
                    messageText.text = "銀の鍵を使った！";
                    talking = true; // 会話モードON
                    GameController.gameState = "talk";
                    Time.timeScale = 0; // ゲーム進行を止める
                }
                // いずれにしても鍵がない
                else
                {
                    messagePanel.SetActive(true); // メッセージパネル表示
                    messageText.text = "鍵がかかっている";
                    talking = true; // 会話モードON
                    GameController.gameState = "talk";
                    Time.timeScale = 0; // ゲーム進行を止める
                }
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

            if (isDelete)
            {
                Destroy(gameObject); // 鍵を開けた場合のみ削除
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // キーイベントフラグがなく、ぶつかった相手がPlayer
        if (!isEkey && collision.gameObject.CompareTag("Player"))
        {
            // 金の扉でなく、銀の鍵を所持していたら
            if (!isGoldDoor && GameController.hasSilverKey > 0)
            {
                GameController.hasSilverKey--;
                Destroy(gameObject);
            }

            // 金の扉で、金の鍵を所持していたら
            if (isGoldDoor && GameController.hasGoldKey > 0)
            {
                GameController.hasGoldKey--;
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // キーイベントあり、相手がプレイヤーだったら
        if (isEkey && collision.gameObject.CompareTag("Player"))
        {
            inDoorArea = true; // ドアの領域内に侵入
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // キーイベントあり、相手がプレイヤーだったら
        if (isEkey && collision.gameObject.CompareTag("Player"))
        {
            inDoorArea = false; // ドアの領域から抜ける
        }
    }
}
