using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public string sceneName; // ゲームスタートのシーン名
    public Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        // セーブデータの保存があるか
        string lastScene = PlayerPrefs.GetString("SaveScene");
        // なければコンティニューボタンを無効化
        if (lastScene == "") continueButton.interactable = false;

        // タイトル曲を流す
        SoundController.soundController.PlayBgm(BGMType.Title);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // データ削除専用のメソッド
    public void SaveReset()
    {
        // セーブデータの消去
        PlayerPrefs.DeleteAll();
        continueButton.interactable = false;
    }

    public void GameStart()
    {
        PlayerPrefs.DeleteAll(); // 全消し
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueStart()
    {
        SaveSystem.LoadGame();
    }
}
