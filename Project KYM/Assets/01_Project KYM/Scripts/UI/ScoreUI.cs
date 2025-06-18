using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text scoreText; // 점수 텍스트 UI 컴포넌트

    private void Start()
    {
        // 초기 점수 설정 (예: 0점)
        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        // GameManager의 totalScore를 가져와서 UI에 표시
        scoreText.text = "Score: " + score.ToString();
    }
}
