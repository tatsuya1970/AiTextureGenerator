# AiTextureGenerator


## Getting started

AIが生成した画像をテクスチャに反映させたいオブジェクトを「ai」というタグにする、

こちらから最新版のUnity.packageをUnityへインポートする<br>
https://github.com/tatsuya1970/AiTextureGenerator/releases

<br>
AiGenerateというPrehabをHierarchyにアタッチする。

<br>
AiGenerateのInspectorに以下の要領で必要事項を入力する。
<br><br>
[TexureAiGenerate(Script)]<br>
Api Key : OpenAIのAPIキー<br>
Prompt ：プロンプト<br>
Tiling ：タイルの大きさ（大きいほど小さくで、きめ細やかになる）<br>
<br>
[SkyboxAiGenetrate(Script)]<br>
Api Key : OpenAIのAPIキー<br>
Prompt ：プロンプト<br>

Generate ボタンを押すとAIが画像生成を開始する。<br><br>
生成された画像ファイルはローカルに保存される。<br>
（保存場所は画像生成が完了したときにコンソールに表示）<br>
<br>
AI作成画像は「ai」というタグのオブジェクトのマテリアルに反映される。

