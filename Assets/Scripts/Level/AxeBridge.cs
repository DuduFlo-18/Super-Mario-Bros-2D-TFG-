using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Logica para el puente que se cae al golpear el hacha.
public class AxeBridge : MonoBehaviour
{
    // Hacemos un array de segmentos del puente para poder destruirlos uno a uno de forma secuencial. (Son asignados en el editor)
    public GameObject[] bridgeSegements;
    public Transform finalCamPos;
    public GameObject bridgeCollider;

    // Le pasamos a Bowser para que sepa que se ha tocado el hacha y se caiga el puente.
    public Bowser bowser;

    bool bridgeCollapsing;

    // Al tocar el hacha, se desactiva el collider del hacha para que no se pueda volver a tocar y se inicia la caida del puente.
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

    // Coroutine que se encarga de destruir los segmentos del puente uno a uno y de llamar a la funcion de Bowser para que se caiga.
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


        //Una vez tocada el hacha y tirado el punte, mario andará solo (hasta chocar con el Toad).
        Mario.instance.mover.AutoWalk();
        Camera.main.GetComponent<CameraMove>().UpdateRightLimit(finalCamPos.position.x);
    }
}
