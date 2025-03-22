using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAtoB : MonoBehaviour
{
    [SerializeField]
    private Vector2[] wayPoints;    // 이동 지점 배열
    private int wayIndex = 0;       // 현재 이동 지점 인덱스

    private void OnEnable()
    {
        StartCoroutine("OnMoveLoop");
    }

    private void OnDisable()
    {
        StopCoroutine("OnMoveLoop");
    }

    private IEnumerator OnMoveLoop()
    {
        while (true)
        {
            yield return StartCoroutine("OnMoveTo");
        }
    }

    private IEnumerator OnMoveTo()
    {
        float current = 0;
        float percent = 0;

        // 시작위치는 항상 현재 위치, 목표위치는 wayPoints[]
        Vector2 start = transform.position;
        Vector2 end   = wayPoints[wayIndex];
        // 이동시간은 거리에 비례해서 증가할 수 있도록 설정 (1이동에 1초 소요)
        float time = Vector2.Distance(start, end);

        // Time 시간동안 플레이어가 start에서 end로 이동
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        // 다음 목표위치 설정
        wayIndex = wayIndex < wayPoints.Length - 1 ? wayIndex + 1 : 0;
    }
}
