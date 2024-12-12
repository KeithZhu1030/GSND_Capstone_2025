using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int totalItemsToCollect = 5; 
    private int currentCollectedItems = 0; 
    public Text uiText; 
    public GameObject hiddenObject; 
    public Transform activationZone; 
    public float activationZoneRadius = 2f; 
    private bool canActivate = false; 

    public CompanionFollow companion;
    void Start()
    {
        UpdateUI();
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false); 
        }
    }

    void Update()
    {
        
        if (canActivate && Input.GetKeyDown(KeyCode.E))
        {
            ActivateHiddenObject();
        }
    }

    
    void UpdateUI()
    {
        if (uiText != null)
        {
            uiText.text = "Collected: " + currentCollectedItems + " / " + totalItemsToCollect;
        }
    }

    
    public void CollectItem()
    {
        currentCollectedItems++;
        if(companion!=null)companion.UpdateNumCollectedSprites(currentCollectedItems);
        UpdateUI();

        if (currentCollectedItems >= totalItemsToCollect)
        {
            canActivate = true;
            Debug.Log("All items collected! Go to the activation zone and press 'E'.");
        }
    }

    
    void ActivateHiddenObject()
    {
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(true); 
            Debug.Log("Hidden object is now visible!");
            ClearProgress();
        }
    }


    void ClearProgress()
    {
        currentCollectedItems = 0;
        if(companion!=null)companion.UpdateNumCollectedSprites(currentCollectedItems);
        UpdateUI();
        canActivate = false; 
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            CollectItem(); 
            Destroy(other.gameObject);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {

            if (Vector3.Distance(other.transform.position, activationZone.position) <= activationZoneRadius)
            {
                Debug.Log("Press 'E' to activate the hidden object.");
            }
        }
    }
}
