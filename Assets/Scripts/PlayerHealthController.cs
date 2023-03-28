using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;

    private const int StartingMaxHealth = 5;
    private const int StartingHealth = 5;

    public int currentHealth;
    public int maxHealth;

    [FormerlySerializedAs("damageInvincLength")] public float damageInvincibleLength = 1f;
    private float _invincibleCount;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Level 1" || sceneName == "Level 1 Start Again")
        {
            maxHealth = StartingMaxHealth;
            currentHealth = StartingHealth;
        }
        else
        {
            maxHealth = CharacterTracker.Instance.maxHealth;
            currentHealth = UIController.Instance.playerCurrentHealth;
        }


        //currentHealth = UIController.instance.playerCurrentHealth;

        UIController.Instance.healthSlider.maxValue = maxHealth;
        UIController.Instance.healthSlider.value = currentHealth;
        UIController.Instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (_invincibleCount > 0)
        {
            _invincibleCount -= Time.deltaTime;

            if(_invincibleCount <= 0)
            {
                var color = PlayerController.Instance.bodySr.color;
                color = new Color(color.r, color.g, color.b, 1f);
                PlayerController.Instance.bodySr.color = color;
            }
        }

    }

    public void DamagePlayer()
    {
        if (_invincibleCount <= 0)
        {
            AudioManager.Instance.PlaySfx(8);
            currentHealth--;

            _invincibleCount = damageInvincibleLength;

            var color = PlayerController.Instance.bodySr.color;
            color = new Color(color.r, color.g, color.b, 0.5f);
            PlayerController.Instance.bodySr.color = color;

            if (currentHealth <= 0)
            {
                PlayerController.Instance.gameObject.SetActive(false);

                UIController.Instance.deathScreen.SetActive(true);
                UIController.Instance.bossHealthUI.SetActive(false);
                LevelManager.Instance.isPaused = true;
                AudioManager.Instance.PlayGameOver();
                AudioManager.Instance.PlaySfx(7);
            }

            UIController.Instance.healthSlider.value = currentHealth;
            UIController.Instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void MakeInvincible(float length)
    {
        _invincibleCount = length;
        var color = PlayerController.Instance.bodySr.color;
        color = new Color(color.r, color.g, color.b, 0.5f);
        PlayerController.Instance.bodySr.color = color;
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.Instance.healthSlider.value = currentHealth;
        UIController.Instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;

        UIController.Instance.healthSlider.maxValue = maxHealth;
        UIController.Instance.healthSlider.value = currentHealth;
        UIController.Instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void DefaultHealth()
    {
        maxHealth = StartingMaxHealth;
        currentHealth = StartingHealth;
    }
}
