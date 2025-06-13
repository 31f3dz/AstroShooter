using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public enum ItemType // 列挙型 ※自作するデータ型
{
    light,
    bullet,
    goldKey,
    silverKey,
    life
}

public class ItemData : MonoBehaviour
{
    public ItemType type; // アイテムの種類
    public int stockCount; // 入手数

    // セーブデータの識別番号
    public int arrangeId;

    Rigidbody2D rbody;

    public bool isTalk; // トークパネルでアナウンスするかのフラグ

    [TextArea]
    public string message1; // メッセージ1
    [TextArea]
    public string message2; // メッセージ2

    // トークキャンバスのオブジェクト群を認識できるようにする
    GameObject messageCanvas;
    GameObject messagePanel;
    TextMeshProUGUI messageText;

    bool talking; // 会話発生中かどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        // TalkCanvasを見つける
        messageCanvas = GameObject.FindGameObjectWithTag("Talk");
        // TalkCanvasの子供たちから"TalkPanel"というオブジェクトを探す
        messagePanel = messageCanvas.transform.Find("TalkPanel").gameObject;
        messageText = messagePanel.transform.Find("TalkText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 既にトークウィンドウが表示されていればスペースキーでウィンドウを閉じる
            if (talking && GameController.gameState == "talk")
            {
                messagePanel.SetActive(false); // パネルを閉じる
                talking = false; // 会話中フラグをOFF
                GameController.gameState = "playing"; // ゲームステータスを元に戻す
                Time.timeScale = 1f; // 時の流れを元に戻す

                ItemDestroy(); // アイテムは消滅
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case ItemType.light:
                    LightController.getLight = true; // ライトの入手フラグをON
                    break;
                case ItemType.bullet:
                    GameController.hasBullet += stockCount;
                    break;
                case ItemType.goldKey:
                    GameController.hasGoldKey += stockCount;
                    break;
                case ItemType.silverKey:
                    GameController.hasSilverKey += stockCount;
                    break;
                case ItemType.life:
                    if (PlayerController.hp < 5) PlayerController.hp++;
                    break;
                default:
                    break;
            }

            if (!isTalk)
            {
                ItemDestroy(); // アイテムは消滅
            }
            else
            {
                GameController.gameState = "talk";

                // トークウィンドウに出すメッセージの作成
                string message = message1 + stockCount + message2;
                // UIパネルを表示
                messagePanel.SetActive(true);
                // UIテキストに変数の内容を反映
                messageText.text = message;
                talking = true; // 会話が開始されている
                Time.timeScale = 0f; // ゲーム進行を止める
            }
        }
    }

    // Itemの消滅演出
    void ItemDestroy()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        rbody.gravityScale = 2.5f;
        rbody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
        Destroy(gameObject, 0.5f);
    }
}
