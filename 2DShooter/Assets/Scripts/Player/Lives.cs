using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public GameObject[] live;
    private Health h=null;
    private GameObject p=null;

    private void Awake()
    {
        p = GameObject.FindGameObjectWithTag("Player");
        if(p!=null)
        {
            h = p.GetComponent<Health>();

            foreach (GameObject l in live)
            {
                l.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (h.currentLives < 3 && h.currentLives >= 0)
        {
            for (int i = h.currentLives; i < 3; i++)
            {
                live[i].SetActive(false);
            }

        }
    }
}
