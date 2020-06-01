using System;
using System.Collections;
using CameraLogic;
using Characters.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneInfo : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Camera _mainCamera;

    private CameraFollow _cameraFollower;

    private void Awake()
    {
        _cameraFollower = _mainCamera.GetComponent<CameraFollow>();
        StartCoroutine(WaitPlayer());
    }

    private IEnumerator WaitPlayer()
    {
        while (PlayerMainScript.MyPlayer == null) {
            yield return null;
        }

        _cameraFollower.targetBody = PlayerMainScript.MyPlayer.gameObject;
        _cameraFollower.enabled = true;
    }
    
}
