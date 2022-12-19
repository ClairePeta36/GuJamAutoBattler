using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color validColor;
    public Color wrongColor;
    public Renderer renderer;

    private void SetHighlight(bool active, bool valid)
    {
        if (active)
        {
            var colour = valid ? validColor : wrongColor;
            renderer.material.SetColor("_Color", colour);
        }
        else
        {
            renderer.material.SetColor("_Color", Color.white);
        }
    }

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.IsPurchasing)
        {
            return;
        }
        SetHighlight(true, !GridManager.Instance.GetNodeFromTile(this).IsOccupied);
    }

    private void OnMouseExit()
    {
        if (!GameManager.Instance.IsPurchasing)
        {
            return;
        }
        SetHighlight(false, !GridManager.Instance.GetNodeFromTile(this).IsOccupied);
    }
}