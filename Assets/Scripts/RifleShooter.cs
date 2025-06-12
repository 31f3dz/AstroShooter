using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : MonoBehaviour
{
    public GameObject riflePrefab; // 生成されるべきライフル
    GameObject rifleObj; // 生成されたライフルオブジェの情報
    PlayerController playerCnt;

    public GameObject bulletPrefab; // 生成されるべき弾丸
    public float shootSpeed = 12.0f; // 弾丸の速度
    public float shootDelay = 0.25f; // 発射間隔
    bool inAttack; // 攻撃中かどうかのフラグ

    Transform gate; // ライフルの子オブジェクトの発射口

    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerPos = transform.position; // Playerの位置
        // ライフルを生成しつつrifleObjに情報を格納
        rifleObj = Instantiate(riflePrefab, playerPos, Quaternion.identity);
        rifleObj.transform.SetParent(transform); // 生成したライフルはPlayerの子オブジェクトになる
        gate = rifleObj.transform.Find("Gate"); // ライフルの子オブジェクトのGateという名前のTransform情報を格納

        playerCnt = GetComponent<PlayerController>(); // PlayerControllerを取得
    }

    // Update is called once per frame
    void Update()
    {
        // ライフルの回転とZ軸の優先順位
        float rifleZ = -1; // 基本は手前に写るように値をセッティング

        if (playerCnt.angleZ > 45 && playerCnt.angleZ < 135)
        {
            rifleZ = 1; // Playerが上向きのときはライフルは奥に写るようにセッティング
        }

        // ライフルをangleZに合わせて常に回転
        rifleObj.transform.rotation = Quaternion.Euler(0, 0, playerCnt.angleZ + 90);

        // ライフルのZ座標の位置を調整
        rifleObj.transform.position = new Vector3(transform.position.x, transform.position.y, rifleZ);

        if (Input.GetKeyDown(KeyCode.R)) Attack(); // キーが押されたら弾丸発射
    }

    // 攻撃(弾丸発射メソッド)
    void Attack()
    {
        // 既に攻撃中であれば何もしない
        if (inAttack) return;

        inAttack = true; // 攻撃中フラグをON

        float angleZ = playerCnt.angleZ; // Playerの角度を取得
        Quaternion bulletRotate = Quaternion.Euler(0, 0, angleZ); // 生成する弾丸の角度をセッティング
        GameObject bulletObj = Instantiate(bulletPrefab, gate.position, bulletRotate); // 生成してbulletObjに情報を格納

        // angleZと三角関数を使ってVector3のためのx成分とy成分を作っていく
        float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); // 角度(ラジアン値)を与える
        float y = Mathf.Sin(angleZ * Mathf.Deg2Rad); // 角度(ラジアン値)を与える
        Vector3 v = new Vector3(x, y).normalized * shootSpeed; // 向かうべき方向(正規化)と弾速をかけた値が最終的な方向データ ※Zは省略→0が入る

        // 生成した弾丸のRigidbody2D情報を獲得
        Rigidbody2D bulletRigid = bulletObj.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(v, ForceMode2D.Impulse);

        // 攻撃中フラグをOFFにするメソッドを時間差で発動
        Invoke("StopAttack", shootDelay);
    }

    // 攻撃中フラグをOFFにするメソッド
    void StopAttack()
    {
        inAttack = false;
    }
}
