using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChangeColorOnContact : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    private void Start()
    {
        GameManager.Instance.OnScoreUpdate += ChangeColor;
    }

    public void ChangeColor()
    {
        //Generating only saturated colors for the plane
        float[] rgb = { Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f) };
        int max, min;
        if (rgb[0] > rgb[1])
        {
            max = (rgb[0] > rgb[2]) ? 0 : 2;
            min = (rgb[1] < rgb[2]) ? 1 : 2;
        }
        else
        {
            max = (rgb[1] > rgb[2]) ? 1 : 2;
            int notmax = 1 + max % 2;
            min = (rgb[0] < rgb[notmax]) ? 0 : notmax;
        }

        rgb[max] = 1f;
        rgb[min] = 0f;
        
        _renderer.material.color = new Color(rgb[0], rgb[1], rgb[2]);
    }
}
