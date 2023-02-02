using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


enum TypeOfDamage
{
    SLIME,
    BAT,
    PLAYER,
    CHICKEN
}

public class HealthControlsMob : MonoBehaviour
{
    
    [Header("HealthData")]
    [SerializeField] float _maxHealth;
    [SerializeField] public float _currentHealth;
    [SerializeField] float _minHealth;
    //[SerializeField] bool _invincible = false;

    [Header ("DamageWhenHitBy")]
    [SerializeField] float _slimeCollision;
    [SerializeField] float _playerCollision;
    [SerializeField] float _batCollision;
    [SerializeField] float _spellCollision;

    [Header("YourAttacks")]
    [SerializeField] float _onSlime;
    [SerializeField] float _onPlayer;
    [SerializeField] float _onBat;
    [SerializeField] float _onChicken;

    private Rigidbody2D _rb2D;

    //private bool _isHit;
    private float _hitTimeOut = 0.5f;
    private float _hitTimeOutCounter;

    private SpriteRenderer _PNJSprite;
    [SerializeField] GameObject _deathparticles;
    private ParticleSystem _deathparticlesParticles;
    [SerializeField] GameObject _collectible;

    private GameObject _graphics;
    private bool _created = false;

    private float _waitforDestroy;
    private float _waitforDestroyCounter;


    // Start is called before the first frame update 

    private void Awake()
    {
        
        
    }

    void Start()
    {
        _rb2D= GetComponent<Rigidbody2D>();
        _hitTimeOut = _hitTimeOutCounter;

        _PNJSprite= GetComponentInChildren<SpriteRenderer>();
        _graphics = gameObject.transform.Find("Graphics").GetComponent<GameObject>();

        
    }

    // Update is called once per frame
    void Update()
    {


        if (_currentHealth <= 0)
        {
            MobDeath();
        }
    }


    void MobDeath()
    {
        //_knockback.OnHit(_deathKnockback);

        _PNJSprite.enabled = false;
        

        if (!_created)
        {
            Instantiate(_deathparticles, transform.position, Quaternion.identity);
            Instantiate(_collectible, transform.position, Quaternion.identity);
            
            _deathparticlesParticles = _deathparticles.GetComponent<ParticleSystem>();
            _created = true;

            StopAllCoroutines();
            StartCoroutine(DestroyMob());
           //if (!_deathparticlesParticles.IsAlive())
           //{
           //    cree une erreur instance manquante (1)
           //}
        }
        

    }

    IEnumerator DestroyMob()
    {
        yield return new WaitForSeconds(0.5f);
        DestroyThisGameObject();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TakeDamage(_playerCollision);
        }
        if (collision.collider.CompareTag("Spell"))
        {
            TakeDamage(_spellCollision);
        }

    }
    void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth < _minHealth)
        {
            _currentHealth = 0;
        }
    }
    void GetHealed(float heal)
    {
        _currentHealth += heal;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }


    public void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }
}
