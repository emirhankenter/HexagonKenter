using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.Elements
{
    public class FloatingText : TextMeshProUGUI
    {
        public void FadeAway(int score, float duration = 1)
        {
            text = $"+{score}";
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + 20, transform.position.z), duration).SetEase(Ease.OutCubic);
            this.DOColor(new Color(color.r, color.g, color.b, 0), duration).SetEase(Ease.InCubic).OnComplete(() => Destroy(gameObject));
        }
    }
}