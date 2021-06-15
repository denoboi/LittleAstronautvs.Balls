﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float powerUpStrength = 10;
    public GameObject powerupIndicator;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup = false;

    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    
    void Update()
    {
        float verticalMove = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalMove * speed);
        powerupIndicator.transform.position = gameObject.transform.position + new Vector3(0, -0.2f, 0); // player ile power up indikator ayni anda hareket etmeli.
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUp")) //if other thing that we run into is a power up:
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>(); //enemy rigidbodysi ama bunu tam carptigimizda aliyoruz
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position; //enemy'nin player'dan uzakligi. bunu da enemy pozisyonundan playerin pozisyonunu cikararak bulabiliriz.
            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse); //aninda bir hizlanma istedigimiz icin forcemode.impulse kullandik.
            Debug.Log("Collided with" + gameObject + "with power up set to: " + hasPowerup);
        }
    }
    IEnumerator PowerupCountdownRoutine() //powerup suresi
    {
        yield return new WaitForSeconds(5);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
}
