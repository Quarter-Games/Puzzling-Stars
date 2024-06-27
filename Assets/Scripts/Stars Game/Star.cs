using UnityEngine;
using UnityEngine.EventSystems;
public class Star : MonoBehaviour
{
    private static Star selectedStar;
    private static Star HoveredStar;
    private float timeMouseDown;
    private void OnMouseEnter()
    {
        Debug.Log("OnPointerEnter");
        if (selectedStar == null) return;
        if (selectedStar == this) return;

        HoveredStar = this;
        StarConnection.CreateConnection(new[] { selectedStar, HoveredStar });
        selectedStar = this;
        HoveredStar = null;
    }
    private void OnMouseExit()
    {
        if (!Input.GetMouseButton(0)) { selectedStar = null; HoveredStar = null; }
        if (HoveredStar == this)
        {
            HoveredStar = null;
        }
    }
    private void OnMouseDrag()
    {
        if (selectedStar == null)
        {
            Debug.Log("OnBeginDrag");
            selectedStar = this;
        }
    }
    private void OnMouseUp()
    {
        if (selectedStar != this) return;
        Debug.Log("OnEndDrag");
        if (HoveredStar != null && selectedStar != null)
        {
            StarConnection.CreateConnection(new[] { selectedStar, HoveredStar });
        }
    }
    private void OnMouseUpAsButton()
    {
        if (selectedStar != this) return;
        if (Time.time - timeMouseDown > 0.5f) return;
        StarConnection.ClearConnectionsFromStar(this);
    }
    private void OnMouseDown()
    {
        timeMouseDown = Time.time;
    }

}