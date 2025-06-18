using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour, IHittable
{
    public enum CanSize { Small, Large } // ĵ ũ�� ����
    public CanSize canSize = CanSize.Small; // ĵ ũ�� ����

    private bool isHit = false; // ĵ�� �浹�ߴ��� ����

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (isHit) { return; } // �̹� �浹�� ��� ����

    //    if (collision.gameObject.CompareTag("Bullet")) // �Ѿ˰� �浹�ߴ��� Ȯ��
    //    {
    //        isHit = true; // �浹 ���� ����
    //        int score = canSize == CanSize.Small ? 100 : 50; // ĵ ũ�⿡ ���� ���� ����
    //        GameManager.Instance.AddScore(score); // ���� �߰�

    //        Debug.Log($"Can hit! Size: {canSize}, Score: {score}"); // ����� �α� ���
    //    }
    //}

    // IHittable �������̽��� OnHit �޼��� ����
    public void OnHit(int damage)
    {
        if (isHit) { return; } // �̹� �浹�� ��� ����

        isHit = true; // �浹 ���� ����
        int score = canSize == CanSize.Small ? 100 : 50; // ĵ ũ�⿡ ���� ���� ����
        GameManager.Instance.AddScore(score); // ���� �߰�

        Debug.Log($"Can hit! Size: {canSize}, Score: {score}"); // ����� �α� ���
    }

}
