using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AxeBridge : MonoBehaviour
{
    public GameObject[] bridgeSegements;
    public Transform finalCamPos;
    public GameObject bridgeCollider;

    public Bowser bowser;

    bool bridgeCollapsing;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!bridgeCollapsing)
            {
                bowser.collapseBridge = true;
                bridgeCollapsing = true;
                GetComponent<CircleCollider2D>().enabled = false;
                Mario.instance.mover.StopMove();
                //Para que no siga avanzando el tiempo
                LevelManager.instance.levelPaused = true;
                //La Corotuina tiene que ser al final porque si antes bloquea a Mario.
                StartCoroutine(FallBridge());
            }
        }
    }
    IEnumerator FallBridge()
    {
        if (!bowser.isBowserDead)
        {
            foreach (GameObject segment in bridgeSegements)
            {
                Destroy(segment);
                yield return new WaitForSeconds(0.1f);
            }
            Destroy(bridgeCollider);
            bowser.FallBridge();
            yield return new WaitForSeconds(1.5f);
        }
       
        
        //Una vez tocada el hacha y tirado el punte, mario andará solo.
        Mario.instance.mover.AutoWalk();
        Camera.main.GetComponent<CameraMove>().UpdateRightLimit(finalCamPos.position.x);
    }
}
