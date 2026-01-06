using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishController : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public LayerMask player;
    public int menu;

    void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(point1.position,4.5f,player) && Physics2D.OverlapCircle(point2.position, 4.5f, player))
        {
            SceneManager.LoadScene(menu);
        }
    }
}
