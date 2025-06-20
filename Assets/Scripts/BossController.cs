using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    // ヒットポイント
    public int hp = 10;

    // 反応距離
    public float reactionDistance = 7.0f;

    public GameObject ballPrefab; // 弾
    public float shootSpeed = 5.0f; // 弾の速度

    // 攻撃中フラグ
    bool inAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            // Playerのゲームオブジェクトを得る
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // プレイヤーとの距離チェック
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);
                if (dist <= reactionDistance && !inAttack)
                {
                    // 範囲内＆攻撃中ではない
                    inAttack = true;
                    // アニメーションを切り替える
                    GetComponent<Animator>().Play("BossAttack");
                }
                else if (dist > reactionDistance && inAttack)
                {
                    inAttack = false;
                    // アニメーションを切り替える
                    GetComponent<Animator>().Play("BossIdle");
                }
            }
            else
            {
                inAttack = false;
                // アニメーションを切り替える
                GetComponent<Animator>().Play("BossIdle");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // ダメージ
            hp--;
            if (hp <= 0)
            {
                // 死亡 当たりを消す
                GetComponent<CircleCollider2D>().enabled = false;
                // アニメーションを切り替える
                GetComponent<Animator>().Play("BossDeath");

                inAttack = false;

                // BGM停止
                SoundController.soundController.StopBgm();

                // クリアSEの再生
                SoundController.soundController.SEPlay(SEType.GameClear);

                // 1秒後に消す
                // Destroy(gameObject, 1);

                // ステータスを切り替える
                GameController.gameState = "gameclear";

                // 時間差でシーンが切り替わる
                Invoke("GameClear", 10);
            }
        }
    }

    // 攻撃
    void Attack()
    {
        // 発射口オブジェクトを取得
        Transform tr = transform.Find("Gate");
        GameObject gate = tr.gameObject;

        // 弾を発射するベクトルを作る
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            // アークタンジェント2関数で角度(ラジアン)を求める
            float rad = Mathf.Atan2(dy, dx);
            // ラジアンを度に変換して返す
            float angle = rad * Mathf.Rad2Deg;

            // Prefabから弾のゲームオブジェクトを作る(進行方向に回転)
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject ball = Instantiate(ballPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            Rigidbody2D rbody = ball.GetComponent<Rigidbody2D>(); // 発射
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }

    // ボス撃破後タイトルに戻す
    void GameClear()
    {
        SceneManager.LoadScene("Title");
    }
}
