using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO; //ファイルSave
using System.Collections; // コルーチンのために追加

public class SkyboxAiGenerate : MonoBehaviour
{
    // Inspectorでユーザーが入力するところ
    public string apiKey; 
    public string prompt;

    //OpenAIのAPIにリクエストするJSON
    [Serializable]
    public class RequestBody
    {
        public string model;
        public string prompt;
        public int n;     //
        public string size;
    }

    //OpenAIのAPIからリターンのあるJSON
    [System.Serializable]
    public class ImageItem
    {
        public string revised_prompt;
        public string url;
    }
    [System.Serializable]
    public class ImageResponse
    {
        public int created;
        public ImageItem[] data;
    }


    public void onClick() //ボタンをクリックしたら呼ばれる 
    {
        Debug.Log("Skybox AI Generate Start!");
        StartCoroutine(GenerateAI());
    }

    private IEnumerator GenerateAI()
    {
        string mes = prompt;　//ユーザーが入力した内容を取得

        //入力したプロンプトに以下を足してます。このへんは自由に修正してください
        mes = "create image suitable for use in virtual reality or panoramic viewers,"
            + "an equirectangular 360-degree panoramic sky of "
            + mes ;

        Debug.Log(mes);

        var url = "https://api.openai.com/v1/images/generations";

        RequestBody body = new RequestBody();
        body.model = "dall-e-3";
        body.prompt = mes;
        body.n = 1;
        body.size = "1792x1024";

        string jsonBody = JsonUtility.ToJson(body);

        //以下、OpenAIのAPIへJSONを投げる
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey); //OPENAIのAPIキー
                                                                    
        Debug.Log("Just a moment");

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            yield return null; // 待機
        }
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.responseCode);

            // OpenAIのAPIが正常にレスポンスがあった場合は以下
            if (request.responseCode == 200 || request.responseCode == 201)
            {
                string text = request.downloadHandler.text;

                // JsonUtilityを使ってJSONデータを解析
                ImageResponse response = JsonUtility.FromJson<ImageResponse>(text);

                // 最初の画像のURLを取得
                string imageUrl = response.data[0].url;

                StartCoroutine(DownloadAndSetTexture(imageUrl));
            }
            else
            {
                Debug.Log("Failure");
                yield return new WaitForSeconds(3.0f); // 3秒待機
            }
        }
        request.Dispose();

    }


    private IEnumerator DownloadAndSetTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)

            //if (UnityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                
            {
                Debug.Log(www.error);
            }
            else
            {
                // ダウンロードしたテクスチャ
                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(www);

                // Skybox用のマテリアルを作成
                Material skyboxMaterial = new Material(Shader.Find("Skybox/Panoramic"));

                // テクスチャをマテリアルに設定
                skyboxMaterial.SetTexture("_MainTex", downloadedTexture);

                // Skyboxとして設定
                RenderSettings.skybox = skyboxMaterial;

                // 必要に応じてSkyboxの更新を強制
                DynamicGI.UpdateEnvironment();

                // ダウンロードしたテクスチャを保存（ユニークなファイル名）
                SaveTextureAsPNG(downloadedTexture, "skybox_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
           
            }
        }
    }

    private void SaveTextureAsPNG(Texture2D texture, string filename)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        byte[] _bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + filePath);
    }

}
