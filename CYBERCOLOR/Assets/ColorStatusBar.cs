using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ColorStatusBar : MonoBehaviour
{
    [SerializeField] private float _borderThickness;
    [SerializeField] private GameObject _barPrefab;
    [SerializeField] private List<RectTransform> _bars;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        float offset = 0;
        float barWidth = _rectTransform.rect.width;
        float barHeight = _rectTransform.rect.height;

        int totalFields = ScoreManager.Singleton.TotalFields;

        int i = 0;
        foreach (var entry in ScoreManager.Singleton.PlayerScores)
        {
            int coloredFields = entry.Value;
            var bar = _bars[i++];

            float width = (float)coloredFields / totalFields * barWidth;
            bar.sizeDelta = new(width, barHeight);
            bar.localPosition = new Vector3(offset, barHeight / 2f, 0);
            offset += width;
        }
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        var bar = CreateBar();
        _bars.Add(bar);
        bar.GetComponent<Image>().color = player.GetComponent<PlayerPainter>().PaintMaterial.color;
    }

    private RectTransform CreateBar()
    {
        var bar = Instantiate(_barPrefab, transform).GetComponent<RectTransform>();
        bar.pivot = new Vector2(0, 0.5f);
        bar.anchorMax = new Vector2(0, 0.5f);
        bar.anchorMin = new Vector2(0, 0.5f);
        bar.sizeDelta = Vector2.zero;
        return bar;
    }
}
