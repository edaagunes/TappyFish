using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody2D _rb;

    [SerializeField]
    private float _speed;

    int angle;
    int maxAngle=20;
    int minAngle=-60;
  
    public Score score;

    public GameManager gameManager;

    public Sprite fishDied;
    private SpriteRenderer sp;
    public Animator anim;

    public ObstacleSpawner obstaclespawner;

    bool touchedGround;

    [SerializeField] private AudioSource swim,hit,point;


   void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale=0;
        
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        FishSwim();  
        
    }

    void FixedUpdate()
    {
        FishRotation();
    }

   void FishSwim()
   {

    if(Input.GetMouseButtonDown(0) && GameManager.gameOver== false)
        {
            swim.Play();
            if (GameManager.gameStarted == false)
            {
                _rb.gravityScale = 4f;
                _rb.velocity =Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
                obstaclespawner.InstantiateObstacle();
                gameManager.GameHasStarted();

            }
            else{
            _rb.velocity = Vector2.zero;
            _rb.velocity = new Vector2(_rb.velocity.x,_speed);
            }
        }
   }

   void FishRotation()
   {

    if(_rb.velocity.y > 0)
        {
            if(angle <= maxAngle)
            {
                angle = angle + 4; //hızlandığında balık kafasını açısına +4 ekleyerek kaldıracak
            }
        }

        else if(_rb.velocity.y < -1.2)
        {
            if(angle > minAngle)
            {
            angle = angle - 2;  //yavaşladığında balık kafasını açısını -2 azaltarak indirecek
            }

        }

        else if(_rb.velocity.y < -1.1)
        {
          if (angle > minAngle)
        {
            angle -= 2;
        }
        }

        if(touchedGround == false)
        {
            transform.rotation = Quaternion.Euler(0,0,angle);
        }        
  

   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
    if(collision.CompareTag("Obstacle"))
    {
        score.Scored();
        point.Play();
    }

    else if(collision.CompareTag("Column") && GameManager.gameOver==false)
    {
        FishDieEffect();
        gameManager.GameOver();
        GameOver();
    }

   }

   void FishDieEffect()
    {
        hit.Play();
    
    }
    private void OnCollisionEnter2D(Collision2D collision)
   {
    if(collision.gameObject.CompareTag("Ground"))
    {
        if (GameManager.gameOver == false)
        {
            FishDieEffect();
            gameManager.GameOver();
            GameOver();
        }
    }

    else
    {
        //gameOver fish;
        gameManager.GameOver();
        GameOver();
    }
   }

   void GameOver()
   {
    touchedGround = true;
    transform.rotation = Quaternion.Euler(0, 0, -90);
    sp.sprite = fishDied;
    anim.enabled = false;
   }
}
