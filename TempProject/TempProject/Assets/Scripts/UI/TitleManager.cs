using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("씬 이동")]
    [SerializeField] private string _stage1SceneName = "Stage 1";

    [Header("패널 연결")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _creditPanel;
    [SerializeField] private GameObject _controlPanel;

    public void OnClickStart()
    {
        SceneManager.LoadScene(_stage1SceneName);
    }

    public void OnClickCredit()
    {
        _mainPanel.SetActive(false);
        _creditPanel.SetActive(true);
    }

    public void OnClickControl()
    {
        _mainPanel.SetActive(false);
        _controlPanel.SetActive(true);
    }

    public void OnClickBack()
    {
        _creditPanel.SetActive(false);
        _controlPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }
}