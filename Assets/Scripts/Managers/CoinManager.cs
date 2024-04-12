using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private List<Coin> m_coins;
    [SerializeField] private List<Tween> m_tweens;

    public void Initialize()
    {
        m_coins = FindObjectsOfType<Coin>().ToList();
        m_tweens = new List<Tween>();
        
        m_coins.ForEach(x =>
        {
            Tween rotateTween = x.transform.DOLocalRotate(new Vector3(0, 360, 0), 2, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental);
            x.Initialize(rotateTween, this);
            m_tweens.Add(rotateTween);
        });
    }

    public void CollectCoin(Coin coin)
    {
        coin.transform.DOScale(new Vector3(0, 0, 0), .5f).OnComplete(() =>
        {
            m_tweens.Find(x=> x.Equals(coin.RotateTween)).Kill();
            Destroy(coin.gameObject);
        });
    }
}