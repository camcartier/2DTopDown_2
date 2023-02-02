using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    [SerializeField] GameObject _destroyableAnimation;
    
    [SerializeField] public float _destroyableCurrentHP;
    [SerializeField] public float _destroyableMaxHP;

    private void Update()
    {
        if (_destroyableCurrentHP <= 0)
        {
            DestroyThisGameObject();
        }
    }


    IEnumerator DestroyDestructible()
    {
        yield return new WaitForSeconds(0.5f);
        DestroyThisGameObject();

    }

    public void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }

}
