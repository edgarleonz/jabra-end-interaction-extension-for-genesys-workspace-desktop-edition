# Genesys Workspace Deskstop Edition extension

## Screenshot
![Screenshot](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/blob/master/docs/screenshot01.png)

## Overview
This Genesys Workspace Desktop Edition extension adds value to callcenter agents using selected Jabra headsets. 

## Build status
![VSTS build status](https://gnaudio.visualstudio.com/_apis/public/build/definitions/45495ae2-8252-4d9e-a321-699be9abf508/100/badge)

## Extension
The extension supports:
-	Genesys Workspace Desktop Edition version 8.5
-	The following Jabra headsets: Jabra BIZ 2300, Jabra LINK 260 or Jabra BIZ 2400 II CC

## Use case
When a Interaction window is shown – a new tab named Headset is shown. When selecting this tab the name and firmware version of a connected Jabra headset is shown. When the Interaction goes into a state where it is possible to mark is as Done – it can be done from the headset controller box.

## How to test this extension
To test this extension (before deploying it to an organization), you must do this on a PC with Genesys Workspace Desktop Edition version 8.5 installed:
- Close the Genesys Workspace Desktop Edition software
- Unzip the [zip file](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/releases) and copy the content to _C:\Program Files (x86)\GCTI\Workspace Desktop Edition_
- Start the Genesys Workspace Desktop Edition software
You remove the extension again, close the Genesys Workspace Desktop Edition software and remove the files from the zip file.

## Deployment
Genesys recommend using the [ClickOnce](https://msdn.microsoft.com/en-us/library/142dbbz4(v=vs.90).aspx) technology for deploying Genesys Workspace Deskstop Edition to an organization.

### How to deploy this extension to an organization
Unzip the [zip file](https://github.com/gnaudio/jabra-end-interaction-extension-for-genesys-workspace-desktop-edition/releases) and copy the content to _C:\Program Files (x86)\GCTI\Workspace Desktop Edition_ to prepare a new ClickOnce revision. This is documented here:
[https://docs.genesys.com/Documentation/IW/latest/Dep/DeploymentProcedures](https://docs.genesys.com/Documentation/IW/latest/Dep/DeploymentProcedures)

