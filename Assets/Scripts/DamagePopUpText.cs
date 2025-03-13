using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpText : MonoBehaviour
{
    public static DamagePopUpText Instance;
    public GameObject damageTextPrefab;
    void Awake()
    {
        Instance = this;
    }
    public void ShowDamageNumber(Vector3 position, string text)
    {
        var popup = Instantiate(damageTextPrefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popup, 1f);
    }
}
