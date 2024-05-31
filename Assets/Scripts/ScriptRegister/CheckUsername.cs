//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CheckUsername : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

using Firebase.Firestore;
using System.Threading.Tasks;

public class CheckUsername
{
    private static FirebaseFirestore db;

    public CheckUsername()
    {
        db = FirebaseFirestore.DefaultInstance;
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
}

