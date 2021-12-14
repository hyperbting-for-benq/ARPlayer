using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private Color lineColor = Color.red;
    public Action<Color, Color> OnLineColorChanged;
    public Color LineColor
    {
        get => lineColor;
        set
        {
            if (value == lineColor)
                return;
            
            var old = lineColor;
            OnLineColorChanged?.Invoke(old, value);
        }
    }
    
    [SerializeField] private float lineLength = 1;
    public Action<int, int> OnLineLengthChanged;
    public int LineLength
    {
        get => (int)(lineLength * 100);
        set
        {
            var old = (int)(lineLength * 100);
            if (value == old)
                return;
            
            lineLength = ((float)value)/100;
            OnLineLengthChanged?.Invoke(old, value);
        }
    }

    [ContextMenu("Force Set")]
    private void ForceSetLineLength()
    {
        Debug.LogWarning($"ForceSetLineLength {lineLength}");
        var colors = new List<Color>() { Color.blue, Color.red, Color.cyan, Color.green};
        var colorIdx = Random.Range(0, colors.Count);
        Setup(
        Random.Range(0.5f, 3.0f),
            colors[colorIdx]
        );
    }

    #region Unity
    private void OnEnable()
    {
        OnLineLengthChanged += DefaultOnLineLengthChanged;
        OnLineColorChanged += DefaultOnLineColorChanged;
    }

    private void OnDisable()
    {
        OnLineLengthChanged -= DefaultOnLineLengthChanged;
        OnLineColorChanged -= DefaultOnLineColorChanged;
    }

    private void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
    }
    #endregion

    #region ZZRuler
    void SetYScale(float newYScale)
    {
        zzruler.transform.DOScaleY(newYScale, 1);
    }
    #endregion
    
    public void Setup(float lenFloat, Color color)
    {
        Setup(lenFloat);
        Setup(color);
    }

    public void Setup(Color color)
    {
        lineColor = color;
    }
    
    public void Setup(float lenFloat)
    {
        LineLength = (int)(lenFloat * 100);
    }
    
    #region Default
    [SerializeField] private LineRenderer m_LineRenderer;
    [SerializeField] private GameObject zzruler;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text uiTxt;
    private void DefaultOnLineLengthChanged(int oldIntVal, int newIntVal)
    {
        var newFloatVal = (float)newIntVal/100;
        
        // // Update LineRender 2nd point?
        // var poss = m_LineRenderer.GetPosition(1);
        // poss.y = newFloatVal;
        // m_LineRenderer.SetPosition(1, poss);

        var lrTra = m_LineRenderer.transform;
        lrTra.DOScaleY(newFloatVal, 1);
        
        SetYScale(newFloatVal);
        
        var rt = canvas.GetComponent<RectTransform>();
        rt.DOLocalMoveY(newFloatVal/2, 1);
        
        uiTxt.text = newFloatVal.ToString("N2") + "m";
    }
    
    private void DefaultOnLineColorChanged(Color oldClr, Color newClr)
    {
        var alpha = 1.0f;
        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(oldClr, 0.0f), new GradientColorKey(newClr, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        
        m_LineRenderer.colorGradient = gradient;
    }
    #endregion
}
