using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// シリアライズ可とする→JSON化が可能になる
[System.Serializable]
public class SaveData
{
    public int arrangeId; // 識別ID
    public string tag; // タグ名

    // コンストラクタ
    public SaveData(string tag, int arrangeId)
    {
        this.tag = tag;
        this.arrangeId = arrangeId;
    }
}

// JSON化するためにリストをラッピングするクラス
[System.Serializable]
class Wrapper
{
    // SaveDataクラスをまとめたリストを扱えるクラス
    public List<SaveData> items;
}

public class SaveSystem
{
    // ゲーム状態をパソコンにセーブする
    public static void SaveGame()
    {
        // Player情報を獲得しておく
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 複製先となるリスト情報を宣言
        List<SaveData> dataList = new List<SaveData>();

        // 普段運用しているConsumedEventリストをdataListに複製
        foreach (var obj in SaveController.Instance.consumedEvent)
        {
            dataList.Add(new SaveData(obj.tag, obj.arrangeId));
        }

        Wrapper wrapper = new Wrapper(); // 設計したWrapperクラスの実体化
        wrapper.items = dataList; // JSON化に耐えうるかたちで再構築したdataListの中身をWrapperクラスのitemsリストに代入

        // 情報をJSON変換して文字列型の変数に格納
        string json = JsonUtility.ToJson(wrapper);

        // パソコンのライブラリに情報を記録
        PlayerPrefs.SetString("ConsumedJson", json); // 消費リストを文字列化(JSON化)した情報を特定のキーワードで保存

        PlayerPrefs.SetInt("GoldKey", GameController.hasGoldKey);
        PlayerPrefs.SetInt("SilverKey", GameController.hasSilverKey);
        PlayerPrefs.SetInt("Bullet", GameController.hasBullet);
        PlayerPrefs.SetInt("Life", PlayerController.hp);

        int lightCount = 0;
        if (LightController.getLight) lightCount = 1;
        PlayerPrefs.SetInt("Light", lightCount);

        // セーブしたときのシーン名の保存
        PlayerPrefs.SetString("SaveScene", SceneManager.GetActiveScene().name);

        PlayerPrefs.SetFloat("posX", player.transform.position.x);
        PlayerPrefs.SetFloat("posY", player.transform.position.y);

        PlayerPrefs.Save(); // ディスクへ反映させる ※明示的な行為
    }

    // ゲームを再開する
    public static void LoadGame()
    {
        // 復元先のリストをまっさらにして準備
        SaveController.Instance.consumedEvent.Clear();

        // PlayerPrefsを使ってパソコンからリスト情報(JSON)を入手
        string json = PlayerPrefs.GetString("ConsumedJson");
        if (!string.IsNullOrEmpty(json))
        {
            // 獲得したJSONをWrapperクラスに復元
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);

            // Wrapperクラスのitemsリストに復元された情報をconsumedEventリストに一つ一つ再現
            foreach (var item in wrapper.items)
            {
                SaveController.Instance.ConsumedEvent(item.tag, item.arrangeId);
            }
        }

        GameController.hasGoldKey = PlayerPrefs.GetInt("GoldKey");
        GameController.hasSilverKey = PlayerPrefs.GetInt("SilverKey");
        GameController.hasBullet = PlayerPrefs.GetInt("Bullet");
        PlayerController.hp = PlayerPrefs.GetInt("Life");

        int lightCount = PlayerPrefs.GetInt("Light");
        if (lightCount == 1) LightController.getLight = true;

        // シーン名の獲得
        string sceneName = PlayerPrefs.GetString("SaveScene");
        if (string.IsNullOrEmpty(sceneName)) sceneName = "Title";
        RoomController.isContinue = true; // コンティニューしたというフラグ
        SceneManager.LoadScene(sceneName);
    }
}
