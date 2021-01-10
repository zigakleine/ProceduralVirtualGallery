using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;

public class BuildingDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(300/500);

        LoadImages loadImages = new LoadImages();

        // Images imgs = loadImages.ClassifyImages();

        // for(int i = 0; i<imgs.images.Length; i++) {
        //     Debug.Log(imgs.images[i] + "\n");
        // }

        // string json = JsonUtility.ToJson(imgs);
        // Debug.Log(json);

        // string url = "http://0.0.0.0:5000/featureExtractionAndKmeans";
 
        // StartCoroutine(CallApi(url, json));

        float[] locXs = {0.0f, -70.0f, 70.0f};    
        float[] locYs = {120.0f, -30.0f, -30.0f};   

        for(int i = 0; i<3; i++) {
        Building b = BuildingGenerator.Generate(loadImages, locXs[i], locYs[i], i);
        Debug.Log(b.Location.ToString());
        BuildingRenderer br = GetComponent<BuildingRenderer>();
        br.Render(b, loadImages, i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator CallApi(string url, string json)
    {
        var request = new UnityWebRequest (url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log(request.downloadHandler.text);
 
        }

    
    }
}
