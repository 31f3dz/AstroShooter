using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public static bool getLight; // ライトを入手しているかどうかのフラグ
    public static bool onLight; // ライトスイッチのON/OFF
    public GameObject playerLight; // ライトのオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        playerLight.SetActive(onLight);
    }

    // Update is called once per frame
    void Update()
    {
        if (!getLight) return; // ライトを取得していなければ何もしない

        if (Input.GetKeyDown(KeyCode.L))
        {
            onLight = !onLight; // ライトのスイッチをON/OFF切り替え
            playerLight.SetActive(onLight); // オブジェクトが連動して表示/非表示
        }
    }
}
