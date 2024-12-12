using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sprite : MonoBehaviour
{

    Canvas popUpCanvas;
    public ItemCollector player;

    public bool inContact = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<ItemCollector>();
        popUpCanvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!inContact)return;
        if(popUpCanvas!=null) popUpCanvas.transform.LookAt(Camera.main.transform);
        if(Input.GetKeyDown(KeyCode.E)){
            player.CollectItem();
            StartCoroutine(Collected());
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            inContact = true;
            popUpCanvas.enabled = true;
        }
    }

    private IEnumerator Collected(){
        popUpCanvas.enabled = false;
        yield return new WaitForSeconds(4.5f);
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            inContact = false;
            
            popUpCanvas.enabled = false;
        }
    }
}
