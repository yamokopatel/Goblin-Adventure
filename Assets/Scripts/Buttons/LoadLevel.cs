using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public int level;

    public void Load()
    {
        SceneManager.LoadScene(level);
    }
}
