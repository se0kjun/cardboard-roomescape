using UnityEngine;
using System.Collections;

public class ShowPaperMsg : MonoBehaviour {
    public GameObject paperObj;
    public string[] _messages;
    public GameObject messageObject;

    private int currentPage;
    private bool isMemo;

	void Start () 
    {
        paperObj = GameObject.Find("paper");
	}
	
    public void showMessage(bool memo, string[] messages)
    {
        paperObj.GetComponent<SpriteRenderer>().enabled = true;
        paperObj.GetComponentInChildren<MeshRenderer>().enabled = true;

        _messages = messages;
        isMemo = memo;

        if (isMemo)
        {
            paperObj.GetComponentInChildren<TextMesh>().text = messages[0].Replace("\\n", "\n");
        }
        else
        {
            paperObj.GetComponentInChildren<TextMesh>().text = messages[0].Replace("\\n", "\n");
            currentPage = 0;
        }
    }

    public void nextMessage()
    {
        if (isMemo)
        {
            DisableText();
        }

        if (currentPage < _messages.Length - 1)
        {
            currentPage++;
            paperObj.GetComponentInChildren<TextMesh>().text = _messages[currentPage].Replace("\\n", "\n");
        }
        else if (currentPage == _messages.Length - 1)
        {
            DisableText();
        }
    }

    public void prevMessage()
    {
        if (isMemo)
        {
            DisableText();
        }

        if (currentPage != 0)
        {
            currentPage--;
            paperObj.GetComponentInChildren<TextMesh>().text = _messages[currentPage].Replace("\\n", "\n");
        }
        else if (currentPage == 0)
        {
            DisableText();
        }
    }

    private void EnableText()
    {
        paperObj.GetComponent<SpriteRenderer>().enabled = true;
        paperObj.GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    private void DisableText()
    {
        paperObj.GetComponent<SpriteRenderer>().enabled = false;
        paperObj.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
}
