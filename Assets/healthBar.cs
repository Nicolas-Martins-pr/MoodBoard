using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RawImage fullHealthImg;  // L'image de la barre de vie compl�te
    public RawImage emptyHealthImg; // L'image de la barre de vie vide
    public float healthScore = 100f; // Le score de sant� initial
    public GameObject player;

    private void Start()
    {
        // On r�cup�re les composants des images
        fullHealthImg = transform.Find("FullHealth").GetComponent<RawImage>();
        emptyHealthImg = transform.Find("EmptyHealth").GetComponent<RawImage>();
    }

    void Update()
    {
        // Calcul de la largeur de la barre de vie
        float healthWidth = healthScore / 100f * fullHealthImg.rectTransform.rect.width;

        // Mise � jour de l'image de la barre de vie pleine
        fullHealthImg.rectTransform.sizeDelta = new Vector2(healthWidth, fullHealthImg.rectTransform.rect.height);
        emptyHealthImg.rectTransform.sizeDelta = new Vector2(fullHealthImg.rectTransform.rect.width - healthWidth, emptyHealthImg.rectTransform.rect.height);

    }
}