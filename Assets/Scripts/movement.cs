using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public GameObject Manager;
    public GameObject Fireworks;
    public static float coins = 0;
    public float keys = 0;
    public Rigidbody2D rb2D;
    public ParticleSystem dust;
    public Image staminaBarimg;
    private float speed;
    public float NormalSpeed = 15f;
    public float stamina = 10f;
    public bool ready;

    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("Spawner");
        speed = NormalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<health>().alive && ready) {
            rotation();
            thrust();
            StaminaFill();
        }
    }


    public void rotation() {
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Rotate(0f, 0f, 5f);
        }

        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Rotate(0f, 0f, -5f);
        }

        else
        {
            transform.Rotate(0f, 0f, Random.Range(-0.5f, 0.5f));
        }
    }

    public void thrust() {
        rb2D.AddForce(transform.up * speed * Time.deltaTime, ForceMode2D.Impulse);
        dust.Play();
        if (stamina > 0f && ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))))
        {
            speed = 400f;
            stamina -= (2 * Time.deltaTime);
        }

        else if (stamina < 10f && !((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))))
        {
            StartCoroutine("Regen", 3f);
        }

        else {

            speed = NormalSpeed;

        }
    }


    IEnumerator Regen(float duration)
    {
        speed = NormalSpeed;
        yield return new WaitForSeconds(duration);
        if (stamina < 10f && !((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))) && gameObject.GetComponent<Hunger>().energy) {
            stamina += Time.deltaTime;
        }
    }

    public void StaminaFill()
    {
        staminaBarimg.fillAmount = stamina / 10;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Coin")
        {
            coins++;
            Manager.GetComponent<Spawner>().numCoins--;
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Key")
        {
            keys++;
            GameObject clone2 = Instantiate(Fireworks, col.gameObject.transform.position, Quaternion.identity);
            Destroy(clone2.gameObject, 1f);
            Destroy(col.gameObject);
        }
    }

}
