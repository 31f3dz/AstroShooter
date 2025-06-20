using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BGMのタイプ
public enum BGMType
{
    None, // なし
    Title, // タイトル
    InGame, // ゲーム中
    InBoss, // ボス戦
}

// SEのタイプ
public enum SEType
{
    GameClear, // ゲームクリア
    GameOver, // ゲームオーバー
    Shoot, // 発砲時
}

public class SoundController : MonoBehaviour
{
    public AudioClip bgmInTitle;
    public AudioClip bgmInGame;
    public AudioClip bgmInBoss;

    // 各SE
    public AudioClip meGameClear;
    public AudioClip meGameOver;
    public AudioClip seShoot;

    public static SoundController soundController; // 自分自身(ゲーム起動時から最初のSoundController)

    public static BGMType playingBGM = BGMType.None; // 再生中のBGM情報

    void Awake()
    {
        if (soundController == null)
        {
            soundController = this; // static変数に初めてのシーンのSoundControllerが代入される
            DontDestroyOnLoad(gameObject); // シーン切り替え先に自分自身を持っていく
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // BGMを変更する
    public void PlayBgm(BGMType type)
    {
        // メソッドの引数に与えたものが現在の曲情報と異なれば
        if (type != playingBGM)
        {
            playingBGM = type; // BGM変更
            AudioSource audio = GetComponent<AudioSource>();

            if (type == BGMType.Title)
            {
                audio.clip = bgmInTitle;
            }
            else if (type == BGMType.InGame)
            {
                audio.clip = bgmInGame;
            }
            else if (type == BGMType.InBoss)
            {
                audio.clip = bgmInBoss;
            }

            // AudioSourceのPlayメソッド
            audio.Play();
        }
    }

    public void StopBgm()
    {
        GetComponent<AudioSource>().Stop();
        playingBGM = BGMType.None;
    }

    public void SEPlay(SEType type)
    {
        if (type == SEType.GameClear)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameClear); // ゲームクリア
        }
        else if (type == SEType.GameOver)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameOver); // ゲームオーバー
        }
        else if (type == SEType.Shoot)
        {
            GetComponent<AudioSource>().PlayOneShot(seShoot); // 弾丸を発射
        }
    }
}
