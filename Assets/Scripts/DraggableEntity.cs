using UnityEngine;

public class DraggableEntity : MonoBehaviour
{
    private Vector3 oldPosition;
    private int oldSortingOrder;
    private Tile previousTile = null;
    
    public bool IsDragging = false;
    public BaseEntity baseEntity; 
    
    
    public void OnStartDrag()
    {
        if (GameManager.Instance.GetIsGameRunning())
        {
            return;
        }
        Debug.Log(this.name + " start dragging");
        oldPosition = this.transform.position;
        IsDragging = true;
        GameManager.Instance.SetIsDraggingEntity(true);
        GameManager.Instance.SetDraggingEntity(baseEntity);
    }
    
    public void OnDragging()
    {
        if (!IsDragging)
            return;
        
        Debug.Log(this.name + " dragging");

        transform.position = Input.mousePosition;
    }
    
    public void OnEndDrag(Tile tile)
    {
        if (!IsDragging)
            return;
        
        if (!TryRelease(tile))
        {
            //Nothing was found, return to original position.
            this.transform.position = oldPosition;
        }
        
        IsDragging = false;
        GameManager.Instance.SetIsDraggingEntity(false);
    }
    
    private bool TryRelease(Tile tile)
    {
        if (tile == null)
        {
            return false;
        }
        
        BaseEntity thisEntity = GameManager.Instance.getDraggingEntity();
        Node candidateNode = GridManager.Instance.GetNodeForTile(tile);
        if (candidateNode == null || thisEntity == null)
        {
            return false;
        }

        if (candidateNode.IsOccupied)
        {
            return false;
        }
        
        //Let's move this unity to that node
        thisEntity.CurrentNode.SetOccupied(false);
        thisEntity.SetCurrentNode(candidateNode);
        candidateNode.SetOccupied(true);
        thisEntity.transform.position = candidateNode.worldPosition;

        return true;
    }
}
