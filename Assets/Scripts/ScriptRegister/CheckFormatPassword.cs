using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CheckFormatPassword : MonoBehaviour
{
    //Check Format Email
    public static bool IsValidEmailFormat(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    //kiểm tra độ dài  của password(phải đủ 8 kí tự trở lên)
    public static bool IsPasswordValid(string password)
    {
        if (password.Length < 8)
        {
            Debug.Log("Password must be at least 8 characters long.");
            return false;
        }
        return true;
    }
}
