using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleEmperor.AudioData.Enums;
using System.Linq;

/// <summary>
/// クロスフェード入り、Sound-BGM-Manager
/// </summary>
public class MusicManager : MonoBehaviour
{
    private const float BGM_DEFAULT_VOLUME = 0.35f;
    private const float SE_DEFAULT_VOLUME = 0.7f;

    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("MusicManager");
                    _instance = obj.AddComponent<MusicManager>();

                    AudioSource[] auds = new AudioSource[2];
                    AudioSource[] audsSe = new AudioSource[1];

                    GameObject asBgmA = new GameObject("BGMAudioSourceA");
                    auds[0] = asBgmA.AddComponent<AudioSource>();
                    auds[0].volume = BGM_DEFAULT_VOLUME;
                    asBgmA.transform.SetParent(obj.transform);

                    GameObject asBgmB = new GameObject("BGMAudioSourceB");
                    auds[1] = asBgmB.AddComponent<AudioSource>();
                    auds[1].volume = BGM_DEFAULT_VOLUME;
                    asBgmB.transform.SetParent(obj.transform);

                    GameObject asSe = new GameObject("SEAudioSource");
                    audsSe[0] = asSe.AddComponent<AudioSource>();
                    audsSe[0].volume = SE_DEFAULT_VOLUME;
                    asSe.transform.SetParent(obj.transform);

                    _instance.SetBGMAudioSource(auds);
                    _instance.SetSEAudioSource(audsSe);
                }
            }
            return _instance;
        }
    }

    //音声情報を管理する連想配列
    private Dictionary<string, AudioData> _audioDataDictionary = null;

    [SerializeField] private AudioSource[] _asBgmPair = new AudioSource[2];
    private bool _currentAudioSourceIsIndexZero = false;
    private AudioSource _nextAs => _currentAudioSourceIsIndexZero ? _asBgmPair[1] : _asBgmPair[0];
    private AudioSource _currentAs => _currentAudioSourceIsIndexZero ? _asBgmPair[0] : _asBgmPair[1];

    // SE用AudioSource
    [SerializeField]
    private AudioSource[] _asSePool = new AudioSource[1];
    // SE用のAudioSourceを管理するオブジェクト

    [SerializeField]
    private float _fadeInTimeSec = 2f;
    [SerializeField]
    private float _fadeOutTimeSec = 2f;
    private float _bgmDefaultVolume = 0f;

    private Coroutine _fadeInCoroutine = null;
    private Coroutine _fadeOutCoroutine = null;

    //Const用のファイルにまとめる予定
    private const string AUDIO_FILE_PATH = "Audio";
    private const string AUDIO_MASTER_DATA_PATH = "AudioData/AudioMaster";

    //
    [SerializeField]
    private AudioData audioData = null;

    void Start()
    {
        _bgmDefaultVolume = _nextAs.volume;

        // 乱数の種（シード値）を設定する
        // 現在時刻のミリ秒をシードにして、毎回違う乱数の回答がでるようにする
        UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
    }

    public IEnumerator ChangeBGM_Cor(AudioSource beOutAs, AudioSource beInAs, string key)
    {
        // FadeOutをさせる際に、フェイドインが残っていたら、処理を破棄する
        if (_fadeInCoroutine != null) _fadeInCoroutine = null;
        _fadeOutCoroutine = StartCoroutine(FadeOut_Cor(beOutAs));

        // 流すAudioClipの取得
        AudioData audioData = GetAudioData(key);
        if (audioData == null)
        {
            yield return null;
        }

        // 音源のロードが行えるまで待機
        yield return new WaitUntil(() =>
        audioData.audioClip.loadState != AudioDataLoadState.Loading
        );

        // クロスフェード開始
        beInAs.clip = audioData.audioClip;
        beInAs.Play();
        beInAs.volume = 0f;
        _fadeInCoroutine = StartCoroutine(FadeIn_Cor(beInAs));

        _currentAudioSourceIsIndexZero = !_currentAudioSourceIsIndexZero;

        yield return new WaitForSeconds(_fadeOutTimeSec);

        beOutAs.Stop();
        beOutAs.volume = BGM_DEFAULT_VOLUME;
        _fadeOutCoroutine = null;
    }

    private IEnumerator FadeOut_Cor(AudioSource audioSource)
    {
        float frame = 0f;
        float startFadeOutVolume = _currentAs.volume;
        while (audioSource.volume > 0f)
        {
            frame += Time.deltaTime;

            float per = frame / _fadeOutTimeSec;
            if (per < 0f) per = 0f;

            float v = Mathf.Lerp(startFadeOutVolume, 0f, per);
            audioSource.volume = v;

            yield return null;
        }
    }

    private IEnumerator FadeIn_Cor(AudioSource audioSource)
    {
        float frame = 0f;
        while (audioSource.volume < _bgmDefaultVolume)
        {
            frame += Time.deltaTime;

            float per = frame / _fadeInTimeSec;
            if (per > 1f) per = 1f;

            float v = Mathf.Lerp(0f, _bgmDefaultVolume, per);
            audioSource.volume = v;

            yield return null;
        }
    }

    /// <summary>
    /// SE用のAudioSource
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private IEnumerator PlaySE_Cor(string key)
    {
        AudioData audioData = GetAudioData(key);
        if (audioData == null)
        {
            yield return null;
        }
        //未動作のAudioSourceを取得
        AudioSource asSe = _asSePool.FirstOrDefault(asSe => asSe.isPlaying == false);

        //新しいAudioSourceを作成、プールに追加
        if (asSe == null)
        {
            asSe = _asSePool[0].gameObject.AddComponent<AudioSource>();
            _asSePool = _asSePool.Append(asSe).ToArray();
        }
        //再生
        asSe.clip = audioData.audioClip;
        asSe.Play();
        asSe.volume = SE_DEFAULT_VOLUME;
        float sePlayTime = audioData.audioClip.length;
        yield return new WaitForSeconds(sePlayTime);
        asSe.Stop();
    }

    private AudioData GetAudioData(string key)
    {
        if (_audioDataDictionary.TryGetValue(key, out AudioData audioData) == false)
        {
            //流すAudioClipが見つからなった場合
            Debug.LogError($"key名:{name}の音源が見つかりませんでした");
            return null;
        }
        return audioData;
    }

    public void SetBGMAudioSource(AudioSource[] aus)
    {
        _asBgmPair = aus;
    }

    public void SetSEAudioSource(AudioSource[] aus)
    {
        _asSePool = aus;
    }

    public void ChangeBGM(string key)
    {
        //完全にBGMがフェードアウトされていない場合、次のBGMの再生を行わない
        if (_fadeOutCoroutine != null)
        {
            return;
        }
        StartCoroutine(ChangeBGM_Cor(_nextAs, _currentAs, key));
    }
    public void PlaySE(string key)
    {
        StartCoroutine(PlaySE_Cor(key));
    }
}
