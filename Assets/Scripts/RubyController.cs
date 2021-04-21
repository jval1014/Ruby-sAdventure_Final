using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
 
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public int currentHealth;
    public int health { get { return currentHealth; } }
    public GameObject projectilePrefab;
    public ParticleSystem rubyHurt;
    public Text winText;
    public Text scoreText;
    public GameObject hitparticlesPrefab;
 
 
    private int scoreValue = 0;
    
 
    bool isInvincible;
    float invincibleTimer;
 
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
 
    AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip JambiTalk; 
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    public AudioClip backgroundmusic;
   

 
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
 
        audioSource = GetComponent<AudioSource>();
        rubyHurt = GetComponent<ParticleSystem>();
 
        winText.text = "";
      
 
    }
 
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        
    }
 
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
 
        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
 
 
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
 
        if (currentHealth <= 0)
        {
            winText.text = "Failure!";
            speed = 0;
        }
 
 
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
 
 
     if (Input.GetKeyDown(KeyCode.E))
        {
          musicSource.clip = musicClipOne; 
          musicSource.Play();
           
         }

     if (Input.GetKeyUp(KeyCode.E))
        {
        musicSource.Stop();
        musicSource.clip = backgroundmusic;
        musicSource.Play();
         }

     if (Input.GetKeyDown(KeyCode.Q))
        {
          musicSource.clip = musicClipTwo;
          musicSource.Play();
         }

     if (Input.GetKeyUp(KeyCode.Q))
        {
          musicSource.Stop();
          musicSource.clip = backgroundmusic;
        musicSource.Play();

         }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
 
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f,
            LayerMask.GetMask("NPC"));
 
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                    PlaySound(JambiTalk);
                }
            }
        }
 
 
    }
 
    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
 
        rigidbody2d.MovePosition(position);
 
    }
 
     void ChangeScore(int scoreAmount)
    {
       
      
        scoreValue += scoreAmount;
        scoreText.text = scoreValue.ToString();
        Debug.Log ("hit");
      
 
        if (scoreValue == 4)
        {
            winText.text = "You Win! Game By Juan Valderrama";
        }
    }
 
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
 
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitSound);
              GameObject hitparticlesObject = Instantiate(hitparticlesPrefab, transform.position, Quaternion.identity);
 
           
        }
 
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
 
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
 void OnCollisionEnter2D(Collision2D other)
{
    ChargerEnemyController c = other.collider.GetComponent<ChargerEnemyController>();
    if (c != null)
    {
        c.Fix();
    }
 
}



    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f,
        Quaternion.identity);
 
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
 
        animator.SetTrigger("Launch");
 
        PlaySound(throwSound);
 
    }
    }