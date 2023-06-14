using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<GameObject> coinPrefabs;
    public ShakeBank piggybank;
    public int coinsToDrop = 4;
    public float upShift = 6;
    public float waitingTime = .1f;

    public float xRight = 9;
    public float yStart = 4;
    public float yEnd = -2;

    public Spine.Unity.SkeletonAnimation character;

    public Canvas gameEnd;
    public Text endText;

    private List<GameObject> droppedCoins;
    private int coinsInBank = 0;
    private bool coinsToRight = false;

    private bool isWinning = true;
    private int clickCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (coinPrefabs == null || piggybank == null)
            throw new System.Exception("Нет префабов!!!");


        InitData();
    }

    // Update is called once per frame
    void Update()
    {
        if (coinsInBank == coinsToDrop)
        {
            coinsInBank = 0;
            StartCoroutine(ShakeBank());
        }

        if (coinsToRight)
        {
            coinsToRight = false;
            CoinsToRight();
        }

        if (coinsToDrop == clickCounter)
        {
            clickCounter=0;
            GameEnd();
        }
    }

    IEnumerator ShakeBank()
    {
        var coroutine = StartCoroutine(piggybank.ShakeMe());
        yield return new WaitForSeconds(1);
        StopCoroutine(coroutine);
        Cursor.visible = true;
        coinsToRight = true;
    }
    IEnumerator DropCoinsCourutine()
    {
        //yield return new WaitForSeconds(1);
        for(int i = 0; i < droppedCoins.Count; i++)
        {
            yield return new WaitForSeconds(waitingTime);
            var coin = Instantiate(droppedCoins[i]);
            coin.transform.position = Vector3.up * upShift;
            var coinBhv = coin.AddComponent<CoinBehaviour>();
            coinBhv.order = i;
            coinBhv.mainController = this;
            droppedCoins[i] = coin;
        }
    }
    IEnumerator DeleteCoins()
    {
        for (int i = 0; i < droppedCoins.Count; i++)
            Destroy(droppedCoins[i].gameObject);
        droppedCoins = null;
        yield return null;
    }
    public void InitData()
    {
        Cursor.visible = false;
        character.AnimationName = "idle";
        droppedCoins = new List<GameObject>();
        ShuffleCoins(ref coinPrefabs);
        for (int i = 0; i < coinsToDrop; i++)
        {
            droppedCoins.Add(coinPrefabs[i]);
        }
        StartCoroutine(DropCoinsCourutine());
    }
    public void GameEnd()
    {
        StartCoroutine(DeleteCoins());
        if (isWinning)
        {
            character.AnimationName = "win";
            endText.text = "Молодец!";
        }
        else
        {
            character.AnimationName = "wrong";
            endText.text = "Попробуй еще раз...";
        }

        piggybank.gameObject.SetActive(false);
        gameEnd.gameObject.SetActive(true);
    }
    public void ShuffleCoins(ref List<GameObject> list)
    {
        var rnd = new System.Random();
        for (int i = 0; i < list.Count; i++)
        {
            int j = rnd.Next(list.Count);
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
    public void CoinsToRight()
    {
        ShuffleCoins(ref droppedCoins);
        var ystep = (yStart - yEnd) / coinsToDrop;
        for (int i = 0; i < droppedCoins.Count; i++)
            droppedCoins[i].GetComponent<CoinBehaviour>().ActiveAgain(new Vector3(xRight, yEnd + i * ystep));
        character.AnimationName = "correct";
    }
    public void AnotherCoinInBank()
    {
        coinsInBank++;
    }
    public void CoinClick(int order) 
    {
        isWinning &= order == clickCounter;
        clickCounter++;
    }
    public void OnRestartClick()
    {
        piggybank.gameObject.SetActive(true);
        gameEnd.gameObject.SetActive(false);
        InitData();
    }
}
