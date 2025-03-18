using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDirectionChange : MonoBehaviour
{
    public float startAngle;
    public float endAngle;
    public GameObject darkCloud;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(startAngle, -30, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float currentAngle = Mathf.Lerp(startAngle, endAngle, GameStartManager.instance.setTimeLimit*(2/3));
        transform.eulerAngles = new Vector3(currentAngle, -30, 0);

        if(GameStartManager.instance.timeLeft < GameStartManager.instance.setTimeLimit/2)
        {
            darkCloud.SetActive(true);
        }
    }
}
