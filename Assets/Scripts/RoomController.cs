using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    // ゲーム中共通して管理するドア番号
    public static int doorNumber;

    // シーン移動かどうか、あるいはコンティニューかどうか
    public static bool isContinue;

    // プレイヤーのアニメと方向を計算
    int direction;
    float angleZ;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (isContinue)
        {
            // ゲームをロードした処理
            // レジストリから最後の座標データを獲得
            float posX = PlayerPrefs.GetFloat("posX");
            float posY = PlayerPrefs.GetFloat("posY");

            // 獲得したデータをもとにプレイヤーの座標を設定
            player.transform.position = new Vector2(posX, posY);
            return;
        }

        GameObject[] exits = GameObject.FindGameObjectsWithTag("Exit");
        for (int i = 0; i < exits.Length; i++)
        {
            Exit exit = exits[i].GetComponent<Exit>();
            if (doorNumber == exit.doorNumber)
            {
                float x = exits[i].transform.position.x;
                float y = exits[i].transform.position.y;

                switch (exit.direction)
                {
                    case ExitDirection.up:
                        y += 1;
                        direction = 1; // 上向きアニメの番号
                        angleZ = 90; // 上向きの角度
                        break;
                    case ExitDirection.down:
                        y -= 1;
                        direction = 0; // 下向きアニメの番号
                        angleZ = -90; // 下向きの角度
                        break;
                    case ExitDirection.left:
                        x -= 1;
                        direction = 2; // 左向きアニメの番号
                        angleZ = 180; // 左向きの角度
                        break;
                    case ExitDirection.right:
                        x += 1;
                        direction = 3; // 右向きアニメの番号
                        angleZ = 0; // 右向きの角度
                        break;
                }

                player.GetComponent<PlayerController>().angleZ = angleZ; // プレイヤーの角度を決める
                player.GetComponent<Animator>().SetInteger("direction", direction); // アニメを決める
                player.transform.position = new Vector3(x, y);

                break; // そのループを抜ける
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 静的メソッド(どこのシーンに、何番のドア)
    public static void ChangeScene(string scenename, int doornum)
    {
        // staticであるRoomControllerのdoorNumberに引数に指定したdoornumを代入
        doorNumber = doornum; // 次のシーンにドア番号が引き継がれる
        isContinue = false; // コンティニューではなく単なる部屋移動
        SceneManager.LoadScene(scenename);
    }
}
