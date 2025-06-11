using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; // Rgidbody2D
    Animator anime; // Animator

    float axisH, axisV; // 横軸、縦軸

    public float speed = 3.0f; // スピード
    public float angleZ; // 角度
    int direction = 0; // アニメの方向番号

    public static int hp = 5; // プレイヤーの体力
    bool inDamage; // ダメージ中フラグ
    bool isMobileInput; // スマホ操作中かどうか

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        // 下向き
        anime.SetInteger("direction", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMobileInput)
        {
            // 水平方向と垂直方向のキー入力を検知
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
        }

        VectorAnime(axisH, axisV); // 方向アニメを決めるメソッド
    }

    void FixedUpdate()
    {
        // 斜めを縦・横に合わせる
        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;
    }

    void VectorAnime(float h, float v)
    {
        angleZ = GetAngle();

        // アニメ番号を一時記録
        int dir;

        if (angleZ > -135 && angleZ < -45) dir = 0; // 下
        else if (angleZ >= -45 && angleZ <= 45) dir = 3; // 右
        else if (angleZ > 45 && angleZ < 135) dir = 1; // 上
        else dir = 2; // 左

        // 前フレームのdirectionとアニメ番号が異なっていなければそのまま
        if (dir != direction)
        {
            direction = dir;
            anime.SetInteger("direction", direction);
        }
    }

    public float GetAngle()
    {
        Vector2 fromPos = transform.position; // 現在地
        Vector2 toPos = new Vector2(fromPos.x + axisH, fromPos.y + axisV);

        float angle; // 角度情報の受け皿

        if (axisH != 0 || axisV != 0)
        {
            // キーが押された方向と現在地の差分
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;

            // アークタンジェントに第一：高さ、第二：底辺を与えると角度が出る(ラジアン値)
            float rad = Mathf.Atan2(dirY, dirX);
            // ラジアン値をオイラー値に変換
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            // 前フレームの角度情報そのまま
            angle = angleZ;
        }

        return angle;
    }
}
