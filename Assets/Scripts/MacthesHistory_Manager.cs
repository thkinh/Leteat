using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MacthesHistory_Manager : MonoBehaviour
{
    public GameObject MatchPrefab;
    public Transform contentHolder;
    public GameObject matchDetails;

    private List<Match> m_MatchList;

    // Start is called before the first frame update
    async void Start()
    {
        m_MatchList = new List<Match>();
        m_MatchList = await FirestoreClient.fc_instance.MatchesHistory();
        foreach (Match match in m_MatchList)
        {
            GameObject m = Instantiate(MatchPrefab, contentHolder);
            TMP_Text friendText = m.GetComponentInChildren<TMP_Text>();
            friendText.text = match.date.ToDateTime().ToString();

            Button button = m.GetComponent<Button>();
            if (button != null )
            {
                button.onClick.AddListener(() => {
                    ShowDetails(match);

                });
            }
        }
    }

    public void ShowDetails(Match match)
    {
        matchDetails.SetActive(true);
        TMP_Text match_lobbyId = matchDetails.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text match_date = matchDetails.transform.GetChild(3).GetComponent<TMP_Text>();
        TMP_Text match_time = matchDetails.transform.GetChild(4).GetComponent<TMP_Text>();
        TMP_Text match_exp = matchDetails.transform.GetChild(5).GetComponent<TMP_Text>();

        match_lobbyId.text = "LobbyID: " + match.lobbyId;
        match_date.text = "Date: " + match.date.ToDateTime().ToString();
        match_time.text = "Time: " + match.time.ToString();
        match_exp.text = "Exp Gained: " + match.exp.ToString();

    }

    public async void Reload() 
    {
        foreach(Transform transform in contentHolder)
        {
            Destroy(transform.gameObject);
        }
        m_MatchList = await FirestoreClient.fc_instance.MatchesHistory();
        foreach (Match match in m_MatchList)
        {
            GameObject m = Instantiate(MatchPrefab, contentHolder);
            TMP_Text friendText = m.GetComponentInChildren<TMP_Text>();
            friendText.text = match.date.ToDateTime().ToString();

            Button button = m.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => {
                    ShowDetails(match);

                });
            }
        }
    }
    public void Exit_btn()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitDetails()
    {
        matchDetails.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
