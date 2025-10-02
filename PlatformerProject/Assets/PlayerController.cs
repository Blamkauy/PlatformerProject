using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float MoveSpeed;
    public bool OnGround;
    float gte = 0f;
    float ste = -10000f;
    public Sprite[] sprites;
    public SpriteRenderer sr;
    public static PlayerController main;
    public Camera cam;
    /*
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) { UnityEngine.SceneManagement.SceneManager.LoadScene(0); }
    }
    */
    private void Awake()
    {
        main = this;
    }
    void Start()
    {
        cam = Camera.main;
    }
    private void FixedUpdate()
    {
        RaycastHit2D ground = Physics2D.CircleCast((Vector2)transform.position - Vector2.up * .65f, .5f, -Vector2.up, .1f, 1);
        if(!OnGround&&ground&&rb.velocity.y<=0)
        {
            stretchVel = rb.velocity.y*0.4f;
        }
        OnGround = ground && rb.velocity.y <= 0;
        if (OnGround)
        {
            transform.position = new Vector2(transform.position.x, ground.point.y + .70f);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
    float walkAnim = 0;
    float jte = -100;
    public float stretch = 0f;
    public float stretchExponent = 0;
    float stretchVel = 0f;
    void Update()
    {
        if (transform.position.y < -10) { transform.position = Vector3.zero; rb.velocity = Vector2.zero; }
        if (Input.GetKeyDown(KeyCode.Space))
            ste = Time.time;
        bool Crouching = Input.GetKey(KeyCode.S);
        //healthBar.SetPosition(1, Vector3.right * (Health * .2f - 1f));
        if (OnGround)
        {
            gte = Time.time;
            walkAnim = (walkAnim + Mathf.Abs(rb.velocity.x) * Time.deltaTime * .4f) % 1;
            sr.transform.parent.localScale = new Vector2(1f, 1f);

        }
        else
        {
            if (rb.velocity.y <= 0) rb.velocity -= Vector2.up * Time.deltaTime * (Crouching ? 25 : 15); else if (Time.time - ste < .5f && !Input.GetKey(KeyCode.Space)) rb.velocity = rb.velocity -= Vector2.up * Time.deltaTime * 25;
            

        }
        if (Time.time - ste < 0.1f && Time.time - gte < 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 15); jte = Time.time;
            stretchVel = 5;
        }

        stretchVel += Time.deltaTime * (Mathf.Abs(rb.velocity.y)*0.02f-stretchExponent) * 90;
        stretchVel *= Mathf.Pow(0.04f, Time.deltaTime);
        stretchExponent += stretchVel * Time.deltaTime;
        stretch = Mathf.Pow(2f,stretchExponent);

        sr.transform.parent.localScale = new Vector2(1f / stretch, stretch);

        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(rb.velocity.x * Mathf.Pow(0.5f, Time.deltaTime * (OnGround ? 20 : 4f)), rb.velocity.y);
        rb.velocity += Vector2.right * move * MoveSpeed * Time.deltaTime * (OnGround ? 1f : 0.4f) * (Crouching ? .5f : 1.0f);
        //sr.sprite = OnGround ? Crouching ? sprites[3] : Mathf.Abs(move) > 0 ? walkAnim < .5f ? sprites[1] : sprites[2] : sprites[0] : sprites[4];
        sr.flipX = move != 0 ? move < 0 : sr.flipX;
        cam.transform.position = Vector3.Lerp(transform.position - Vector3.forward * 10 + Vector3.up * 2 + (Vector3)rb.velocity * .1f, cam.transform.position, Mathf.Pow(0.06f, Time.deltaTime));
    }
}
