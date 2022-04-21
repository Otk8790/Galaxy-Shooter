using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0f;

    [SerializeField] 
    private int _powerUpID;

    [SerializeField] 
    private AudioClip _clip;

    private float volume = 0.35f;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (_gameManager.gameOver == true)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide with: " + other.name);

        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, volume);
            
            //Access Player Script
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                //Enabled Triple Shoot
                if (_powerUpID == 0)
                {
                    player.TripleShootPowerUpOn();
                }
                //Enabled Speed
                else if (_powerUpID == 1)
                {
                    player.SpeedBoostPowerUpOn();
                }
                //Enable Shield
                else if (_powerUpID == 2)
                {
                    player.EnableShields();
                }
                else if (_powerUpID == 3)
                {
                    player.LivesUp();
                }
                else if (_powerUpID == 4)
                {
                    player.BursLaserPowerUpOn();
                }
            }
            
            Destroy(this.gameObject);
        }
        
    }
}
