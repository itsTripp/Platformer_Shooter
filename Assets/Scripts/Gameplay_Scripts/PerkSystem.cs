using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PerkSystem : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] perkCardsToSpawn;

    public List<GameObject> possiblePerkCards = new List<GameObject>();
    public List<int> selectedPerks = new List<int>();
    private static System.Random _randomNumber = new System.Random();

    private int currentIndex = 0;

    public List<GameObject> remainingPerkCards = new List<GameObject>();
    public Text _perkDescription;

    public GameObject chosenPerkCard;

    // Start is called before the first frame update
    void Start()
    {
        possiblePerkCards = perkCardsToSpawn.OrderBy(a => _randomNumber.Next()).ToList();
        SpawnPerkCards();
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click on " + hit.collider.gameObject.name);
                GameControl.gameControl.selectedPerks.Add(hit.collider.gameObject);
                
                //Subtract perkCost from PlayerLevel
                ///Perk Cards are built as scriptable objects and contain the cost of the perk.
                ///I'd prefer to only have that information in the PerkCards script,
                ///instead of the ChoosePerk script. It should be possible, I've just been having issues
                ///with it.
                hit.collider.gameObject.SetActive(false);
                _perkDescription.gameObject.SetActive(false);
                //Place remaining non selected cards back into the possiblePerkCards list
                //StartCoroutine(NextLevel());
                chosenPerkCard = hit.collider.gameObject.GetComponent<ChoosePerk>().perkCard;

                for (int i = 0; i < possiblePerkCards.Count; i++)
                {
                    if(possiblePerkCards[i] == chosenPerkCard)
                    {
                        possiblePerkCards.RemoveAt(i);
                        Debug.Log("Removed: " + chosenPerkCard);
                    }
                }
            }
            _perkDescription.text = "Test Text " + hit.collider.gameObject.name;
            _perkDescription.gameObject.SetActive(true);
            Debug.Log("Mouse is over " + hit.collider.gameObject.name);
        }
        else
        {
            _perkDescription.text = "";
            _perkDescription.gameObject.SetActive(false);
        }
    }

    private void SpawnPerkCards()
    {
        foreach (Transform spawnPoints in spawnPoints)
        {
            if (currentIndex < perkCardsToSpawn.Length)
            {
                GameObject newPerkCardObject = Instantiate(possiblePerkCards[currentIndex],
                 spawnPoints.transform.position, Quaternion.identity) as GameObject;
                newPerkCardObject.GetComponent<ChoosePerk>().perkCard = possiblePerkCards[currentIndex];

                currentIndex++;
            }
            else
            {
                for (int i = 0; i < perkCardsToSpawn.Length; i++)
                {
                    if (!GameControl.gameControl.selectedPerks.Contains(possiblePerkCards[i]))
                    {
                        remainingPerkCards.Add(possiblePerkCards[i]);
                    }
                }
                if (remainingPerkCards.Any())
                {
                    possiblePerkCards = remainingPerkCards.OrderBy(a => _randomNumber.Next()).ToList();
                }
                currentIndex = 0;
            }
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ///I think the selected perk cards should be persistent between scene changes.
        ///Currently the selected perk cards are removed when the next scene loads.
        ///I only want to have a way to ensure the selected perks won't be instantiated again,
        ///when the perk scene loads.
    }
}