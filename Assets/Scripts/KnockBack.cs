using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] float _knockbackForce;
    [SerializeField] float _feebleKnockbackForce;
    //[SerializeField] bool _invicible = false;


    private Rigidbody2D _rb2D;
    private int _damage;



    // Start is called before the first frame update
    void Start()
    {

        _rb2D= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit(Vector2 knockback)
    {
        //Debug.Log("hit");

        _rb2D.AddForce(knockback);

        
        //Debug.Log($"{knockback}");
        //Debug.Log($"velocity {_rb2D.velocity}");
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        if (collider.CompareTag("Player")) 
        {
            Vector2 _direction = (transform.position - collider.transform.position).normalized;
            Vector2 knockback = _direction * _knockbackForce;

            OnHit(knockback);
        }

        if (collider.CompareTag("Meat") || collider.CompareTag("Mob"))
        {
            Vector2 _direction = (transform.position - collider.transform.position).normalized;
            Vector2 knockback = _direction * _feebleKnockbackForce;

            OnHit(knockback);
        }
    }
}
