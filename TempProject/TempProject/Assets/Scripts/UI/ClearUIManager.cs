using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUIManager : MonoBehaviour
{
    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title Scene");
    }
}