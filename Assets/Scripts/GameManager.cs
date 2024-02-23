using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    private const int MAX_CUBES = 20;

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Bounds _spawningBounds;
    [SerializeField] private List<CubeController> _cubes = new List<CubeController>();
    [SerializeField] private int _score;
    [SerializeField] private List<int> _roundsInstantiated = new List<int>();
    [SerializeField] private int _globalRound;

    public int GlobalRound => _globalRound;
    private bool _running;

    protected override void OnAwake()
    {
        _running = true;
    }

    private void Update()
    {
        if (_cubes.Count >= MAX_CUBES && _running)
        {
            EndGame(true);
        }
    }

    public void NewCube(int round)
    {
        if (_cubes.Count >= MAX_CUBES) return;
        if (_roundsInstantiated.Contains(round)) return;
        
        var target = new Vector3(
            Random.Range(_spawningBounds.min.x, _spawningBounds.max.x),
            Random.Range(_spawningBounds.min.y, _spawningBounds.max.y),
            Random.Range(_spawningBounds.min.z, _spawningBounds.max.z)
        );

        target = _spawningBounds.ClosestPoint(target);
        var cube = Instantiate(_cubePrefab, target, Quaternion.identity).GetComponent<CubeController>();
        _cubes.Add(cube);
        _roundsInstantiated.Add(round);
        _globalRound = round > _globalRound ? round : _globalRound;
    }

    public void AddScore(int amount)
    {
        if(_running) _score += amount;
    }

    public void EndGame(bool isWin)
    {
        _running = false;
        print($"end game, score {_score}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_spawningBounds.center, _spawningBounds.size);
    }
}
