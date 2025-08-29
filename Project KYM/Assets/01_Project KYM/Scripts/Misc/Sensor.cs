using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{ 
    public class Sensor : MonoBehaviour
    {
        [SerializeField] private string targetTag; // 감지할 태그 설정
        [SerializeField] private LayerMask targetLayerMask; // 감지할 레이어 마스크 설정

        private void Awake()
        {
            // 초기화 작업이 필요하다면 여기에 작성
            if (string.IsNullOrEmpty(targetTag))
            {
                targetTag = transform.tag; // 현재 오브젝트의 태그를 사용, 아 이건 굳이같기도 한데 아닌가 싶기도 하고
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                // 감지된 오브젝트가 설정된 레이어 마스크에 포함되어 있는지 확인
                if (((targetLayerMask & (1 << other.gameObject.layer)) != 0) && other.CompareTag(targetTag)) // 비트연산자 무엇;; 아무튼 태그도 같이 봄니다
                {
                    // Debug.Log($"센서가 감지한 오브젝트 : {other.gameObject.name}");
                }
                else
                {
                    // Debug.Log("센서에 들어온 오브젝트와 태그 혹은 레이어가 다릅니다.");
                }
            }
        }
    }
}
