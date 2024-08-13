using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunsMenu : MonoBehaviour
{
    public GameObject[] Guns;
    public Dictionary<int, bool> BuyGuns = new Dictionary<int, bool>();
    private Dictionary<int, float> PriceGuns = new Dictionary<int, float>();
    private Dictionary<int, int> BulletNum = new Dictionary<int, int>();
    private Dictionary<int, float> PriceBullet = new Dictionary<int, float>();
    private PlayerMoverment player;
    private HeadController headController;
    [SerializeField]private TextMeshProUGUI priceGuns;
    [SerializeField] private TextMeshProUGUI pricebullet;
    [SerializeField] private TextMeshProUGUI bulletnum;
    [SerializeField] private TextMeshProUGUI coin;
    int currentGun = 0;
    void Awake()
    {
        player = FindObjectOfType<PlayerMoverment>();
        headController = FindObjectOfType<HeadController>();
        Guns[0].SetActive(true);
        PriceGuns.Add(0, 0);
        PriceGuns.Add(1, 1000);
        PriceGuns.Add(2, 2000);
        PriceGuns.Add(3, 3000);
        PriceGuns.Add(4, 4000);
        BulletNum.Add(0, 10);
        BulletNum.Add(1, 100);
        BulletNum.Add(2, 100);
        BulletNum.Add(3, 100);
        BulletNum.Add(4, 100);
        PriceBullet.Add(0, 1);
        PriceBullet.Add(1, 10);
        PriceBullet.Add(2, 10);
        PriceBullet.Add(3, 10);
        PriceBullet.Add(4, 10);
        BuyGuns[0] = true;
        for (int i = 1; i < Guns.Length; i++)
        {
            BuyGuns[i] = false;
        }

    }
    public void NextGun()
    {
        Guns[currentGun ].SetActive(false);
        currentGun++;
        if (currentGun >= Guns.Length)
            currentGun = 0;
        Guns[currentGun].SetActive(true);
    }
    public void PreviousGun()
    {
        Guns[currentGun].SetActive(false);
        currentGun--;
        if (currentGun < 0)
            currentGun = Guns.Length - 1;
        Guns[currentGun].SetActive(true);
    }
    private void Update()
    {
        bulletnum.text = ": " + player.GetBullet(currentGun);
        coin.text = ": " + headController.Killcoin;
        pricebullet.text = "" + PriceBullet[currentGun];
        if (BuyGuns[currentGun])
        {
            priceGuns.text = "";
        }
        else
        {
            priceGuns.text = "" + PriceGuns[currentGun];
        }
       
    }
    public void BuyGun()
    {
        if (!BuyGuns[currentGun] && headController.Killcoin >= PriceGuns[currentGun])
        {
            headController.Killcoin -= PriceGuns[currentGun];
            BuyGuns[currentGun] = true;
        }
    }
    public bool IsGunBought(int gunIndex)
    {
        return BuyGuns.ContainsKey(gunIndex) && BuyGuns[gunIndex];
    }
    public void BuyBullet()
    {
        if(headController.Killcoin >= PriceBullet[currentGun])
        {
            headController.Killcoin -= PriceBullet[currentGun];
            player.SetBulletGun(currentGun, BulletNum[currentGun]);
        }
        
    }
}
