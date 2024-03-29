
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeController : MonoBehaviour
{
    public enum CubeFace: sbyte {
        Back   = -3,  // -z
        Bottom = -2,  // -y
        Left   = -1,  // -x
        None   =  0,
        Right  =  1,  // +x
        Top    =  2,  // +y
        Front  =  3   // +z
    }
    
    [System.Serializable]
    struct MinMax
    {
        public float min;
        public float max;
    }
    
    [Header("References")]
    [SerializeField] private Rigidbody _rb;

    [Header("Parameters")] 
    [SerializeField] private MinMax jumpForce;
    [SerializeField] private MinMax torqueForce;

    private CubeFace _lastLocal;
    private bool _ignoreNextContact;
    private bool _canJump;
    public int _localRound;

    private void Awake()
    {
        _lastLocal = FaceInDirection(Vector3.forward);
        _ignoreNextContact = true;
    }

    public void Jump()
    {
        if (!_canJump) return;
        _rb.AddForce(Random.Range(jumpForce.min, jumpForce.max) * Vector3.up);
        _rb.AddTorque(Random.Range(torqueForce.min, torqueForce.max) * Vector3.right);
        _localRound = GameManager.Instance.GlobalRound;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Plane"))
        {
            StartCoroutine(FaceChecker());
        }
        else if (other.gameObject.CompareTag("GameOver"))
        {
            GameManager.Instance.EndGame(false);
        }
        _ignoreNextContact = false;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Plane"))
        {
            _canJump = false;
        }
    }

    IEnumerator FaceChecker()
    {
        while (!_rb.IsSleeping()) yield return null;
        var local = FaceInDirection(Vector3.forward);
        if (_lastLocal != local && !_ignoreNextContact)
        {
            GameManager.Instance.AddScore(1, _localRound);
            GameManager.Instance.NewCube(_localRound);
        }
        _localRound = GameManager.Instance.GlobalRound + 1;
        GameManager.Instance.UpdateRound(_localRound);
        _lastLocal = local;
        _canJump = true;
    }
    
    public CubeFace FaceInDirection(Vector3 worldDirection) {
        var local = transform.InverseTransformDirection(worldDirection);
        int code = Mathf.RoundToInt(local.x + 2 * local.y + 3 * local.z);
        return (CubeFace)code;
    }
}
