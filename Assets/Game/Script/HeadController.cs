using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadController : MonoBehaviour
{
    public float Head { get; set; }
    private float HeadMax = 3000f;
    public float Killcoin { get; set; }
    private Animator anim;
    private MenuController menuController;
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI ValueHeath;
    [SerializeField] private TextMeshProUGUI headMax;
    private void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        anim = GetComponent<Animator>();
        Killcoin = 10000;
        Head = HeadMax;
    }
    void Update()
    {
        UpdateHeath(Head, HeadMax);
        headMax.text = "Máu nhân vật: " + HeadMax;
        if (Head <= 0)
        {
            PlayerDie();
        }
    }
    public void UpdateHeath(float heath, float heathMax)
    {
        fillBar.fillAmount = (float)heath / (float)heathMax;
        ValueHeath.text = heath.ToString() + " / " + heathMax.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.collider.CompareTag("Store"))
        {
            menuController.OpenStore();
        }
        else if (collision.collider.CompareTag("BuffHead") && Head != HeadMax)
        {
            Head = HeadMax;
            Destroy(collision.collider.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Store"))
        {
            menuController.OpenStore();
        }
        else if (other.CompareTag("BuffHead") && Head != HeadMax)
        {
            Head = HeadMax;
            Destroy(other.gameObject);
        }
    }
    public void Headdown(float damage)
    {
        Head -= damage;
    }
    private void PlayerDie()
    {
        anim.SetTrigger("IsDeath");
        menuController.GameOver();
        gameObject.SetActive(false);
    }
}
