using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public Image arrowOne, arrowTwo;
    private bool isPanelOpen = false;
    public Text shopButtonText;

    public Button refreshButton;
    public Vector3 startPosition;

    private void Awake()
    {
        startPosition = this.transform.position;
    }

    public void OpenClosePanel()
    {
        isPanelOpen = !isPanelOpen;
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
}
