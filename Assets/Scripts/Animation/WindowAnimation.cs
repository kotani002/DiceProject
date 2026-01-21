using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WindowAnimation
{
    [SerializeField]
    private GameObject window = null;
    [SerializeField]
    private Image bg = null;

    // 看板を下げる
    public async UniTask WindowShowDown()
    {
        bg.gameObject.SetActive(true);
        window.SetActive(true);

        bg.DOFade(endValue: 0.8f, duration: 0.75f).SetEase(Ease.InCubic);
        await window.transform.DOLocalMove(new Vector2(0f, 0f), 0.75f).SetEase(Ease.OutBounce);
    }

    // 看板を上げる
    public async UniTask WindowShowUp()
    {
        bg.DOFade(endValue: 0f, duration: 0.75f).SetEase(Ease.InCubic);
        await window.transform.DOLocalMove(new Vector2(0f, 1200f), 0.75f).SetEase(Ease.InCubic);
        bg.gameObject.SetActive(false);
        window.SetActive(false);
    }
}
