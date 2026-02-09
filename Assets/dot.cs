using UnityEngine;

public class LaserSight : MonoBehaviour
{
    [Header("Setup")]
    public Camera fpsCam;           // La tua telecamera
    public float maxDistance = 100f;

    public float dotSizeOnScreen = 0.02f;

    void Start()
    {
        
    }

    void Update()
    {

        RaycastHit hit;

        // Lanciamo il raggio
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, maxDistance))
        {
            // Se colpiamo qualcosa, attiviamo il puntino
            GetComponent<MeshRenderer>().enabled = true;

            // --- RISOLUZIONE FLICKERING ---
            // Posizioniamo il puntino nel punto di impatto + un piccolo offset verso fuori
            transform.position = hit.point + (hit.normal * 0.05f);

            // --- ROTAZIONE CORRETTA ---
            // Facciamo ruotare il puntino in modo che sia "piatto" contro la superficie
            // LookRotation allinea l'asse Z alla normale. 
            transform.rotation = Quaternion.LookRotation(hit.normal);

            // 3. SCALA DINAMICA (Il trucco per la dimensione costante)
            // La formula è: Distanza * FattoreDiGrandezza
            float scaleFactor = hit.distance * dotSizeOnScreen;
            
            // Applichiamo un clamp (limite) minimo per evitare che sparisca se è troppo vicino
            scaleFactor = Mathf.Max(scaleFactor, 0.05f); 

            transform.localScale = Vector3.one * scaleFactor;
        }
        else
        {
            // Se guardiamo il cielo (nulla colpito), nascondiamo il puntino
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}