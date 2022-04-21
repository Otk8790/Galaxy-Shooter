using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0f;

    [SerializeField]
    private GameObject _enemyExplosionPrefab;

    private UIManager _uiManager;
    
    [SerializeField]
    private AudioClip _clip;
    private GameManager _gameManager;
    private float volume = 0.35f;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        //Movement
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        //Destroy in the Game Over 
        if (_gameManager.gameOver == true)
        {
            Destroy(this.gameObject);
        }
        
        //Random Spawn
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
            {
                Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(other.transform.parent.gameObject);
            }
            
            Destroy(other.gameObject);
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, volume);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player" )
        {
            Player player = other.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }
        }
        Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, volume);
        Destroy(this.gameObject);
    }
}
