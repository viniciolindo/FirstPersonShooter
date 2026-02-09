using UnityEngine;
using System.Collections;

public class AdvancedShoot : MonoBehaviour
{
    [Header("Statistiche Arma")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 10f; // Colpi al secondo

    [Header("Riferimenti")]
    public Camera fpsCam;           // La telecamera principale per mirare
    public Transform muzzleEnd;     // La punta dell'arma (da dove parte la scia visiva)
    public ParticleSystem muzzleFlash; // Fiammata alla bocca dell'arma
    public GameObject impactEffectPrefab; // Il prefabbricato del foro/scintilla

    [Header("Effetti Visivi")]
    public LineRenderer bulletTracer; // Trascina qui il componente LineRenderer
    public float tracerDuration = 0.05f; // Quanto dura la scia visiva

    private float nextTimeToFire = 0f;

    void Update()
    {
        // Gestione rateo di fuoco
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // 1. Feedback immediato (fiammata)
        if (muzzleFlash != null) muzzleFlash.Play();

        RaycastHit hit;
       

        // Lancia il raggio fisico DALLA CAMERA (per mirare dove guardi)
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Colpito: " + hit.transform.name);

            // --- SOLUZIONE FLICKERING/COMPENETRAZIONE ---
            // Calcola una posizione leggermente staccata dalla superficie (0.01m lungo la normale)
            Vector3 offsetSpawnPos = hit.point + (hit.normal * 0.01f);
            
            // Ruota l'effetto per guardare "fuori" dalla superficie
            Quaternion lookRotation = Quaternion.LookRotation(hit.normal);

            // Istanzia l'effetto nel punto corretto con l'offset
            if (impactEffectPrefab != null)
            {
               GameObject impact = Instantiate(impactEffectPrefab, offsetSpawnPos, lookRotation);
               // Distruggi l'effetto dopo 2 secondi per non intasare la scena
               Destroy(impact, 2f);
            }

            // Applica danno (opzionale)
            // Target target = hit.transform.GetComponent<Target>();
            // if (target != null) target.TakeDamage(damage);
        }

       
    }

   
}
