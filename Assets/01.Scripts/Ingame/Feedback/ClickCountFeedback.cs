//using TMPro;
//using UnityEngine;

//public class ClickCountFeedback : MonoBehaviour, IFeedback
//{
//    [SerializeField] private TextMeshProUGUI _textMesh;

//    private void Awake()
//    {
//        if (_textMesh == null)
//        {
//            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
//        }
//    }

//    public void Play(ClickInfo clickInfo)
//    {
//        GameManager.Instance.ClickCount++;
//        UpdateText();
//    }

//    private void UpdateText()
//    {
//        if (_textMesh != null)
//        {
//            _textMesh.text = GameManager.Instance.ClickCount.ToString();
//        }
//    }
//}
