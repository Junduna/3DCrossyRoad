using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
   public static MusicControl instance;

   private void Awake()
   {
        // gameobject tidak dimatikan ketika ganti scene
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
    }
}