# VRUnited SDK

The __VRUnited SDK__ is the software library needed to make any Unity project compatible with the __VRUnited__ platform. Using this SDK, you will be able to upload and use your own avatars and scenes in __VRUnited__. 

# Install

Create a new Unity project or open an existing one and make sure that the _Color Space_ is set to _Linear_. 

![](/Documentation~/img/install/0000.png)

Now go to _Window > Package Manager_ and click on the ‘+’ symbol in the top left corner of the new window. Select _Add package from git URL…_

![](/Documentation~/img/install/00.png)

A text field will open. Copy and paste the following URL, and then click on _Add_. 

https://github.com/eventlab-projects/VRUnited_SDK.git

The [Unity Addressables system](https://docs.unity3d.com/Packages/com.unity.addressables@1.19/manual/index.html) is automatically installed as a dependency of the package. Once the installation process finishes, go to _VRUnited > Init Addressable Settings_ and the Addressables system will be automatically configured for you. 

![](/Documentation~/img/install/setup000.png)

Follow the [Wiki](../../wiki) for more in depth information. 
