using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;

public class CheckMail : MonoBehaviour
{

    private static FirebaseFirestore db;

    public CheckMail()
    {
        db = FirebaseFirestore.DefaultInstance;

    }

    public async Task<bool> IsEmailExists(string email)
    {
        CollectionReference playersRef = db.Collection("Players");
        Query query = playersRef.WhereEqualTo("email", email);

        // Get the snapshot of the query result
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Check if there's a matching document
        return snapshot.Count > 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
