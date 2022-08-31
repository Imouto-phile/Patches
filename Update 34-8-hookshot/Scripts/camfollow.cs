using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class camfollow : MonoBehaviour
{
   public Transform player;

    // Update is called once per frame
    void Update () {
        if(player){
        transform.position = player.transform.position + new Vector3(0, 0, -5);}
        else{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
