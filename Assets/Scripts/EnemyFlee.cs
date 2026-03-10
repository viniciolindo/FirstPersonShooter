using UnityEngine;
using UnityEngine.AI; // Necessario per la NavMesh

public class EnemyFlee : MonoBehaviour
{
    [Header("Impostazioni Fuga")]
    public float enemySpeed = 5f;
    public float fleeDistance = 10f; // Quanto lontano cerca di andare
    public float startFleeingDistance = 15f; // A che distanza inizia a scappare

    public Transform player;
    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;

        // Trova il giocatore automaticamente (assicurati che abbia il tag "Player")

       
    }

    void Update()
    {
        if (player == null) return;

        // Calcola la distanza tra nemico e giocatore
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Se il giocatore è troppo vicino, scappa!
        if (distanceToPlayer < startFleeingDistance)
        {
            Flee();
        }
    }

    void Flee()
    {
        // --- GEOMETRIA VETTORIALE DELLA FUGA ---
        
        // 1. CALCOLA VETTORE DI FUGA (direzione opposta al giocatore)
        // Formula: (PosizioneMia - PosizioneGiocatore)
        // Questo crea un vettore che "punta via" dal giocatore
        // Esempio: Se io sono a (10, 0) e giocatore a (0, 0), il vettore è (10, 0) → allontanamento
        Vector3 dirToPlayer = transform.position - player.position;

        // 2. NORMALIZZA + MOLTIPLICA PER DISTANZA (estendiamo il vettore)
        // dirToPlayer.normalized = vettore unitario (lunghezza 1) nella direzione corretta
        // Moltiplicare per fleeDistance = allunghiamo il vettore alla distanza desiderata
        // Formula: PosizioneMia + (VettoreFuga * DistanzaDesiderata)
        // Risultato: un punto target a "fleeDistance" metri dalla posizione attuale, nella direzione opposta
        Vector3 newPos = transform.position + dirToPlayer.normalized * fleeDistance;

        // 3. VERIFICA PUNTO SU NAVMESH (geometria del mondo di gioco)
        // NavMesh.SamplePosition cerca il punto valido più vicino a "newPos"
        // Raggio di ricerca: 5 metri (se il punto ideale non è raggiungibile, cerca un'alternativa vicina)
        // Questo evita che il nemico corra fuori dai limiti della mappa o dentro i muri
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(newPos, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}