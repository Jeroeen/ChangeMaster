using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateName : MonoBehaviour
{
    [SerializeField] private InputField field;

    public void CreateName()
    {
        PlayerPrefs.SetString("PlayerName", field.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
