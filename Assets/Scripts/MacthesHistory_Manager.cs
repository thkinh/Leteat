using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MacthesHistory_Manager : MonoBehaviour
{
    public GameObject MatchPrefab;
    public Transform contentHolder;
    private List<Match> m_MatchList;

    // Start is called before the first frame update
    async void Start()
    {
        m_MatchList = new List<Match>();
        m_MatchList = await FirestoreClient.fc_instance.MatchesHistory();
        foreach (Match match in m_MatchList)
        {
            Debug.Log(match.exp);
            Instantiate(MatchPrefab, contentHolder);
        }
    }

    public async void Reload() 
    {
        m_MatchList = await FirestoreClient.fc_instance.MatchesHistory();
        foreach (Match match in m_MatchList)
        {
            Debug.Log(match.exp);
            Instantiate(MatchPrefab, contentHolder);
        }
    }
    public void Exit_btn()
    {
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
