using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public GameObject itemPrefab; // 中身となるアイテム
    public Sprite openImage; // 開封された宝箱の絵
    public bool isClosed = true; // 未開封かどうか

    // アイテムがPlayerのもとにちゃんと届くように
    GameObject player; // プレイヤーの情報
    // 接触ではなく、領域に入ったかどうかのフラグ
    bool inBoxArea;
    // Eキーで開ける仕様かどうか
    public bool isEkey;

    public int arrangeId; // セーブデータ用の識別ID

    // Start is called before the first frame update
    void Start()
    {
        ExistCheck(); // リストを見て、存在して良いかをチェック
    }

    // Update is called once per frame
    void Update()
    {
        // Eキーが押されたら
        if (Input.GetKeyDown(KeyCode.E))
        {
            // キーイベントで開く＆Playerが近くにいる＆未開封
            if (isEkey && inBoxArea && isClosed)
            {
                BoxOpen(); // 開封処理
            }
        }
    }

    // 宝箱に接触したとき
    void OnCollisionEnter2D(Collision2D collision)
    {
        // もしも接触するだけで入手可能な仕様だったら
        if (!isEkey && isClosed && collision.gameObject.CompareTag("Player"))
        {
            BoxOpen(); // 開封処理
        }
    }

    // エリアに侵入したとき
    void OnTriggerEnter2D(Collider2D collision)
    {
        // キーで入手フラグがONのとき、相手がPlayerなら
        if (isEkey && collision.gameObject.CompareTag("Player"))
        {
            inBoxArea = true; // エリアに入っているというフラグをON
        }
    }

    // エリアから抜けたとき
    void OnTriggerExit2D(Collider2D collision)
    {
        // キーで入手フラグがONのとき、相手がPlayerなら
        if (isEkey && collision.gameObject.CompareTag("Player"))
        {
            inBoxArea = false; // エリア侵入のフラグをOFF
        }
    }

    // 開封処理
    void BoxOpen()
    {
        GetComponent<SpriteRenderer>().sprite = openImage; // 開封済みの絵に差し替え
        isClosed = false;
        if (itemPrefab != null) // アイテムプレハブがセッティングされていれば
        {
            player = GameObject.FindGameObjectWithTag("Player"); // プレイヤー検索
            Instantiate(itemPrefab, player.transform.position, Quaternion.identity); // プレイヤーの位置にアイテムを生成

            // 消費リストにまだ掲載されていなければリストアップ
            if (!SaveController.Instance.IsConsumed(this.tag, arrangeId))
            {
                SaveController.Instance.ConsumedEvent(this.tag, arrangeId);
            }
        }
    }

    void ExistCheck()
    {
        if (SaveController.Instance.IsConsumed(this.tag, arrangeId))
        {
            isClosed = false; // オープン状態
            GetComponent<SpriteRenderer>().sprite = openImage; // 見た目を開いた箱
        }
    }
}
