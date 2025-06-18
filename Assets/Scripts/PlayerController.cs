using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; // Rgidbody2D
    Animator anime; // Animator

    float axisH, axisV; // 横軸、縦軸

    public float speed = 3.0f; // スピード
    public float angleZ = -90.0f; // 角度
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
        if (GameController.gameState != "playing" || inDamage) return;

        // モバイルからの入力がない場合のみ
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
        if (GameController.gameState != "playing") return;

        if (inDamage)
        {
            // 点滅処理
            float value = Mathf.Sin(Time.time * 50); // valueに正負の波を作る ※Time.timeはゲームの経過時間
            if (value > 0) GetComponent<SpriteRenderer>().enabled = true; // 絵を表示
            else GetComponent<SpriteRenderer>().enabled = false; // 絵を表示

            return;
        }

        // 斜めを縦・横に合わせる
        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;
    }

    void VectorAnime(float h, float v)
    {
        angleZ = GetAngle();

        // アニメ番号を一時記録
        int dir = direction;

        if (h == 0 && v == 0)
        {
            if (angleZ > -135 && angleZ < -45) dir = 0; // 下
            else if (angleZ >= -45 && angleZ <= 45) dir = 3; // 右
            else if (angleZ > 45 && angleZ < 135) dir = 1; // 上
            else dir = 2; // 左
        }
        else
        {
            // 左右キーが押されたら
            if (Mathf.Abs(h) >= Mathf.Abs(v))
            {
                if (h > 0) dir = 3;       // 右
                else if (h < 0) dir = 2;  // 左
            }
            else //左右キーが押されなかったら
            {
                if (v > 0) dir = 1;       // 上
                else if (v < 0) dir = 0;  // 下
            }
        }

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ダメージメソッドの発動
            GetDamage(collision.gameObject);
        }
    }

    // ダメージメソッド
    void GetDamage(GameObject enemy)
    {
        hp--; // 体力を減らす

        if (hp > 0)
        {
            // 動きが止まる
            rbody.velocity = Vector2.zero;

            // ノックバック(方角計算、AddForceで飛ぶ)
            // プレイヤー位置 - 敵の位置 の差分を正規化で整えて * 4.0で方向と力を調整
            Vector3 v = (transform.position - enemy.transform.position).normalized * 4.0f;
            rbody.AddForce(v, ForceMode2D.Impulse);

            // ダメージフラグをONにする ※硬直 FixedUpdateに影響が行く
            inDamage = true;

            // 時間差でダメージフラグをOFFに解除
            Invoke("DamageEnd", 0.25f);
        }
        else
        {
            // ゲームオーバー
            GameOver();
        }
    }

    // ダメージフラグを解除するメソッド
    void DamageEnd()
    {
        inDamage = false; // フラグ解除
        // プレイヤーの姿(SpriteRendererコンポーネント)を明確に表示状態にしておく
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void GameOver()
    {
        GameController.gameState = "gameover";
        // ゲームオーバー演出
        GetComponent<CircleCollider2D>().enabled = false; // コライダーなし
        rbody.velocity = Vector2.zero; // 動きを止める
        rbody.gravityScale = 1; // 重力発生
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 上に跳ね上げる
        anime.SetTrigger("death"); // 死亡アニメの開始
    }

    public void PlayerDestroy()
    {
        Destroy(gameObject);
    }
}
