# Genesys Workspace Desktop Edition extension

## Screenshot
![Screenshot](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/blob/master/docs/screenshot01.png)

## Overview
This Genesys Workspace Desktop Edition extension adds value to call center agents using selected Jabra headsets. 

## Build status
![VSTS build status](https://gnaudio.visualstudio.com/_apis/public/build/definitions/45495ae2-8252-4d9e-a321-699be9abf508/100/badge)

## Extension
This extension supports:
-	Genesys Workspace Desktop Edition version 8.5
-	The following Jabra headsets: Jabra BIZ 2300, Jabra LINK 260, Jabra LINK 265 or Jabra BIZ 2400 II CC

## Use case
When an Interaction window is shown – a new tab named _Headset_ is shown. When selecting this tab the name and firmware version of a connected Jabra headset is shown. When the Interaction goes into a state where it is possible to mark is as _Done_ – it can be done from the headset controller box.

### How to implement a custom use case
It is possible to implement another use use. Developer skills are required. This is done by change the implementation and rebuild the software solution. Genesys Workspace Desktop developer information can be found here: https://docs.genesys.com/Documentation/IW
1. [Fork](https://help.github.com/articles/fork-a-repo/) [this](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition.git) GitHub repo
2. Modify [this C# file](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/blob/master/src/JabraInteractionExtension/Models/PresentationModel.cs). Look at the method *CloseInteractionIfPossible()*, and implement your custom use case like this
3. Read the [note](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/blob/master/src/JabraInteractionExtension/Import/note.txt) and rebuild the solution
4. Copy the output files to _C:\Program Files (x86)\GCTI\Workspace Desktop Edition_ to test and debug your custom use case implementation

## How to test this extension
Before deploying the extension to an organization, you can test this extension from a PC with Genesys Workspace Desktop Edition version 8.5 installed:
- Close the Genesys Workspace Desktop Edition software
- Unzip the [zip file](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/releases) and copy the content to _C:\Program Files (x86)\GCTI\Workspace Desktop Edition_
- Start the Genesys Workspace Desktop Edition software

To remove the extension, close the Genesys Workspace Desktop Edition software and remove the files copied from the zip file.

## Deployment
### Option 1
Genesys recommend using the [ClickOnce](https://msdn.microsoft.com/en-us/library/142dbbz4(v=vs.90).aspx) technology for deploying Genesys Workspace Desktop Edition to an organization. Unzip the [zip file](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/releases) and copy the content to _C:\Program Files (x86)\GCTI\Workspace Desktop Edition_ to prepare and publish a new ClickOnce revision as documented here:
[https://docs.genesys.com/Documentation/IW/latest/Dep/DeploymentProcedures](https://docs.genesys.com/Documentation/IW/latest/Dep/DeploymentProcedures)
### Option 2
Use the the dedicated Windows installer (.MSI file) from a [release](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/releases). This installer supports mass deployment scenarios.
