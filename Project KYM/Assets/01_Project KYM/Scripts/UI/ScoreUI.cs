using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text scoreText; // ���� �ؽ�Ʈ UI ������Ʈ

    private void Start()
    {
        // �ʱ� ���� ���� (��: 0��)
        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        // GameManager�� totalScore�� �����ͼ� UI�� ǥ��
        scoreText.text = "Score: " + score.ToString();
    }
}
