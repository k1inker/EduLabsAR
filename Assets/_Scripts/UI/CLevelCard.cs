using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CLevelCard : MonoBehaviour
{
    [SerializeField] private Button levelStartButton;
    [SerializeField] private int levelId;
    private void Awake()
    {
        this.levelStartButton.onClick.AddListener(OnLevelStartButton);
    }
    private void OnDestroy()
    {
        this.levelStartButton.onClick.RemoveListener(OnLevelStartButton);
    }
    private void OnLevelStartButton()
    {
        SceneManager.LoadScene(levelId);
    }
}