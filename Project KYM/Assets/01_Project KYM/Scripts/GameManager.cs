using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    private int totalScore = 0; // �� ����
    public ScoreUI scoreUI; // ���� UI ������Ʈ

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddScore(int score)
    {
        totalScore += score; // ���� �߰�
        scoreUI.UpdateScore(totalScore); // UI ������Ʈ
    }

}
