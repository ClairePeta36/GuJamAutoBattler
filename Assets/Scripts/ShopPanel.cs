using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public Image arrowOne, arrowTwo;
    private bool isPanelOpen = false;
    public Text shopButtonText;
    public Vector3 startPosition;
    public CardShop cardShop;
    public Text refreshCost;

    private void Awake()
    {
        startPosition = this.transform.position;
        refreshCost.text = cardShop.refreshCost.ToString();
        isPanelOpen = true;
        MovePanel(isPanelOpen);
    }

    public void OpenClosePanel()
    {
        isPanelOpen = !isPanelOpen;
        MovePanel(isPanelOpen);
    }

    public void ClosePanelOnGameStart()
    {
        isPanelOpen = false;
        MovePanel(isPanelOpen);
    }

    private void MovePanel(bool isOpen)
    {
        if (isOpen)
        {
            arrowOne.transform.Rotate(0, 0, 180);
            arrowTwo.transform.Rotate(0, 0, 180);
            shopButtonText.text = "CLOSE SHOP";
            this.gameObject.transform.position = startPosition + new Vector3(180, 0, 0);
        }
        else
        {
            arrowOne.transform.Rotate(0, 0, 180);
            arrowTwo.transform.Rotate(0, 0, 180);
            shopButtonText.text = "OPEN SHOP";
            this.gameObject.transform.position = startPosition;
        }
    }

    public void OnRefreshClick()
    {
        cardShop.OnRefreshClick();
    }
}
