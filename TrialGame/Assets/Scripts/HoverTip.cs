using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TipText;
    [SerializeField]
    private RectTransform TipWindow;
    [SerializeField]
    private string TipToShow;
    private float timer = 0.3f;
    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        HideTip();
    }

    private void OnMouseOver()
    {
        //distance check
        if (Vector3.Distance(transform.position, player.transform.position) < 3f){
            StartCoroutine(StartTimer());
        }
        else
        {
            StopAllCoroutines();
            HideTip();
        }
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();
        HideTip();
    }

    private void ShowMessage(string Tip, Vector2 MousePos)
    {
        TipText.text = Tip;
        TipWindow.sizeDelta = new Vector2(TipText.preferredWidth > 200 ? 200 : TipText.preferredWidth, TipText.preferredHeight);

        TipWindow.gameObject.SetActive(true);
        TipWindow.transform.position = new Vector2(MousePos.x + TipWindow.sizeDelta.x * 1f, MousePos.y);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timer);

        ShowMessage(TipToShow, Input.mousePosition);
    }

    private void HideTip()
    {
        TipText.text = default;
        TipWindow.gameObject.SetActive(false);
    }
}
