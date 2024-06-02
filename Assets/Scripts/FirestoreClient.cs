using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Linq;
using Unity.VisualScripting;
using System;

public class FirestoreClient : MonoBehaviour
{
    public static FirestoreClient fc_instance;
    private static FirebaseFirestore db;
    public string thisPlayerID;

    public Player thisPlayer;
    public bool playerisLoaded = false;

    void Awake()
    {
        if (fc_instance == null)
        {
            fc_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (fc_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        thisPlayerID = "XxbEXW5ZIN2P18EmunDF";
    }

    public async void Write(Match match)
    {
        DocumentReference matchRef = db.Collection("Matches").Document(match.id.ToString());
        await matchRef.SetAsync(match);
        Debug.Log("Match added with ID: " + matchRef.Id);
    }

    public async void Write(Lobby lobby)
    {
        DocumentReference lobbyRef = db.Collection("Matches").Document(lobby.id.ToString());
        await lobbyRef.SetAsync(lobby);
        Debug.Log("Match added with ID: " + lobbyRef.Id);
    }

    public async void Write(Player player)
    {
        CollectionReference playerRef = db.Collection("Players");
        DocumentReference docRef = await playerRef.AddAsync(player);
        Debug.Log("Player added with ID: " + docRef.Id);
    }



    public async Task<int> GetDocumentCount(string collectionName)
    {
        QuerySnapshot snapshot = await db.Collection(collectionName).GetSnapshotAsync();
        return snapshot.Documents.Count();
    }



    //Ham lay list friends
    public async Task<List<Relationship>> FetchUserRelationShips(string userID)
    {
        if (string.IsNullOrEmpty(userID))
        {
            Debug.Log("Alo, bi khung khong?");
            return null;
        }

        List<Relationship> relationships = new List<Relationship>();
        DocumentReference docRef = FirebaseFirestore.DefaultInstance.Collection("Players").Document(userID);
        QuerySnapshot snapshot = await docRef.Collection("Relationships").GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Relationship relationship = document.ConvertTo<Relationship>();
            relationships.Add(relationship);
            Debug.Log($"User: {relationship.playerID}, Relationship Type: {relationship.type}");
        }

        return relationships;
    }


    //ham ket ban

    public async void AddUserRelationship(string userID, Relationship relationship)
    {
        CollectionReference coref = db.Collection("Players").Document(userID).Collection("Relationships");
        QuerySnapshot snapshot = await coref.GetSnapshotAsync();
        

        //add ben this user
        DocumentReference userRef = db.Collection("Players").Document(userID).Collection("Relationships").Document($"R{snapshot.Count.ToString()}");
        Task addUserTask = userRef.SetAsync(relationship);


        coref = db.Collection("Players").Document(relationship.playerID).Collection("Relationships");
        QuerySnapshot napshot = await coref.GetSnapshotAsync();
        //add ben the other user
        DocumentReference otherUserRef = db.Collection("Players").Document(relationship.playerID).Collection("Relationships").Document($"R{napshot.Count.ToString()}");
        Relationship reverse_relationship = new Relationship()
        {
            playerID = userID,
            type = relationship.type,
        };
        Task addOtherUserTask = otherUserRef.SetAsync(reverse_relationship);
    }

    //Ham check sign in
    public async Task<string> ReadPassword_ByEmail(string email)
    {
        string pass = string.Empty;
        CollectionReference playersRef = db.Collection("Players");
        Query query = playersRef.WhereEqualTo("email", email);

        // Get the snapshot of the query result
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Check if there's a matching document
        if (snapshot.Count > 0)
        {
            // Get the first document (assuming email is unique)
            DocumentSnapshot document = snapshot.Documents.FirstOrDefault();

            // Deserialize the document data into a Player struct
            Player player = document.ConvertTo<Player>();

            // Get the password from the player struct
            pass = player.password;
            RegisPlayerID(document.Id, player);
        }
        else
        {
            // Handle case where no player with the specified email is found
            Debug.LogWarning("No player found with email: " + email);
        }

        return pass;
    }

    public async Task<string> ReadUsernameByID(string id)
    {
        string username = null;
        DocumentReference docRef = db.Collection("Players").Document(id);
        DocumentSnapshot doc = await docRef.GetSnapshotAsync();

        Player player = doc.ConvertTo<Player>();

        username = player.username;
        
        return username;
    }

    public async Task<bool> IsEmailExists(string email)
    {
        if (db == null)
        {
            Debug.LogError("Firestore is not initialized.");
            return false;

        }

        CollectionReference playersRef = db.Collection("Players");
        Query query = playersRef.WhereEqualTo("email", email);

        // Get the snapshot of the query result
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Check if there's a matching document
        return snapshot.Count > 0;
    }

    public async Task<bool> IsUsernameExists(string username)
    {
        CollectionReference playersRef = db.Collection("Players");
        Query query = playersRef.WhereEqualTo("username", username);

        // Get the snapshot of the query result
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Check if there's a matching document
        return snapshot.Count > 0;
    }
    public void RegisPlayerID(string id, Player player)
    {
        this.thisPlayerID = id;
        playerisLoaded = true;
        thisPlayer = player;
    }


    //Ham tim kiem ban be khong dieu kien
    public async Task<List<Player>> QueryForAllPlayers()
    {
        List<Player> list = new List<Player>();

        CollectionReference collection = db.Collection("Players");
        
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();

        foreach (var item in snapshot.Documents)
        {
            Player player = item.ConvertTo<Player>();
            list.Add(player);
        }
        return list;
    }

    public async Task<Player> GetPlayer(string playerID)
    {
        Player player = new Player();
        DocumentReference doc = db.Collection("Players").Document(playerID);
        DocumentSnapshot snapshot = await doc.GetSnapshotAsync();

        player = snapshot.ConvertTo<Player>();

        return player;
    }

    public async Task<string> GetPlayerID(string username)
    {
        string playerId = null;
        
        Query query = db.Collection("Players").WhereEqualTo("username", username);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        if (snapshot.Count > 0)
        {
            // Get the first document (assuming username is unique)
            DocumentSnapshot document = snapshot.FirstOrDefault();

            // Get the document ID
            playerId = document.Id;
        }
        else
        {
            Debug.LogWarning("No player found with username: " + username);
        }
        return playerId;
    }


    //Ham xem ho so thang ban trong list friend
  
    public async Task<Player> FindPlayer_byName(string username)
    {
        Player player = new Player();

        CollectionReference col = db.Collection("Players");

        Query query = col.WhereEqualTo("username", username);

        QuerySnapshot snapshots = await query.GetSnapshotAsync();

        DocumentSnapshot document = snapshots.FirstOrDefault();
        
        player = document.ConvertTo<Player>();
        return player;
    }

    public async void SendRequest(string username)
    {
        string toID = await GetPlayerID(username);

        CollectionReference colRef = db.Collection("Requests");

        Request request = new Request
        {
            from = FirestoreClient.fc_instance.thisPlayerID,
            to = toID,
            accepted = false,
        };

        await colRef.AddAsync(request);
    }

    //Lay ve list request, check xem accepted co true hay khong de tao object
    public async Task<List<Request>> RetrieveAllRequests()
    {
        List<Request> requests = new List<Request>();

        CollectionReference colRef = db.Collection("Requests"); 

        Query query = colRef.WhereEqualTo("from", FirestoreClient.fc_instance.thisPlayerID);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            Request request = doc.ConvertTo<Request>();
            requests.Add(request);
        }

        return requests;
    }

    public async void Accept (string username)
    {
        string from = await  GetPlayerID(username);

        Relationship relationship = new Relationship
        {
            playerID = from,
            type = "friend",
        };

        AddUserRelationship(from, relationship);

        Query query = db.Collection("Requests").WhereEqualTo("to", FirestoreClient.fc_instance.thisPlayerID).WhereEqualTo("from", from);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        DocumentSnapshot doc = snapshot.FirstOrDefault();

        await doc.Reference.DeleteAsync();
        Debug.Log("Deleted document with ID: " + doc.Id);

    }

    public async void Reject (string username)
    {
        string from = await GetPlayerID(username);
        Query query = db.Collection("Requests").WhereEqualTo("to", FirestoreClient.fc_instance.thisPlayerID).WhereEqualTo("from", from);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        DocumentSnapshot doc = snapshot.FirstOrDefault();

        await doc.Reference.DeleteAsync();
        Debug.Log("Deleted document with ID: " + doc.Id);
    }



}
