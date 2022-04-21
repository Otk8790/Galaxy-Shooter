using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    public bool canTripleShoot = false;
    public bool isSpeedBoostActive = false;
    public bool shieldsActive = false;
    public bool canLaserBurst = false;
    public int lives = 3;
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    [SerializeField]
    private GameObject _laserPrefab;
    
    [SerializeField]
    private GameObject _laserBurstPrefab;

    [SerializeField]
    private GameObject _tripleShootLaserPrefab;
    
    [SerializeField]
    private GameObject _tripleShootLaserBurstPrefab;
    
    [SerializeField]
    private GameObject _shieldsPrefab;
    
    [SerializeField] 
    private GameObject[] _engines;
    
    [SerializeField] 
    private GameObject _explosionPrefab;
    
    [SerializeField]
    private float _fireRate = 0.8f;
    private float _canFire = 0.0f;
    private float _BurstFireRate = 0.1f;
    private float startTime;

    [SerializeField]
    private float _speed = 0f;

    private UIManager _uIManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    
    private int hitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uIManager != null)
        {
            _uIManager.UpdateLives(lives);
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        /*if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutine();
        }*/

        _audioSource = GetComponent<AudioSource>();

        hitCount = 0;
        
        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0f, -3f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOne == true)
        {
            PlayerOneMovement();
            
#if UNITY_ANDROID
            if ((Input.GetKey(KeyCode.Space) || CrossPlatformInputManager.GetButton("Fire")) && isPlayerOne == true)
            {
                PlayerOneShoot();
            }
#elif UNITY_IOS
            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) && isPlayerOne == true))
            {
                PlayerOneShoot();
            }
#else
            if (_gameManager.isPaused == false)
            {
                if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) && isPlayerOne == true))
                {
                    PlayerOneShoot();
                }
            }
#endif
            
        }
        
        else if (isPlayerTwo == true)
        {
            PlayerTwoMovement();

            if (_gameManager.isPaused == false)
            {
                if ((Input.GetKey(KeyCode.U) || Input.GetMouseButton(1)) && isPlayerTwo == true)
                {
                    PlayerOneShoot();
                }
            }
        }
    }

    public void Damage()
    {
        if (shieldsActive == true)
        {
            shieldsActive = false;
            _shieldsPrefab.SetActive(false);
            return;
        }
        
        hitCount ++;
        Debug.Log("Hits: " + hitCount);

        lives --;
        _uIManager.UpdateLives(lives);
        
        if (hitCount == 1 && lives == 2)
        {
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2 && lives == 1)
        {
            _engines[1].SetActive(true);
        }

        if (lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uIManager.ShowTitleScreen();
            _uIManager.CheckForBestScore();
            Destroy(this.gameObject);
        }
    }
    
    private void PlayerOneShoot()
    {
        if (canLaserBurst == false)
        {
            if (Time.time > _canFire)
            {
                _audioSource.Play();
                if (canTripleShoot == true)
                {
                    Instantiate(_tripleShootLaserPrefab, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                }
                _canFire = Time.time + _fireRate;
            }
        }
        else if (canLaserBurst == true)
        {
            if (Time.time > _canFire)
            {
                _audioSource.Play();
                if (canTripleShoot == true)
                {
                    Instantiate(_tripleShootLaserBurstPrefab, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserBurstPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                }
                _canFire = Time.time + _BurstFireRate;
            }
        }
    }

    private void PlayerOneMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); //Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); //Input.GetAxis("Vertical");

        if (isSpeedBoostActive == true)
        {
            transform.Translate(Vector3.right * _speed * 2f * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * 2f * verticalInput * Time.deltaTime);
        }
        else if (isSpeedBoostActive == false)
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime); 
        }

        //Limits Axis "Y"
        if (transform.position.y > 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        else if (transform.position.y < -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0f);
        }

        //Limits Axis "X"
        if (transform.position.x > 8f)
        {
            transform.position = new Vector3(8f, transform.position.y, 0f);
        }
        else if (transform.position.x < -8f)
        {
            transform.position = new Vector3(-8f, transform.position.y, 0f);
        }
    }
    
    private void PlayerTwoMovement()
    {
        if (isSpeedBoostActive == true)
        {
            if (Input.GetKey(KeyCode.Keypad8) || Input.GetKey(KeyCode.I))
            {
                transform.Translate(Vector3.up * _speed * 2 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.L))
            {
                transform.Translate(Vector3.right * _speed * 2 * Time.deltaTime);
            }
        
            if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.K))
            {
                transform.Translate(Vector3.down * _speed * 2 * Time.deltaTime);
            }
        
            if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.J))
            {
                transform.Translate(Vector3.left * _speed * 2 * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad8) || Input.GetKey(KeyCode.I))
            {
                transform.Translate(Vector3.up * _speed *  Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.L))
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
        
            if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.K))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
        
            if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.J))
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
        }

        //Limits Axis "Y"
        if (transform.position.y > 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        else if (transform.position.y < -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0f);
        }

        //Limits Axis "X"
        if (transform.position.x > 8f)
        {
            transform.position = new Vector3(8f, transform.position.y, 0f);
        }
        else if (transform.position.x < -8f)
        {
            transform.position = new Vector3(-8f, transform.position.y, 0f);
        }
    }

    //Power Up Triple Shoot
    public void TripleShootPowerUpOn()
    {
        canTripleShoot = true;
        StartCoroutine(TripleShootPowerUpDownRoutine());
    }
    public IEnumerator TripleShootPowerUpDownRoutine()
    {
        yield return new WaitForSeconds(12f);
        canTripleShoot = false;
    }

    //Power Up Speed Boost
    public void SpeedBoostPowerUpOn()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedPowerUpDownRoutine());
    }

    public IEnumerator SpeedPowerUpDownRoutine()
    {
        yield return new WaitForSeconds(10f);
        isSpeedBoostActive = false;
    }
    
    //Power Up Shields
    public void EnableShields()
    {
        shieldsActive = true;
        _shieldsPrefab.SetActive(true);
    }

    public void LivesUp()
    {
        if (lives < 3)
        {
            lives++;
            hitCount--;
            _uIManager.UpdateLives(lives);
        }
        if (lives == 3)
        {
            _engines[0].SetActive(false);
        }
        if (lives == 2)
        {
            _engines[1].SetActive(false);
        }
    }

    public void BursLaserPowerUpOn()
    {
        canLaserBurst = true;
        StartCoroutine(BurstLaserPowerUpDown());
    }
    
    public IEnumerator BurstLaserPowerUpDown()
    {
        yield return new WaitForSeconds(6f);
        canLaserBurst = false;
    }

    public IEnumerator ResetRoutine()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
