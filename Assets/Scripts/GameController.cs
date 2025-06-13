using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static string gameState; // ゲームの状態管理
    public static int hasBullet = 100; // 弾数
    public static int hasGoldKey; // 金の鍵の所持数
    public static int hasSilverKey; // 銀の鍵の所持数

    // Start is called before the first frame update
    void Start()
    {
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
