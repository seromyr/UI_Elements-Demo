using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _health;
    private float Health { get { return _health; } }

    private float maxHp, minHp;

    [SerializeField]
    private Image bloodSplatterEffect;
    private Color bloodEffectColor;
    private bool bloodEffect;

    private Image hpBarFill, hpBarFrame;
    [SerializeField]
    private Gradient hpBarFillGradient;
    void Start()
    {
        maxHp = 100;
        minHp = 0;
        _health = maxHp * 0.8f;

        hpBarFill = transform.Find("SpatialHP").transform.Find("Fill").GetComponent<Image>();
        hpBarFrame = transform.Find("SpatialHP").transform.Find("Frame").GetComponent<Image>();
        bloodSplatterEffect = GameObject.Find("BloodSplatter").GetComponent<Image>();

        bloodEffect = false;
        bloodEffectColor = bloodSplatterEffect.color;
        bloodEffectColor.a = 0;
        bloodSplatterEffect.color = bloodEffectColor;
    }

    void Update()
    {
        HpLimiter();
        ShowHpInHpBar();
        ShowBloodEffect();
    }

    private void HpLimiter()
    {
        if (_health >= maxHp)
        {
            _health = maxHp;
        }

        if (_health <= minHp)
        {
            _health = minHp;
        }
    }

    private void ShowHpInHpBar()
    {
        hpBarFill.fillAmount = _health / 100;
        hpBarFill.color = hpBarFillGradient.Evaluate(_health / 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Acid"))
        {
            bloodEffect = true;
        }

        else if (other.CompareTag("Pickup"))
        {
            TakeDamage(-5);
            other.GetComponent<Pickup>().Pick();
        }

        else if (other.CompareTag("Boss"))
        {
            transform.GetComponent<PlayerController>().ToggleController(false);
            other.gameObject.GetComponent<Boss>().Go();
            for (int i = 0; i < other.transform.childCount; i++)
            {
                other.transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Acid"))
        {
            TakeDamage(0.5f);
        }

        else if (other.CompareTag("Fountain"))
        {
            TakeDamage(-1);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Acid"))
        {
            bloodEffect = false;
        }
    }

    private void ShowBloodEffect()
    {
        if (bloodEffect)
        {
            bloodEffectColor.a = Mathf.Sin(Time.time * 10);

            hpBarFrame.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(Time.time * 20));
        }
        else
        {
            bloodEffectColor.a = 0;
            hpBarFrame.color = Color.white;
        }

        bloodSplatterEffect.color = bloodEffectColor;
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
    }

}
