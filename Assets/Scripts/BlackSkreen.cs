using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSkreen : MonoBehaviour
{
    float alpha = 0;
    float speed = 1f;

    void Update()
    {
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        alpha = alpha + speed * Time.deltaTime;
    }
}
