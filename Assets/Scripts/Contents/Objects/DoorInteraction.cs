using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class DoorInteraction : InteractionBase
{
    [Header("Spawn TO")]
    [SerializeField] SceneField _sceneToLoad;
    [SerializeField] DoorToSpawnAt _doorToSpawnTo;

    [Space(10f)]
    [Header("This Door")]
    [SerializeField] public DoorToSpawnAt _currentDoorPosition;

    public override void Interact()
    {
       SceneChangeManager.ChangeSceneFromDoorUse(_sceneToLoad,_doorToSpawnTo);
    }
}
