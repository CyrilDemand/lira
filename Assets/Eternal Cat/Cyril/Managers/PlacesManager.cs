using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesManager : MonoBehaviour
{
    // Toutes les pièces du jeu
    private List<RoomNode> allRooms = new List<RoomNode>();

    // Ajoute une salle au gestionnaire
    public void AddRoom(RoomNode room)
    {
        allRooms.Add(room);
    }

    // Trouver une salle par son nom
    public RoomNode GetRoomByName(string name)
    {
        return allRooms.Find(room => room.RoomName == name);
    }

    // Connecte deux salles
    public void ConnectRooms(RoomNode roomA, RoomNode roomB, List<Vector2> teleporters)
    {
        roomA.ConnectTo(roomB, teleporters);
        roomB.ConnectTo(roomA, teleporters); // Connecter dans les deux sens
    }

    // Exemple pour initialiser des pièces
    void Start()
    {
        // Exemple de téléporteurs
        var teleporterA = new Vector2(1, 2);
        var teleporterB = new Vector2(3, 4);

        // Créer les pièces avec leurs téléporteurs
        RoomNode room1 = new RoomNode("Room1", new List<Vector2> { teleporterA });
        RoomNode room2 = new RoomNode("Room2", new List<Vector2> { teleporterB });

        // Ajoute les pièces au gestionnaire
        AddRoom(room1);
        AddRoom(room2);

        // Connecte Room1 et Room2
        ConnectRooms(room1, room2, new List<Vector2> { teleporterA, teleporterB });

        // Affiche le contenu pour vérifier
        Debug.Log(room1);
        Debug.Log(room2);
    }
}