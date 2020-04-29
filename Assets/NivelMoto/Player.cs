﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float acceleration = 0.6f;
    public float maximunSpeed = 10f;
    public float obstacleSlowDown = 0.25f;
    private float speed = 0f;
    public bool reachedFinishLine = false;
    public AudioSource vaquitamu;
    //public AudioSource pedaleo;

    // Start is called before the first frame update
    [SerializeField] private GameObject pausePanel;
    void Start()
    {
        pausePanel.SetActive(false);    
        vaquitamu = GetComponent<AudioSource>();
        //pedaleo = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //pause event
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy) 
            {
                PauseGame();
            }
            else 
            {
                 ContinueGame();   
            }
        }

        // Acceleration logic

        Vector3 direction = new Vector3(transform.parent.forward.x, 0, transform.parent.forward.z);

        speed += acceleration * Time.deltaTime;
        if (speed > maximunSpeed)
        {
            speed = maximunSpeed;
        }

        if (speed < 0)
        {
            //detener pedaleo
            speed = 0;
        }

        if (acceleration < 0)
        {
            transform.parent.position += direction.normalized * speed * Time.deltaTime;
        }

        // Make player move automatically

        // Vector3 direction = new Vector3(transform.parent.forward.x, 0, transform.parent.forward.z);
        // transform.parent.position += direction.normalized * speed * Time.deltaTime;

        if (Input.GetKey("up")) {
            if (acceleration < 0) {
                acceleration *= -1;
                //reproducir pedaleo
            }
            transform.parent.position += direction.normalized * speed * Time.deltaTime;
        }

        if (Input.GetKeyUp("up"))
        {
            acceleration *= -1;
        }




        // Make player stay inside a certain area
        if (transform.parent.position.x < -5f)
        {
            transform.parent.position = new Vector3(-5f, transform.parent.position.y, transform.parent.position.z);
        }
        else if (transform.position.x > 5f) {
            transform.parent.position = new Vector3(5f, transform.parent.position.y, transform.parent.position.z);
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == "Obstacle")
        {
            vaquitamu.Play();
            speed *= obstacleSlowDown;
        }
        else if (otherCollider.tag == "FinishLine") {
            reachedFinishLine = true;
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    } 
    private void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }
}
