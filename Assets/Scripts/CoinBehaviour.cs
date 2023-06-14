using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public int order = -1;
    public float speed = .05f;
    public bool isInBank = false;
    public GameController mainController;

    private bool isDown = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (order != -1 && isDown)
            transform.Translate(Vector3.down * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     if (collision.CompareTag("piggybank"))
        {
            gameObject.SetActive(false);
            isDown = false;
            isInBank = true;
            mainController.AnotherCoinInBank();
        }   
    }

    public void ActiveAgain(Vector3 newPos)
    {
        transform.position = newPos;
        gameObject.SetActive(true);
    }

    private void OnMouseUpAsButton()
    {
        mainController.CoinClick(order);
    }
}
