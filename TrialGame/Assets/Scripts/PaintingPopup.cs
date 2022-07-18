using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PaintingPopup : MonoBehaviour
{
    [SerializeField]
    private Button goButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private RectTransform PopupWindow;
    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PopupWindow.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        //distance check
        if (Vector3.Distance(transform.position, player.transform.position) < 3f)
        {
            ShowMessage();
        }
        ButtonInteraction();
    }

    private void Update()
    {
        //distance check
        if (Vector3.Distance(transform.position, player.transform.position) > 3f)
        {
            HideMessage();
        }
    }

    private void ShowMessage()
    {
        PopupWindow.gameObject.SetActive(true);
    }

    private void HideMessage()
    {
        PopupWindow.gameObject.SetActive(false);
    }

    private void ButtonInteraction()
    {
        backButton.onClick.AddListener(() =>
        {
            PopupWindow.gameObject.SetActive(false);
        });

        goButton.onClick.AddListener(() =>
        {
            PopupWindow.gameObject.SetActive(false);
            Application.OpenURL("https://www.sgpbusiness.com/company/Web3re-Technologies-Pte-Ltd");
        });
    }
}
