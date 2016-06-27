using UnityEngine;
using System.Collections;

public class ShowStatusMsg : MonoBehaviour {
    private UnityEngine.UI.Text TextModule;
    private Coroutine textCoroutinue;

	void Start () 
    {
        TextModule = GetComponent<UnityEngine.UI.Text>();
	}
	
    public void ShowStatusText(string message)
    {
        TextModule.color = new Color(TextModule.color.r, TextModule.color.g, TextModule.color.b, 255.0f);
        TextModule.text = message;
        //stop previous coroutine
        if (textCoroutinue != null)
            StopCoroutine(textCoroutinue);
        //start coroutine
        textCoroutinue = StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        //show a text for 2 seconds
        float duration = 2.0f; 
        float currentTime = 0f;
        //fadeout using alpha
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            TextModule.color = new Color(TextModule.color.r, TextModule.color.g, TextModule.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
