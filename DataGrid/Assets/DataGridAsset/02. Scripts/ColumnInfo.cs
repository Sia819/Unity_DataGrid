using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ColumnInfo : MonoBehaviour
{
    public int ColumnIndex = -1;

    [SerializeField] private TMP_Text Text;

    public ListView ListView { get; set; }

    public string Name
    {
        get => this.Text.text;
        set
        {
            this.Text.text = value;
            this.name = value;
        }
    }

    public float Width
    {
        get => (this.transform as RectTransform).rect.width;
        set
        {
            RectTransform rectTransform = (this.transform as RectTransform);
            rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
        }
    }

    public float FontSize
    {
        get => Text.fontSize;
        set => Text.fontSize = value;
    }

    public Color Color { get; set; } = Color.black;

}
