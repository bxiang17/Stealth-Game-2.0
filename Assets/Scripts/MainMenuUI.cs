using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayClick);
    }

    private void PlayClick()
    {
        SceneManager.LoadScene(1);
    }
}
