using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class Star : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D StarCollider;
    public static Star selectedStar;
    private static Star HoveredStar;
    private float timeMouseDown;
    private void OnMouseEnter()
    {
        //Debug.Log("OnPointerEnter");
        if (selectedStar == null) return;
        if (selectedStar == this) return;

        HoveredStar = this;
        var temp = selectedStar;
        selectedStar = this;
        StarConnection.CreateConnection(new[] { temp, HoveredStar });
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
            //Debug.Log("OnBeginDrag");
            selectedStar = this;
        }
    }
    private void OnMouseUp()
    {
        if (selectedStar != this) return;
        //Debug.Log("OnEndDrag");
        if (HoveredStar != null && selectedStar != null)
        {
            StarConnection.CreateConnection(new[] { selectedStar, HoveredStar });
        }
        else if (selectedStar!=null) selectedStar = null;
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
    public void Disable()
    {
        selectedStar = null;
        HoveredStar = null;
        spriteRenderer.color = Color.gray;
        StarCollider.enabled = false;
        enabled = false;
    }
    public List<ConnectionDef> ContainedIn()
    {
        return StarConnection.connections.Where(c => c.ConnectedStars.Contains(this)).Select(c => c.ConnectedStars).ToList();
    }

}