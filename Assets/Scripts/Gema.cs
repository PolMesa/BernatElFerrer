using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Gema : MonoBehaviour
{
    public Image gema;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gema"))
        { 
            Destroy(other.gameObject);
            gema.gameObject.SetActive(true);
        }

    }
}
