using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public GameObject impactEffect; // Prefab per l'impatto

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Effetto visivo dello sparo
        RaycastHit hit;
        // Lancia il raggio dal centro della telecamera in avanti
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Colpito: " + hit.transform.name);

            // Cerca uno script "Target" sull'oggetto colpito per applicare danno
            /*Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }*/

            // Istanzia l'effetto impatto nel punto colpito, ruotato verso la normale della superficie
            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
