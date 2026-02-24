using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject dinoSprite;
    [SerializeField] private GameObject frogSprite;
    
    private void Start()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        gameObject.SetActive(false);
        scoreCanvas.SetActive(true);
        Time.timeScale = 1;
    }

    public void ChooseDino()
    {
        dinoSprite.SetActive(true);
        ResumeGame();
    }

    public void ChooseFrog()
    {
        frogSprite.SetActive(true);
        ResumeGame();
    }
}
