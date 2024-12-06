# CRI ADX LE for C#

ADX LEは、サウンドミドルウェア「CRI ADX」を無償でご利用いただけるライトエディションです。  
- https://game.criware.jp/products/adx-le/

[『CRI ADX LE』に関するユーザー使用許諾契約書](CRI_ADX_LE_SDK_License_Agreement_ja.txt)の内容に同意のうえでご利用ください。  

---

- [CRI ADX LE for C#](#cri-adx-le-for-c)
	- [利用条件](#利用条件)
	- [利用方法](#利用方法)
		- [ADX向けリソースファイルの準備](#adx向けリソースファイルの準備)
		- [プロジェクトの設定](#プロジェクトの設定)
			- [dotnet](#dotnet)
			- [Unity](#unity)
	- [サポート・FAQ](#サポートfaq)


## 利用条件

下記条件をすべて満たす場合のみ、ADX LEを使用したコンテンツをライセンス許諾料無償で配信することができます。  
- 前年度年商が1,000万円以下の会社、または団体・個人であること
- コンテンツの配信元が自身であること（販売権を自身で持っていること）
- コンテンツの売上が1,000万円以内であること　※1,000万円を超えた場合は、「ADX」に移行いただきます。

また、ADX LEを使用したコンテンツを配信する場合、著作権表記が必須となります。  
次のページより詳細をご確認ください。  
- https://game.criware.jp/products/adx-le/

## 利用方法

以下は CRI ADX LEの導入方法の案内となります。  
その他の詳細な利用方法は[マニュアル・APIリファレンス](https://game.criware.jp/manual/csharp_plugin/latest/index.html)をご確認ください。  

### ADX向けリソースファイルの準備

CRI ADXで音声再生を行う場合、CRI Atom CraftでADX向け独自形式としてデータを書き出します。  
ADX LEのダウンロードページより、ツールパッケージを取得してください。  
- https://game.criware.jp/products/adx-le/

ツールでのデータ作成方法についてはネイティブSDKのマニュアルもご参照いただけます。
- https://game.criware.jp/manual/native/adx2/latest/criatom_qstart_createdata.html

### プロジェクトの設定

アプリケーションのプロジェクトから各プラグインパッケージへの参照を設定してください。  

#### dotnet

プロジェクトのパッケージ参照として`CriWare.CriAtomLE`を追加してください。  
```
dotnet add package CriWare.CriAtomLE
```

#### Unity

Unity Package Manager向けのパッケージとして次のGit URLを指定してください。  
```
https://github.com/cri-middleware/cri_adx_le_for_csharp.git?path=CriWare.CriAtomLE
```
- https://docs.unity3d.com/ja/2021.3/Manual/upm-ui-giturl.html

## サポート・FAQ

ADX LEのご利用者の皆様向けには個別サポートは行っておりません。  
不具合報告や技術的な質問をしたい場合、下記のリンクをご参考ください。  
- [よくある質問](https://game.criware.jp/products/adx2-le/le-faq/)
- [ADX ユーザー助け合い所（Facebook）](https://www.facebook.com/groups/adx2userj/)
- [ゲームサウンド制作よろず（Discord）](https://discordapp.com/invite/hJn9Cyc)
