using System;
using System.Collections;
using System.Collections.Generic;
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

    #region MyRegion
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
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text uiTxt;
    private void DefaultOnLineLengthChanged(int oldIntVal, int newIntVal)
    {
        var newFloatVal = (float)newIntVal/100;
        
        var poss = m_LineRenderer.GetPosition(1);
        poss.y = newFloatVal;
        m_LineRenderer.SetPosition(1, poss);
        
        var rt = canvas.GetComponent<RectTransform>();
        var start = rt.position;
        start.y = newFloatVal/2;
        rt.position = start;
        
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
