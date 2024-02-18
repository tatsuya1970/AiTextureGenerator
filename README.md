# AiTextureGenerator
<br>

## 動作確認環境
- Unity 2021.3.19f1
- Unity 2022.3.10f1
<br>

## 使い方

AIが生成した画像をテクスチャに反映させたいオブジェクトを「ai」というタグにする、

![5bc9184a32623c98c00ec89e8c3845e6](https://github.com/tatsuya1970/AiTextureGenerator/assets/7496610/0e8b05f2-ce89-4d0b-b340-2ed963033ac4)

<br>
こちらから最新版のUnity.packageをUnityへインポートする<br>
https://github.com/tatsuya1970/AiTextureGenerator/releases

<br><br>
AiGenerateというPrehabをHierarchyにアタッチする。
<img width="700" alt="図1" src="https://github.com/tatsuya1970/AiTextureGenerator/assets/7496610/08bb03a4-a947-42ba-a276-0535e158a20b">

<br><br>
AiGenerateのInspectorに以下の要領で必要事項を入力する。
<br>
![02](https://github.com/tatsuya1970/AiTextureGenerator/assets/7496610/a364a131-b3c0-4747-b294-62c6225d4d0d)

<br>
[TexureAiGenerate(Script)]<br>
Api Key : OpenAIのAPIキー<br>
Prompt ：プロンプト<br>
Tiling ：タイルの大きさ（大きいほど小さくで、きめ細やかになる）<br>
<br>
[SkyboxAiGenetrate(Script)]<br>
Api Key : OpenAIのAPIキー<br>
Prompt ：プロンプト<br>

<br><br>
Generate ボタンを押すとAIが画像生成を開始する。<br>
テクスチャ画像は５つ、Skyboxは1つ生成される。<br>
各オブジェクトにテクスチャ画像は５つのうち１つランダムに反映される。<br>
各オブジェクトのマテリアルのMetaricをランダムに設定して、たくさんのテクスチャがあるように見せている。<br><br>

生成された画像ファイルはローカルに保存される。<br>
（保存場所は画像生成が完了したときにコンソールに表示）<br>
<br>
AI作成画像は先ほど設定した「ai」というタグのオブジェクトのマテリアルに反映される。

<br>

## 留意事項

- 国土交通省３D都市モデルPLATEAUの場合、CityGMLのLOD2のみ対応（LOD1には未対応）
- CityGMLをFBX化したオブジェクトには未対応
- OpenAIのAPIのUsage tiersは　「Tier1」 以上を推奨。
  Freeプランだと、頻繁に"Too many Request"のエラーが発生する。
- Skyboxはシームレスな360度画像ではない。
- 屋根の生成はできない。

<br>

## その他
この度、Unity.Packageを初めて公開いたしました。<br>
至らぬ点があるかもしれませんが、ご指導いただけますと幸いです。



