using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO; //ファイルSave
using System.Collections.Generic; //List<>形を使うため
using System.Collections; // コルーチンのために追加

public class TexureAiGenerate : MonoBehaviour
{
    // Inspectorでユーザーが入力するところ
    public string apiKey; 
    public string prompt;
    public int tiling = 1; 

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
        Debug.Log("Texure AI Generate Start!");
        StartCoroutine(GenerateAI());
    }

    // AI生成のコルーチン
    private IEnumerator GenerateAI()
    {
        string mes = prompt;　//ユーザーが入力した内容を取得

        //入力したプロンプトに以下を足してます。このへんは自由に修正してください
        mes = "textured image of a building wall that looks like "
        + mes;
        
        Debug.Log(mes);

        var url = "https://api.openai.com/v1/images/generations";

        RequestBody body = new RequestBody();
        body.model = "dall-e-2";
        body.prompt = mes;
        body.n = 5;
        body.size = "1024x1024";

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

                // ダウンロードしたテクスチャを格納するリスト
                List<Texture2D> downloadedTextures = new List<Texture2D>();

                // response.data内の全ての画像URLに対してループ
                foreach (var imageData in response.data)
                {
                    // コルーチンでテクスチャをダウンロード
                    yield return StartCoroutine(DownloadAndSetTexture(imageData.url, downloadedTextures));
   
                }

                // "ai"タグを持つ全てのオブジェクトを取得
                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("ai");

                System.Random random = new System.Random();

                foreach (GameObject obj in objectsWithTag)
                {
                    // オブジェクトのマテリアルを取得
                    //Material material = obj.GetComponent<Renderer>().sharedMaterial;

                    Material material = new Material(obj.GetComponent<Renderer>().sharedMaterial);
                    obj.GetComponent<Renderer>().material = material;

                    // テクスチャとシェーダーの設定
                    //material.shader = Shader.Find("Unlit/Texture");
                    material.shader = Shader.Find("Standard");

                    // 利用可能なテクスチャの数に基づいてランダムなインデックスを生成
                    int textureIndex = random.Next(downloadedTextures.Count);

                    // 選択したテクスチャを設定
                    material.mainTexture = downloadedTextures[textureIndex];

                    //マテリアルのTilingの設定（大きいほどきめ細かい）
                    material.mainTextureScale = new Vector2(tiling, tiling);

                    // ランダムなメタリック値（0から1の範囲）を生成
                    float metallicValue = (float)random.NextDouble();
                    // マテリアルのメタリック値を設定
                    material.SetFloat("_Metallic", metallicValue);

                }


                // ダウンロードしたテクスチャを保存（ユニークなファイル名）
                for (int i = 0; i < downloadedTextures.Count; i++)
                {
                    SaveTextureAsPNG(downloadedTextures[i], "texture_" + prompt + i + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
                }
            }
            else
            {
                Debug.Log("Failure");
                yield return new WaitForSeconds(3.0f); // 3秒待機
            }
        }
        request.Dispose();

    }


    // テクスチャのダウンロードと設定のコルーチン
    private IEnumerator DownloadAndSetTexture(string url, List<Texture2D> textures)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(www);
                textures.Add(downloadedTexture);
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
