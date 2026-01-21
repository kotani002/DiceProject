using System;
using UnityEngine;

//1.abstractは抽象クラスという概念
//インスタンス化ができない継承される目的で実装されるクラス
//抽象クラスの中に含まれるメソッドは継承先がオーバライドで呼び出す前提の作りにする

//2.クラス名<T>
//インスタンス化するときにクラスの型を決められるジェネリクスという概念
//クラスを呼び出したときに宣言した場合と別の型に変換したい場合に使う

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static GameObject _gameObject = null;
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Type t = typeof(T);

                _instance = (T)FindObjectOfType(t);
                if (_instance == null)
                {
                    _gameObject = new GameObject($"{t}");
                    DontDestroyOnLoad(_gameObject);
                    _instance = _gameObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    public virtual void DestroyInstance()
    {
        if(_instance != null) Destroy(_instance);
        if(_gameObject != null) Destroy(_gameObject);
    }

    public bool IsInstantiated => _instance != null;
}
