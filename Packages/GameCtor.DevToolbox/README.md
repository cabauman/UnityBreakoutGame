# Dev Toolbox

Provides convenient base functionality for writing tests that verify user interaction.

## Compatibility

This project supports Unity 2018.1 and above. Unity introduced the package manager in Unity 2017.2, but at a very early state.

## Installation into Unity project

### Through the UI

Follow the steps provided by Unity [here](https://docs.unity3d.com/Manual/upm-ui-giturl.html).

The Git URL to use is `https://github.com/cabauman/Unity.UITestKit.git`

### Through OpenUPM

You can use [OpenUPM](https://openupm.com/) to install UITestKit.

1. Follow [these](https://openupm.com/docs/getting-started.html) steps for getting the OpenUPM CLI installed.

2. Go to the project directory in your favorite terminal.

3. Type `openupm add com.gamector.uitesting` in your terminal to install UITestKit.

### Manually through editing manifest.json

1. Read the instructions from the official Unity documentation [here](https://docs.unity3d.com/Manual/upm-git.html).

2. Open up *manifest.json* inside the *Packages* directory in your Unity project using a text editor.

3. Under the dependencies section of this file, you should add the following line at the top:
```"com.gamector.uitesting": "https://github.com/cabauman/Unity.UITestKit.git",```

4. You should now see something like this:
```json
{
  "dependencies": {
    "com.gamector.uitesting": "https://github.com/cabauman/Unity.UITestKit.git",
    "com.unity.burst": "1.0.4",
    "com.unity.mathematics": "1.0.1",
    "com.unity.package-manager-ui": "2.1.2"
  }
}
```

5. You can also specify to use a specific version of UITestKit if you wish by appending # to the Git URL followed by the package version. For example:
```"com.gamector.uitesting": "https://github.com/cabauman/Unity.UITestKit.git#v2.2.0",```

6. Success! Start up Unity with your Unity project and you should see UITestKit appear in the Unity Package Manager.

## How to use

```c#

```

## How to contribute

Please read [CONTRIBUTING](https://github.com/cabauman/Unity.UITestKit/blob/master/CONTRIBUTING.md) regarding how to contribute.
