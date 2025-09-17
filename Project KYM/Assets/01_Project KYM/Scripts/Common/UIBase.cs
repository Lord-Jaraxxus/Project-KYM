using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public abstract class UIBase : MonoBehaviour    // 추상 클래스, 부모로만 동작
    {
        public virtual bool IsDepthUI { get; } = false; // UI가 Depth UI인지 여부 (기본값은 false)

        public virtual void Show() // UI를 보여주는 메서드
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide() // UI를 숨기는 메서드
        {
            gameObject.SetActive(false);
        }
    }
}
