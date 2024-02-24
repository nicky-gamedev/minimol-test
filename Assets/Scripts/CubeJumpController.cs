using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeJumpController : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private InputReader _inputReader;

    private Coroutine _coroutine;

    private void Awake()
    {
        _inputReader.JumpEvent += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _inputReader.JumpEvent -= OnJumpPerformed;
    }

    private void OnJumpPerformed()
    {
        var cubeList = GameManager.Instance.Cubes;
        if(_coroutine == null)
            _coroutine = StartCoroutine(PerformJump(cubeList));
    }

    IEnumerator PerformJump(List<CubeController> cubeList)
    {
        for (var index = 0; index < cubeList.Count; index++)
        {
            cubeList[index].Jump();
            yield return new WaitForSeconds(_delay);
        }

        _coroutine = null;
    }
}
