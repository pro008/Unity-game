using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public Animator amin { get; private set; }

    //Run
    [SerializeField]
    protected float movementSpeed;
    protected bool facingRight;

    protected float groundRadius = 0.2f;
    protected float jumpForce = 250f;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    [SerializeField]
    protected int health;

    [SerializeField]
    private EdgeCollider2D SwordCollider;

    [SerializeField]
    private List<string> damageSources;

    public abstract bool IsDead { get; }

    // Use this for initialization
    public virtual void Start () {
        facingRight = true;
        amin = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public abstract IEnumerator TakeDamage();

    public void changeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }

    public void Base_Attack()
    {
        SwordCollider.enabled = !SwordCollider.enabled;
    }


    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
