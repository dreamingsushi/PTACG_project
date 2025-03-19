using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDirectionChange : MonoBehaviour
{
    public float startAngle;
    public float endAngle;
    public GameObject darkCloud;
    private float counter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(startAngle, 5.46f, 0);
    
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        float currentAngle = Mathf.Lerp(startAngle, endAngle, counter/GameStartManager.instance.setTimeLimit*1/2);
        
        transform.eulerAngles = new Vector3(currentAngle, 5.46f, 0);

        if(GameStartManager.instance.timeLeft < GameStartManager.instance.setTimeLimit/2)
        {
            darkCloud.SetActive(true);
        }
    }
}
