using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthBar;

    [Header("Respawn")]
    public Transform respawnPoint;
    public GameObject player;

    [Header("Game Over")]
    public GameObject gameOverCanvas;

    private CharacterController controller;
    private Character movementScript;
    private Animator animator;
    private bool gameStarted = false;
    private bool isDead = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;

        controller = player.GetComponent<CharacterController>();
        movementScript = player.GetComponent<Character>();
        animator = player.GetComponent<Animator>();

        UpdateHealthUI();

        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
    }

    public void StartGame()
    {
        gameStarted = true;
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (healthBar != null)
            healthBar.gameObject.SetActive(true);

        PlayerKeys keys = player.GetComponent<PlayerKeys>();
        if (keys != null)
        {
            keys.ShowKeyUI();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!gameStarted || isDead)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
       
    }

    void RespawnPlayer()
    {
        controller.enabled = false;
        player.transform.position = respawnPoint.position;
        controller.enabled = true;
    }

    void Die()
    {
        isDead = true;
        if (animator != null)
            animator.SetTrigger("Die");
        if (movementScript != null)
            movementScript.enabled = false;

        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        StartCoroutine(ShowGameOverDelayed());
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void RespawnFromGameOver()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthUI();

        RespawnPlayer();

        if (animator != null)
        {
            animator.ResetTrigger("Die");
            animator.Play("Idle_A", 0, 0f);
        }

        if (movementScript != null)
            movementScript.enabled = true;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (healthBar != null)
            healthBar.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSeconds(1.5f);

        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
    }
}

