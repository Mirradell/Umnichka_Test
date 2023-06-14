using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    private float _startAspect = 1080f / 1920f;

    private float _defaultHeight;
    private float _defaultWidth;

    private void Awake()
    {
        _defaultHeight = Camera.main.orthographicSize;
        _defaultWidth = Camera.main.orthographicSize * _startAspect;

        Camera.main.orthographicSize = 5;
    }

}
