using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasScaler))]
public class CanvasAutofitScreen : MonoBehaviour
{
    private const float WIDTH_DEFAULT = 1080f;
    private const float HEIGHT_DEFAULT = 1920f;

    private float _currentWidth;
    private float _currentHeight;
    private CanvasScaler _canvasScaler;

    private float _originScale = 0.5f;

    private void Awake()
    {
        _currentWidth = GetComponent<RectTransform>().rect.width;
        _currentHeight = GetComponent<RectTransform>().rect.height;
        _canvasScaler = GetComponent<CanvasScaler>();
        ScaleScreen();

    }
    private void Update()
    {
        ScaleScreen();
    }
    private void ScaleScreen()
    {
        // float currentWidth = GetComponent<RectTransform>().rect.width;
        // float currentHeight = GetComponent<RectTransform>().rect.height;

        float ratioCurrent = _currentHeight / _currentWidth;
        float ratioDefault = HEIGHT_DEFAULT / WIDTH_DEFAULT;

        if (ratioCurrent > ratioDefault) _canvasScaler.matchWidthOrHeight = 0f;
        if (ratioCurrent < ratioDefault) _canvasScaler.matchWidthOrHeight = 1f;
        if (ratioCurrent == ratioDefault) _canvasScaler.matchWidthOrHeight = 0.5f;

    }
}
