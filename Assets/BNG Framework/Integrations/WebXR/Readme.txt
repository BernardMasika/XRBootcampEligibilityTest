1. Install package by going to Window -> Package Manager -> Add New from git Url : https://github.com/De-Panther/unity-webxr-export.git?path=/Packages/webxr

2. Under Project Settings -> XR Management - WebGL make sure "WebXR Export" is checked, as well as "Initialize XR on Startup"

3. Set InputBridge Source to "WebXR" (already set in demo scene)

4. Make sure Demo scene is added to the Build list and first in order

Optional : 

1. You may want to use a custom WebGL template to make sure necessary JS is in order to do things like switch to full screen. De-Panther has some you can install by navigating to Window -> WebXR -> Copy WebGL Templates. More info can be found here : https://github.com/De-Panther/unity-webxr-export/blob/master/Documentation/Getting-Started.md

2. Install demo XR Interactions using same principle as in step 1, but with this url : https://github.com/De-Panther/unity-webxr-export.git?path=/Packages/webxr-interactions  
  - This can give you a quick set of tools to try out in WebGL
  

Tips : 

1. Call "unityInstance.Module.WebXR.toggleVR();" from javascript to switch VR Mode