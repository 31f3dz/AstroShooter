using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Narrationt : MonoBehaviour
{
    public MessageData storyData;      // ScriptableObject（Messageクラスの配列mgsを持つMessgeDataクラス） の参照
    public TextMeshProUGUI personText; // 人物名
    public TextMeshProUGUI messageText; // メッセージテキスト

    private int currentLine = 0;　//現在何番目のMessageクラスを参照しているか
    public float charInterval = 0.05f;  // 1文字ごとの表示間隔

    public string sceneName; //シーン切替先


    private bool isTyping;　//文字表示中かどうか

    private Coroutine typingCoroutine; //時間差で発動

    private void Start()
    {
        //まずは最初のメッセージを表示
        if (storyData != null && storyData.msgArray.Length > 0)
        {
            //文字表示用のコルーチンTypeLineを発動
            //ストップしたい時に指定する用として、発生したコルーチン情報を変数typingCoroutineに持っておく
            typingCoroutine = StartCoroutine(TypeLine(storyData.msgArray[currentLine]));
        }
    }

    void Update()
    {
        //スペースが押された時
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //文字が表示途中
            if (isTyping)
            {
                // 表示途中にSpaceを押されたら即全表示
                StopCoroutine(typingCoroutine);
                messageText.text = storyData.msgArray[currentLine].message;
                isTyping = false;
            }
            //文字が表示済み
            else
            {
                //次のデータを取得
                currentLine++;
                //メッセージの配列の中身がまだあれば
                if (currentLine < storyData.msgArray.Length)
                {
                    //TypeLineコルーチンで次のメッセージを表示開始
                    //ストップしたい時に指定する用として、発生したコルーチン情報を変数typingCoroutineに持っておく
                    typingCoroutine = StartCoroutine(TypeLine(storyData.msgArray[currentLine]));
                }
                else
                {
                    //メッセージの配列の中身が無ければシーン切替のGoToStageコルーチン開始
                    StartCoroutine(GoToStage());
                }
            }
        }
    }

    //指定されたMessageデータ
    IEnumerator TypeLine(Message msg)
    {
        isTyping = true;

        personText.text = msg.name; //人物名を表示
        messageText.text = "";　//メッセージ内容はいったん空白

        //メッセージ内容を一文字ずつ配列として読み込み
        foreach (char c in msg.message)
        {
            messageText.text += c;　//一文字ずつ表示
            yield return new WaitForSeconds(charInterval); //変数charInterval待ちながら処理を繰り返す
        }

        //全部表示したら表示途中フラグをOFF
        isTyping = false;
    }

    //時間差でシーン切替を行うコルーチン
    IEnumerator GoToStage()
    {
        yield return new WaitForSeconds(3); //3秒待つ
        SceneManager.LoadScene(sceneName); //シーン切替
    }
}
