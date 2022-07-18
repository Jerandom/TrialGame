using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private RectTransform PopupWindow;

    private void Start()
    {
        PopupWindow.gameObject.SetActive(true);
    }

    private void Update()
    {
        startButton.onClick.AddListener(() =>
        {
            PopupWindow.gameObject.SetActive(false);
        });
    }
}
