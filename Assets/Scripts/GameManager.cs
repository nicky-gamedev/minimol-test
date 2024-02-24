using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    private const int MAX_CUBES = 20;

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Bounds _spawningBounds;
    public List<CubeController> _cubes = new List<CubeController>();
    private Dictionary<int, int> _scoreRound = new Dictionary<int, int>();
    private List<int> _roundsInstantiated = new List<int>();
    private int _globalRound;

    public int GlobalRound => _globalRound;

    public event Action OnScoreUpdate = delegate {};
    public event Action<bool> OnGameEnd = delegate {};

    public int TotalScore
    {
        get
        {
            int total = _scoreRound.Values.Sum();
            return total;
        }
    }

    public List<CubeController> Cubes => _cubes;
    
    private bool _running;

    protected override void OnAwake()
    {
        _running = true;
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
        cube.name += _cubes.Count;
        _roundsInstantiated.Add(round);
    }

    public void AddScore(int amount, int round)
    {
        if (_running)
        {
            if (_scoreRound.ContainsKey(round)) _scoreRound[round] += amount;
            else _scoreRound.Add(round, amount);
            
            print($"Round {round} is now {_scoreRound[round]} points");
            if(_scoreRound[round] >= 10) EndGame(true);
            OnScoreUpdate();
        }
    }

    public void UpdateRound(int round)
    {
        if (round > _globalRound)
        {
            _globalRound = round;
        }
    }

    public void EndGame(bool isWin)
    {
        _running = false;
        print($"end game, score {TotalScore}");
        _inputReader.DisableInput();
        OnGameEnd(isWin);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _running = true;
        _cubes = new List<CubeController>();
        _scoreRound = new Dictionary<int, int>();
        _roundsInstantiated = new List<int>();
        _globalRound = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_spawningBounds.center, _spawningBounds.size);
    }
}
