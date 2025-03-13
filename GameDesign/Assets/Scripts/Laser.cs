using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed;
    public float damage = 20f;
    private float direction;
    private bool hit;
    public float lifetime = 2f;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsObstacle;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        Destroy(gameObject);
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    /*
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        Debug.Log("Laser hit: " + collision.gameObject.name + " on layer: " + LayerMask.LayerToName(layer));

        if ((whatIsEnemy.value & (1 << layer)) != 0) 
        {
            Debug.Log("Laser hit an enemy!");
            Destroy(collision.gameObject);  
            Destroy(gameObject);
        }
        else if ((whatIsObstacle.value & (1 << layer)) != 0) 
        {
            Debug.Log("Laser hit an obstacle!");
            Destroy(gameObject);
        }
    }
    */



}
