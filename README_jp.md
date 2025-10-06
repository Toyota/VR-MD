# VR-MD
Copyright (c) 2024 TOYOTA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.

## What is VR-MD?
スマートフォンで分子動力学シミュレーション（MD）を実行し、リアルな分子挙動を再現する化学教育用のAR/VRアプリです。
6DoFに対応しているため、スマートフォンを持ちながら動くことで分子を自由に観察できます。
また、カメラ部分が開放されたスマホVR用ゴーグルを利用することで、ハンドトラッキング機能による分子とのインタラクションが可能です。

論文は下記のURLを参照してください。
- https://doi.org/10.1007/978-3-031-34550-0_13

```bibtex
@article{Matsuda_Educational_Effect_of_2023,
  author = {Matsuda, Kenroh and Kikkawa, Nobuaki and Kajita, Seiji and Sato, Sota and Tanikawa, Tomohiro},
  doi = {10.1007/978-3-031-34550-0\_13},
  journal = {Springer, Cham},
  month = jun,
  pages = {183--198},
  title = {{Educational Effect of Molecular Dynamics Simulation in a Smartphone Virtual Reality System}},
  volume = {14041},
  year = {2023}
}
```

## DEMO
![demo](./resource/vrmd.gif)

## Why VR-MD?
化学では、分子の構造と揺らぎを理解することが重要です。化学反応や相転移などの重要な現象は、ナノスケールのダイナミクスによって引き起こされます。
これらの性質を頭の中でイメージするためには、化学の専門家のように長年の実験とシミュレーションを通じて知識を築く必要があります。
分子動力学シミュレーションで様々な分子運動を表現し、VRで体験することで、分子に対する理解が深まることが期待できます。

## Requirements
このアプリは、ARCoreに対応したAndroid上およびARKitに対応したiOS上で動作します。

開発環境にはUnity2021.3.56f2を使用しています。  
本リポジトリをクローンして利用する場合、下記のアセットが必要です。

- HandMR_0.22.1.unitypackage

> [!NOTE]  
> iOSでご利用の場合、事前にiOSアプリ開発環境を準備してください。

## Installation
### Unityを利用する場合:
- 本リポジトリをクローンしてご利用ください。
- Unityでプロジェクトを開くと「Opening Project in Non-Matching Editor Installation」や「Enter Safe Mode?」というダイアログが表示される可能性がありますので、それぞれ「Continue」や「Ignore」ボタンを押してください。
- メニューバーのAssets>Import Package>Custom Package…をクリックし、ダウンロードした「HandMR_0.22.1.unitypackage」を選択して「Import」ボタンを押してください。
- メニューバーのTools>HandMR>Show Start Dialog Windowをクリックし、Step 1~6を順番に設定してください。
- メニューバーのWindow>Package Managerをクリックし、「ARCore XR Plugin」を最新版にアップデートしてください。
- UnityエディタのProjectからAsstes>VRMD>Scenes>Water>Water32.unityを開いてください。
- PCにスマートフォンが接続されていることを確認した上で、メニューバーのFile>Build Settingsをクリックし、ダイアログを開いてください。
- UnityエディタのHierarchyからWater32をドラックし、Build SettingsダイアログのScenes In Buildの欄にドロップしてください。もし、他のシーンが欄にある場合はWater32のみにチェックマークを付けてください。
- Build SettingsダイアログのPlatformが接続しているスマートフォンに対応していることを確認し、「Build And Run」ボタンを押してください。

> [!NOTE]  
> Dialog WindowのStep5の代わりに、HandMR_iOS_plugin_for_projects_0.20.unitypackageを別途ダウンロードし、メニューバーのAssets>Import Package>Custom Package…からインポートすることもできます。

### Unityを利用しない場合(Androidのみ):
- [こちら](https://github.com/Toyota/VR-MD/releases/latest)に本アプリを事前にビルドしたapkファイルが2つあります。
- xr_education.apkをAndroidスマホに保存してください。
- 保存したxr_education.apkをタップし、アプリをインストールしてください。

> [!NOTE]  
> apkファイルのインストール方法はAndroidスマホごとに異なる場合があるため、必要に応じてお調べください。

スマートフォンのカメラや画面へのアクセスを考慮し、クリップ型のスマホ用VRゴーグルを推奨します。 

スマホ用VRゴーグルをスマートフォンに取り付けてアプリを起動してください。
起動後、前方に分子モデル、下方に温度パネルが表示されます。
位置が異なる場合は、スマートフォン本体の画面をタップするとアプリがリスタートし、分子などが再配置されます。

### 分子を観察する:
画面内の黄色の枠内に手が映っていない場合はARとなり、カメラ映像内に分子が浮かんでいるように表示されます。
前後/左右/上下に移動することで、好きな方向から分子を観察することができます。

### 分子を体験する:
画面内の黄色の枠内に手全体が映っている場合はVRとなり、CG空間内に分子と手が表示されます。
手モデルを原子モデルに近づけることで触れることができ、指で摘まむように近づけると原子を持つことも可能です。
手モデルと近接の原子モデルの間に白色の破線を表示しています。破線の長さを目安に手を近づけると狙った原子に触れやすいです。  
  
分子に触れたり、動かすことで温度パネルに表示された「現在の温度」が上昇します。  
また、温度パネルの左側にあるバーを操作することで、任意の温度に変更することができます。
温度によって固体→液体→気体といった状態変化時の分子挙動を体験できます。

> [!NOTE]  
> #### 1画面モード:
> スマホ用VRゴーグルを使用せずに、分子を観察・体験することも可能です。
> Unityを利用してインストールする際に、UnityエディタのHierarchyからXRHandViewerSystem>XRHandSystemを選択し、DefaultSettingsVRのGoggle Modeを1に変更してください。またAndroidスマホの場合は、[こちら](https://github.com/Toyota/VR-MD/releases/latest)のxr_education_singlecamera.apkも利用可能です。


## License
本リポジトリのソースコードは[MITライセンス](./LICENSE)です。
