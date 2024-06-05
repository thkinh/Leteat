using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;

public class PasswordManager : MonoBehaviour
{
    public TMP_InputField password;
    public TMP_InputField confirmPassword;
    public Button showPassword;
    public Button hidePassword;
    public Button showConfirmPassword;
    public Button hideConfirmPassword;

    private bool isPasswordVisible = false;
    private bool isConfirmPasswordVisible = false;

    void Start()
    {
        SetPasswordVisibility(false, true);
        SetPasswordVisibility(false, false);

        showPassword.onClick.AddListener(() => TogglePasswordVisibility(true, true));
        hidePassword.onClick.AddListener(() => TogglePasswordVisibility(false, true));
        
        if (confirmPassword != null)
        {
            showConfirmPassword.onClick.AddListener(() => TogglePasswordVisibility(true, false));
            hideConfirmPassword.onClick.AddListener(() => TogglePasswordVisibility(false, false));
        }
    }

    void TogglePasswordVisibility(bool visible, bool isPassword)
    {
        if (isPassword)
        {
            isPasswordVisible = visible;
            SetPasswordVisibility(isPasswordVisible, true);
        }
        else
        {
            isConfirmPasswordVisible = visible;
            SetPasswordVisibility(isConfirmPasswordVisible, false);
        }
    }

    void SetPasswordVisibility(bool visible, bool isPassword)
    {
        if (isPassword)
        {
            if (visible)
            {
                password.contentType = TMP_InputField.ContentType.Standard;
                showPassword.gameObject.SetActive(false);
                hidePassword.gameObject.SetActive(true);
            }
            else
            {
                password.contentType = TMP_InputField.ContentType.Password;
                showPassword.gameObject.SetActive(true);
                hidePassword.gameObject.SetActive(false);
            }
            password.ForceLabelUpdate();
        }
        else if (confirmPassword != null)
        {
            if (visible)
            {
                confirmPassword.contentType = TMP_InputField.ContentType.Standard;
                showConfirmPassword.gameObject.SetActive(false);
                hideConfirmPassword.gameObject.SetActive(true);
            }
            else
            {
                confirmPassword.contentType = TMP_InputField.ContentType.Password;
                showConfirmPassword.gameObject.SetActive(true);
                hideConfirmPassword.gameObject.SetActive(false);
            }
            confirmPassword.ForceLabelUpdate();
        }
    }
}
