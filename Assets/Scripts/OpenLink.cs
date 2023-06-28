using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void OpenLinkInBrowser(string url)
    {
        Application.OpenURL(url);
    }
}
