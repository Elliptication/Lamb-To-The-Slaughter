using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject lambLeg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LambCheck()
    {
        if(lambLeg.activeInHierarchy)
        {
            LoadLevel("Lose");
        }
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
