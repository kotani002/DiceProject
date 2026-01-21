using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAudioClip
{
    /// <summary>
    /// 特定のフォルダ内にあるAudioClipを全て取得
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns>AudioClipが取得できない場合nullを返す</returns>
    public List<AudioClip> LoadAudioClipList(string filepath)
    {
        //フォルダのパスが送られてくるのでそのフォルダ内にあるAudioClipを全て読み込む
        var loadDataArray = Resources.LoadAll(filepath, typeof(AudioClip));

        if(loadDataArray.Length < 0)
        {
            return null;
        }

        //読み込んだAudioClipのファイルを返す
        var audioClipList = new List<AudioClip>();
        
        foreach(var loadData in loadDataArray)
        {
            audioClipList.Add((AudioClip)loadData);
        }
        return audioClipList;
    }
}
