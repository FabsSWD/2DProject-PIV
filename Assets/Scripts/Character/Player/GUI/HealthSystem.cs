using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;
    public Image currentHealthGlobe;
    protected CharacterHealth characterHealth;

    void Awake()
    {
        Instance = this;
        characterHealth = FindObjectOfType<CharacterHealth>();
    }
    
    void Start()
    {
        UpdateGraphics();
    }

    private void UpdateHealthGlobe()
    {
        if (characterHealth == null || currentHealthGlobe == null)
            return;

        float ratio = (float)characterHealth.currentHealth / characterHealth.maxHealth;
        currentHealthGlobe.rectTransform.localPosition = new Vector3(0, currentHealthGlobe.rectTransform.rect.height * ratio - currentHealthGlobe.rectTransform.rect.height, 0);
    }

    public void UpdateGraphics()
    {
        UpdateHealthGlobe();
    }
}
