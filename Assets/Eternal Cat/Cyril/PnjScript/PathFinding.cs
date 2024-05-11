using UnityEngine;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
    // La destination où le GameObject doit se déplacer
    [SerializeField]
    private Vector2 destination;

    private List<Teleporter> tps;
    
    [SerializeField]
    public float moveSpeed = 2f;

    private GameObject roomWhereIsTheCharacter;
    private GameObject roomWhereIsTheDestination;

    public void trouverLaListeDeTeleporteurAPrendre()
    {
        // Obtenez toutes les salles et tous les téléporteurs du GlobalGameManager
        GameObject[] allRooms = GlobalGameManager.instance.getAllRooms();
        Teleporter[] allTp = GlobalGameManager.instance.getAllTeleporteur();

        // Construisez un graphe des salles et des téléporteurs
        RoomGraph graph = new RoomGraph(allRooms, allTp);
        
        Debug.Log("Liste des salles et leurs téléporteurs:");
        foreach (var room in graph.RoomToTeleportersMap)
        {
            Debug.Log($"Salle : {room.Key.name} -> Téléporteurs : {string.Join(", ", room.Value)}");
        }
        
        // Utilisez Dijkstra pour trouver le chemin le plus court
        List<Teleporter> path = DijkstraPath(graph, roomWhereIsTheCharacter, roomWhereIsTheDestination);

        // Affichez le résultat dans la console
        if (path != null)
        {
            Debug.Log("Chemin trouvé : ");
            foreach (var tp in path)
            {
                Debug.Log($"Téléporteur vers {tp.roomDestination.name}");
            }

            tps = path;
        }
        else
        {
            Debug.Log("Aucun chemin trouvé.");
        }
    }
    
    // Algorithme de Dijkstra pour trouver le chemin le plus court
    private List<Teleporter> DijkstraPath(RoomGraph graph, GameObject startRoom, GameObject endRoom)
    {
        if (graph == null)
        {
            Debug.LogError("Graph is null");
            return null;
        }

        if (startRoom == null || endRoom == null)
        {
            Debug.LogError("Start or end room is null");
            return null;
        }

        // Vérifier si les salles de départ et de destination existent dans le graphe
        if (!graph.RoomToTeleportersMap.ContainsKey(startRoom))
        {
            Debug.LogError($"Start room {startRoom.name} not found in the graph");
            return null;
        }

        if (!graph.RoomToTeleportersMap.ContainsKey(endRoom))
        {
            Debug.LogError($"End room {endRoom.name} not found in the graph");
            return null;
        }

        Debug.Log("Dans Dijkstra");
        var distances = new Dictionary<GameObject, float>();
        var previous = new Dictionary<GameObject, Teleporter>();
        var queue = new HashSet<GameObject>();

        // Initialiser les distances et ajouter chaque salle à la file d'attente
        foreach (var room in graph.RoomToTeleportersMap.Keys)
        {
            distances[room] = float.MaxValue;
            queue.Add(room);
        }

        distances[startRoom] = 0;

        // Boucle principale de Dijkstra
        // Boucle principale de Dijkstra
        while (queue.Count > 0)
        {
            // Trouver la salle avec la distance minimale
            GameObject currentRoom = null;
            float minDistance = float.MaxValue;

            foreach (var room in queue)
            {
                if (distances[room] < minDistance)
                {
                    currentRoom = room;
                    minDistance = distances[room];
                }
            }

            // Si `currentRoom` est null, cela signifie que toutes les salles restantes sont inaccessibles
            if (currentRoom == null)
            {
                Debug.LogWarning("Aucune salle accessible trouvée dans la file d'attente.");
                break;
            }

            // Retirer la salle courante de la file d'attente
            queue.Remove(currentRoom);

            // Vérifier les téléporteurs disponibles dans la salle courante
            if (!graph.RoomToTeleportersMap.ContainsKey(currentRoom))
            {
                Debug.LogError($"La salle {currentRoom.name} n'est pas dans RoomToTeleportersMap");
                continue;
            }

            foreach (var teleporter in graph.RoomToTeleportersMap[currentRoom])
            {
                GameObject neighborRoom = teleporter.roomDestination;

                // Vérifier que le téléporteur mène bien à une salle valide
                if (neighborRoom == null || !distances.ContainsKey(neighborRoom))
                {
                    Debug.LogWarning($"Invalid or missing destination room for teleporter in {currentRoom.name}");
                    continue;
                }

                float altDistance = distances[currentRoom] + 1; // Chaque téléporteur a un coût de 1

                if (altDistance < distances[neighborRoom])
                {
                    distances[neighborRoom] = altDistance;
                    previous[neighborRoom] = teleporter;
                }
            }
        }


        // Reconstruire le chemin en utilisant le dictionnaire "previous"
        List<Teleporter> path = new List<Teleporter>();
        GameObject current = endRoom;

        while (previous.ContainsKey(current))
        {
            Teleporter tp = previous[current];
            path.Insert(0, tp);
            current = tp.roomWhereIsTheTP;
        }

        return path.Count > 0 ? path : null; // Retourner le chemin s'il existe
    }

    void Start()
    {
        this.tps = new List<Teleporter>();
        roomWhereIsTheCharacter = FindRoomContainingPosition(GlobalGameManager.instance.getAllRooms(), transform.position);
        roomWhereIsTheDestination = FindRoomContainingPosition(GlobalGameManager.instance.getAllRooms(), destination);
        Debug.Log($"roomWhereIsTheCharacter: {roomWhereIsTheCharacter.name}");
        Debug.Log($"roomWhereIsTheDestination: {roomWhereIsTheDestination.name}");
        if (roomWhereIsTheDestination == null)
        {
            Debug.LogError("Room Destination is null");
        }
        if (roomWhereIsTheCharacter != null)
        {
            Debug.Log($"Personnage est dans la salle: {roomWhereIsTheCharacter.name}");
        }
        else
        {
            Debug.Log("Aucune salle trouvée contenant la position actuelle.");
        }
    }

    public void setDestination(Vector2 newDestination)
    {
        this.destination = newDestination;
        roomWhereIsTheCharacter = FindRoomContainingPosition(GlobalGameManager.instance.getAllRooms(), transform.position);
        roomWhereIsTheDestination = FindRoomContainingPosition(GlobalGameManager.instance.getAllRooms(), destination);
    }


    // Appelé à chaque frame
    void Update()
    {
        // Vérifier s'il y a des téléporteurs dans le chemin à parcourir
        if (tps.Count > 0)
        {
            // Obtenir le téléporteur actuel
            Teleporter currentTp = tps[0];

            // Vérifier si le personnage est proche du téléporteur actuel
            if (Vector2.Distance(transform.position, currentTp.transform.position) < 0.1f)
            {
                // Se téléporter vers la destination du téléporteur actuel
                transform.position = currentTp.destination;

                // Mettre à jour la salle actuelle après la téléportation
                roomWhereIsTheCharacter = currentTp.roomDestination;

                // Supprimer le téléporteur actuel de la liste
                tps.RemoveAt(0);

                // Si tous les téléporteurs ont été utilisés et qu'on est dans la salle finale, aller directement à la destination
                if (tps.Count == 0 && roomWhereIsTheCharacter == roomWhereIsTheDestination)
                {
                    MoveTowardsDestination();
                }
            }
            else
            {
                // Continuer à se déplacer vers le prochain téléporteur
                MoveTowardsDestination(currentTp.transform.position);
            }
        }
        else if (roomWhereIsTheCharacter == roomWhereIsTheDestination)
        {
            // Si déjà dans la salle finale, se diriger vers la position finale
            MoveTowardsDestination();
        }
        else
        {
            // Rechercher la liste de téléporteurs à prendre
            trouverLaListeDeTeleporteurAPrendre();
            Debug.Log("Il est où le TP ?");
        }
    }

    
    void MoveTowardsDestination(Vector2 targetPosition)
    {
        // Calcule la nouvelle position en se déplaçant graduellement vers la destination
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed); // Vitesse fixe de 2.0f
    }

    // Méthode pour déplacer le GameObject vers la destination
    void MoveTowardsDestination()
    {
        // Calcule la nouvelle position en se déplaçant graduellement vers la destination
        Vector2 targetPosition = new Vector2(destination.x, destination.y);
        MoveTowardsDestination(targetPosition);
    }
    
    GameObject FindRoomContainingPosition(GameObject[] rooms, Vector2 position)
    {
        foreach (GameObject room in rooms)
        {
            // Vérifie si le GameObject possède un Collider2D
            Collider2D collider = room.GetComponent<Collider2D>();

            if (collider != null && collider.OverlapPoint(position))
            {
                return room; // Retourne le premier GameObject qui contient le point
            }
        }

        return null; // Aucun GameObject ne contient la position donnée
    }
}