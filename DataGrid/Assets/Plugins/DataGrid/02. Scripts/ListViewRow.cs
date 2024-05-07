using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ListView Row Object
/// </summary>
public class ListViewRow : MonoBehaviour
{
    public List<GameObject> gameObjectItems;   // Columns of Row Gamebject. for example label, buttons...
    public bool useCrossBackgroundColor = true;

    [SerializeField] private GameObject content;

    [Header("Prefabs")]
    [SerializeField] private GameObject cellTextPrefab;
    [SerializeField] private GameObject cellButtonPrefab;

    private Image image;

    public int RowIndex
    {
        get => rowIndex;
        internal set
        {
            rowIndex = value;
            if (useCrossBackgroundColor == true && value % 2 == 0)
            {
                image.color = new Color(13 / 255, 13 / 255, 13 / 255);
            }
        }
    }

    private int rowIndex;
    private ListView parent;
    private bool initialized = false;

    private void Awake()
    {
        image = this.GetComponent<Image>();
    }

    private void Start()
    {
        //HorizontalLayoutGroup group = this.gameObject.GetComponent<HorizontalLayoutGroup>();
        //group.childControlHeight = false;
        //group.childControlWidth = false;

        // Add exist cells
        for (int i = 0; i < content.transform.childCount; i++)
        {
            // TODO : cell.cs 가 생기면 TryGetComponent()로 구조가 변경되어야함.
            Transform children = content.transform.GetChild(i);
            gameObjectItems.Add(children.gameObject);
        }
    }

    public void Init(ListView parent, object[] rowElements)
    {
        if (initialized == true)
        {
            Debug.LogWarning("두번 이상 ListViewRow를 초기화하려 했습니다.");
            return;
        }
        this.initialized = true;
        this.parent = parent;

        AddRows(rowElements);
    }

    public void AddRows(object[] rowElements)
    {
        for (int i = 0; i < rowElements.Length; i++)
        {
            ColumnInfo columnInfo = parent.Header.GetColumnInfo(i);
            if (columnInfo == null) return;

            switch (rowElements[i])
            {
                case string text:
                    {
                        GameObject textObject = Instantiate(cellTextPrefab, content.transform);
                        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
                        TMP_Text textComponent = textObject.transform.GetChild(0).GetComponent<TMP_Text>();
                        //rectTransform.sizeDelta = new Vector2(columnInfo.Width, rectTransform.sizeDelta.y);
                        rectTransform.sizeDelta = new Vector2(columnInfo.Width, (transform as RectTransform).rect.height);
                        textComponent.text = text;
                        gameObjectItems.Add(textObject);
                        break;
                    }

                case UnityAction a:
                    Console.Write("[버튼] | ");
                    break;

                case ValueTuple<string, UnityAction> tuple: // 명시적 타입 지정
                    {
                        GameObject buttonObject = Instantiate(cellButtonPrefab, content.transform);
                        Button button = buttonObject.transform.GetChild(0).GetComponent<Button>();
                        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
                        TMP_Text textComponent = buttonObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (transform as RectTransform).rect.height);
                        textComponent.text = tuple.Item1;
                        button.onClick.AddListener(tuple.Item2);
                        gameObjectItems.Add(buttonObject);
                        break;
                    }
                default:
                    Debug.LogError("Unsupported element type");
                    break;
            }
        }

        for (int i = 0; i < rowElements.Length; i++)
        {

        }
    }

    public void ChangeItemWidth(int cellIndex, float width)
    {
        if (cellIndex < gameObjectItems.Count)
        {
            RectTransform rectTransform = gameObjectItems[cellIndex].GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }
    }

}
