using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//元となるクラス
[System.Serializable]
public class Message
{
    public string name;

    [TextArea(3, 10)] //最小3行、最大10行でstringを表示
    public string message;
}

//Messageクラスの配列を扱うが、ScriptableObjectとして実体化しなくても管理可能
//Assets上でMessageScriptsメニュー→MessageDataから生成可能
//（生成時のデフォルトファイル名はMsaData)
[CreateAssetMenu(fileName="MsgData",menuName ="MessageScripts/MessageData")]
public class MessageData : ScriptableObject
{
    public Message[] msgArray; //Messageクラスの配列
}
