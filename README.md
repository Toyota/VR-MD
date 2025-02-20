# VR-MD
Copyright (c) 2024 TOYOTA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.

日本語は[こちら](./README_jp.md)をご確認ください。

## What is VR-MD?
This is an AR/VR app for chemistry education that runs molecular dynamics simulations (MD) on a smartphone, allowing for realistic reproduction of molecular behavior. It supports 6DoF, enabling users to freely observe molecules by moving with the smartphone in hand. Additionally, by using smartphone VR goggles with an open camera section, users can interact with molecules through hand-tracking features.

The paper is available at:
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
In chemistry, understanding the structure and fluctuations of molecules is crucial. Significant phenomena, such as chemical reactions and phase transitions, are driven by nanoscale dynamics. To visualize these properties in your mind, you need to build knowledge through years of experiments and simulations, much like a chemistry expert. By representing various molecular movements through molecular dynamics simulations and experiencing them in VR, one's understanding of molecules is expected to deepen.

## Requirements
This app operates on Android devices compatible with ARCore and on iOS devices compatible with ARKit.

The development environment uses Unity 2021.3.35f1. If you want to clone and use this repository, the following assets are required:

- HandMR_0.22.1.unitypackage

> [!NOTE]  
> If you are using it on iOS, please prepare the iOS app development environment in advance.

## Installation
### When using Unity:
- Please clone this repository.
- When opening the project in Unity, you might see dialogs such as "Opening Project in Non-Matching Editor Installation" or "Enter Safe Mode?". Please click the "Continue" or "Ignore" buttons, respectively.
- Click on Assets > Import Package > Custom Package… from the menu bar, select the downloaded "HandMR_0.22.1.unitypackage", and click the "Import" button.
- Click Tools > HandMR > Show Start Dialog Window in the menu bar, and configure Steps 1 through 6 in order.
- From the Unity Editor's Project window, open Assets > VRMD > Scenes > Water > Water32.unity.
- Ensure that your smartphone is connected to the PC, then click File > Build Settings in the menu bar to open the dialog.
- Drag Water32 from the Unity Editor's Hierarchy and drop it into the "Scenes In Build" section of the Build Settings dialog. If other scenes are present, check only Water32.
- Confirm that the Platform in the Build Settings dialog is compatible with the connected smartphone, then click the "Build And Run" button.

> [!NOTE]  
> Instead of Step 5 in the Dialog Window, you can downloaded "HandMR_iOS_plugin_for_projects_0.20.unitypackage" separately and import it from Assets > Import Package > Custom Package… in the menu bar.

### When NOT using Unity (Android only):
- The two pre-built APK files of this app are [here](https://github.com/Toyota/VR-MD/releases/latest).
- Please save xr_education.apk to your Android smartphone.
- Tap the saved APK file to install the app.

> [!NOTE]  
> The method for installing APK files may vary depending on the Android smartphone, so please check if necessary.

## Usage
We recommend using clip-on VR goggles for smartphones, considering access to the camera and screen.

Attach the VR goggles to your smartphone and launch the app. Once launched, a molecular model will appear in front of you, and a temperature panel will be displayed below. If the positions are incorrect, tap the smartphone's screen to restart the app, and the molecules and other elements will be repositioned.

### Observing Molecules:
If your hand is not visible within the yellow frame on the screen, the mode will switch to AR, and the molecules will appear to float in the camera view. By moving forward or backward, left or right, and up or down, you can observe the molecules from any direction you prefer.

### Experiencing Molecules:
If your entire hand is visible within the yellow frame on the screen, the mode will switch to VR, displaying both the molecules and your hand in the CG space. By bringing the hand model close to the atomic model, you can touch it, and if you bring it closer as if to pinch it with your fingers, you can hold the atom. A white dashed line is displayed between the hand model and the nearby atomic model. Use the length of the dashed line as a guide to approach the target atom more easily.

Touching or moving the molecules will increase the "current temperature" displayed on the temperature panel. Additionally, you can change to any desired temperature by adjusting the bar on the left side of the temperature panel. You can experience molecular behavior during state changes, such as from solid to liquid to gas, depending on the temperature.

> [!NOTE]  
> #### Single Camera Mode:
> You can observe and experience molecules without using VR goggles for smartphones. When installing with Unity, select XRHandViewerSystem > XRHandSystem from the Unity Editor's Hierarchy, and change the Goggle Mode in DefaultSettingsVR to 1. For Android smartphones, you can also use the xr_education_singlecamera.apk file [here](https://github.com/Toyota/VR-MD/releases/latest).

## License
The source code in this repository is licensed under [the MIT license](./LICENSE).
