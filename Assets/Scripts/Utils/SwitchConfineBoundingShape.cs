using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += SwitchBoundingShape;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SwitchBoundingShape;
    }

    private void SwitchBoundingShape(Scene scene, LoadSceneMode mode)
    {
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(TAG_BOUNDS_CONFINER).GetComponent<PolygonCollider2D>();

        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

        cinemachineConfiner.InvalidatePathCache();
    }
}
