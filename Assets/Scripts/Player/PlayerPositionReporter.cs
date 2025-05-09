using System;
using System.Collections;
using UnityEngine;

public class PlayerPositionReporter : MonoBehaviour
{
    public event Action<int, int> OnPlayerMove;

    Coroutine _coReportPlayerPos;

    void OnEnable()
    {
        if (_coReportPlayerPos != null)
        {
            StopCoroutine(_coReportPlayerPos);
            _coReportPlayerPos = null;
        }
        _coReportPlayerPos = StartCoroutine(CoReportPlayerPosition());

    }
    IEnumerator CoReportPlayerPosition()
    {
        while (true)
        {
            PlayerPostion((int)transform.position.x, (int)transform.position.y);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void PlayerPostion(int x, int y)
    {
        OnPlayerMove?.Invoke(x, y);
    }
}
