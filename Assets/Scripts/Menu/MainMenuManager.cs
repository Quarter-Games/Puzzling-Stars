using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] float Distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    [ContextMenu("Align Menu Stars")]
    public void AlignMenuStars()
    {
        var objects = GameObject.FindGameObjectsWithTag("Menu Star");
        foreach (var obj in objects)
        {
            var pos = obj.transform.position;
            obj.transform.position = pos.normalized * Distance;

        }
    }
}
