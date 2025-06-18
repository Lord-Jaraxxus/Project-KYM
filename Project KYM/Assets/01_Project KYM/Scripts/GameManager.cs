using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // 싱글톤 인스턴스

    private int totalScore = 0; // 총 점수
    public ScoreUI scoreUI; // 점수 UI 컴포넌트

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddScore(int score)
    {
        totalScore += score; // 점수 추가
        scoreUI.UpdateScore(totalScore); // UI 업데이트
    }

}
