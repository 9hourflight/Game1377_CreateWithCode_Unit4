using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    [SerializeField]
    private float speed;
    [SerializeField]
    public bool hasPowerup;
    private float powerUpStrength = 15.0f;
    public GameObject powerUpIndicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerUpIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerUpIndicator.gameObject.SetActive(false);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            Debug.Log("Collided with " + collision.gameObject.name + "with powerup set to " + hasPowerup);
            enemyRigidBody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
}
