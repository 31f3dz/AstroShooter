using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 出口として機能したときのプレイヤーの位置を列挙型で自作
public enum ExitDirection
{
    right,
    left,
    up,
    down
}

public class Exit : MonoBehaviour
{
    public string sceneName; // 切り替え先のシーン名
    public int doorNumber; // 切り替え先の出入口との連動番号

    // 自作した列挙型でプレイヤーをどの位置に置く出口なのか決めておく ※初期値は下
    public ExitDirection direction = ExitDirection.down;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // RoomControllerのシーン切り替えメソッド発動
            RoomController.ChangeScene(sceneName, doorNumber);
        }
    }
}
