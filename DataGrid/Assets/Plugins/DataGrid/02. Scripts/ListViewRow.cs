using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace UIExtension.ListView
{
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

        [Header("Sprites")]
        [SerializeField] private Sprite blueUI;
        [SerializeField] private Sprite redUI;
        [SerializeField] private Sprite grayUI;

        private Image image;

        public int RowIndex
        {
            get => rowIndex;
            internal set
            {
                rowIndex = value;
                if (useCrossBackgroundColor == true && value % 2 == 0)
                {
                    image.color = new Color(37 / 255f, 37 / 255f, 37 / 255f);
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
                switch (rowElements[i])
                {
                    case string text:
                        AddText(text, i);
                        break;

                    case int number:
                        AddText(number.ToString(), i);
                        break;

                    case UnityAction action:
                        AddButton("버튼", action);
                        break;

                    case (string text, UnityAction action):
                        AddButton(text, action);
                        break;

                    case (string text, UnityAction action, ButtonColor color):
                        AddButton(text, action, color);
                        break;

                    default:
                        Debug.LogError($"Unsupported element type: {rowElements[i].GetType()}");
                        break;
                }
            }
        }

        private void AddText(string text, int index)
        {
            GameObject textObject = Instantiate(cellTextPrefab, content.transform);
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            TMP_Text textComponent = textObject.transform.GetChild(0).GetComponent<TMP_Text>();
            ColumnInfo columnInfo = parent.Header.GetColumnInfo(index);

            if (columnInfo != null)
            {
                rectTransform.sizeDelta = new Vector2(columnInfo.Width, (transform as RectTransform).rect.height);
                textComponent.text = text;
                gameObjectItems.Add(textObject);
            }
            else
            {
                Debug.LogError("ColumnInfo is null");
            }
        }

        private void AddButton(string buttonText, UnityAction action, ButtonColor? buttonColor = null)
        {
            GameObject buttonObject = Instantiate(cellButtonPrefab, content.transform);
            Button button = buttonObject.transform.GetChild(0).GetComponent<Button>();
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            TMP_Text textComponent = buttonObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (transform as RectTransform).rect.height);
            textComponent.text = buttonText;
            button.onClick.AddListener(action);
            gameObjectItems.Add(buttonObject);

            if (buttonColor.HasValue)
            {
                switch (buttonColor)
                {
                    case ButtonColor.Blue:
                        buttonObject.transform.GetChild(0).GetComponent<Image>().sprite = blueUI;
                        break;
                    case ButtonColor.Red:
                        buttonObject.transform.GetChild(0).GetComponent<Image>().sprite = redUI;
                        break;
                    case ButtonColor.Gray:
                        buttonObject.transform.GetChild(0).GetComponent<Image>().sprite = grayUI;
                        break;
                    default:
                        break;
                }
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

        public enum ButtonColor
        {
            Blue,
            Red,
            Gray
        }
    }
}