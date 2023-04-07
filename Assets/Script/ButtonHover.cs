using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    private Button button;
    private Image image;
    private Color originalColor;
    public Color hoverColor;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        originalColor = image.color;

            Button quitButton = GetComponent<Button>();
            quitButton.onClick.AddListener(QuitGame);
        }

        void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    

    public void OnPointerEnter()
    {
        Debug.Log("çaentreee");
        image.color = hoverColor;
    }

    public void OnPointerExit()
    {
        Debug.Log("çasooortttt");
        image.color = originalColor;
    }
}