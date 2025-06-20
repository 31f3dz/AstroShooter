using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenManager : MonoBehaviour
{
    ObjectGenPoint[] objectGens; // シーンに配置されているObjectGenPointの配列

    // Start is called before the first frame update
    void Start()
    {
        objectGens = GameObject.FindObjectsOfType<ObjectGenPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        // ItemDataを探す
        ItemData[] items = GameObject.FindObjectsOfType<ItemData>();

        // ループを回して弾丸を探す
        for (int i = 0; i < items.Length; i++)
        {
            ItemData item = items[i];
            if (item.type == ItemType.bullet)
            {
                return; // 弾丸があれば何もせずにメソッドを抜ける
            }
        }

        // プレイヤーの存在と弾丸の数をチェックする
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (GameController.hasBullet == 0 && player != null)
        {
            // 弾丸の数が0でプレイヤーがいる
            // 配列の範囲で乱数を作る
            int index = Random.Range(0, objectGens.Length);
            ObjectGenPoint objgen = objectGens[index];
            objgen.ObjectCreate(); // アイテム配置
        }
    }
}
