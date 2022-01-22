using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{

    public static UiController instance;
    //public TextMeshPro gamePlayScoreText;
    public TMP_Text gamePlayScoreText;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGPScoreText()
	{
        gamePlayScoreText.text = GameManager.instance.score.ToString();
	}
}
