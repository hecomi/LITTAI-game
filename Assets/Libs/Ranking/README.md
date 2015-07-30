### 使い方

RankingCam をHierarchyに追加

Rankingコンポーネントを取得
Ranking ranking = GameObject("RankingCam").GetComponent<Ranking>();

スコアと撮影対象のゲームオブジェクトを引数にpost()する
ranking.post(score, player);
// このとき、player は Layer:Playerに属しているものとする

### サンプル

testSchenes/main.unity
ボタンを押すと、その時点のボートを真上からorthoカメラで撮影し、指定のサーバにポストする

### 制約

・メッシュが単一と仮定
・真上(y)から見下ろしてz方向をカメラ上向きと決め打ち(変更は出来ます)
・対象とするGameObjectはLayer:Playerに属している物とする