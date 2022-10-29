using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    public static bool isTutoButton;
    static TutorialButton _Instance;
    bool isEnd = false;
    bool isIndex = false;
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else if (_Instance != this) // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);  // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.


    }
    // Start is called before the first frame update
    void Start()
    {
        isTutoButton = true;


        GameManager.Money = 2000;
        GameManager.ProfileImage = GameManager.AllNuniArray[0].Image;
        SceneManager.LoadScene("Main");
        
        
    }
        // Update is called once per frame
        void Update()
    {
        if (TutorialsManager.itemIndex==14)
        {
            isEnd = true;
        }
        if (isEnd)
        {
            isEnd = false;
            StartCoroutine(WaitTutoEnd());
        }
    }
    IEnumerator WaitTutoEnd()
    { 
     yield return new WaitForSeconds(1f);
        if (Input.GetMouseButtonUp(0))
        {
            isTutoButton = false;
            GameManager.CharacterList.Clear();
            SceneManager.LoadScene("Login");
            Destroy(gameObject);
        }
 
    }
  
}
