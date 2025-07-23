using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour, IHittable
{
    public enum CanSize { Small, Large } // 캔 크기 정의
    public CanSize canSize = CanSize.Small; // 캔 크기 변수

    private bool isHit = false; // 캔이 충돌했는지 여부

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (isHit) { return; } // 이미 충돌한 경우 무시

    //    if (collision.gameObject.CompareTag("Bullet")) // 총알과 충돌했는지 확인
    //    {
    //        isHit = true; // 충돌 상태 설정
    //        int score = canSize == CanSize.Small ? 100 : 50; // 캔 크기에 따라 점수 설정
    //        GameManager.Instance.AddScore(score); // 점수 추가

    //        Debug.Log($"Can hit! Size: {canSize}, Score: {score}"); // 디버그 로그 출력
    //    }
    //}

    // IHittable 인터페이스의 OnHit 메서드 구현
    public void OnHit(int damage)
    {
        if (isHit) { return; } // 이미 충돌한 경우 무시

        isHit = true; // 충돌 상태 설정
        int score = canSize == CanSize.Small ? 100 : 50; // 캔 크기에 따라 점수 설정
        GameManager.Instance.AddScore(score); // 점수 추가

        Debug.Log($"Can hit! Size: {canSize}, Score: {score}"); // 디버그 로그 출력
    }

}
