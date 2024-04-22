using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    
    public float Width { get; set; } = 110f;
    
    public float FontSize { get; set; } = 14f;
    
    public Color Color { get; set; } = Color.black;

}
