# T22.AD- Simulator

This project aims to reduce the costs of developing and testing the automata that will be placed on T22 as a prototype for future vehicles.

## Instalation and Setup

### Development environment

This program as of the current date doesn't have a proper "production" environment, meaning you will need both unityhub and **Unity3D-2021.3.11f1** the ladder being strictly necessary for the project development environment.

For the aforementioned you will need around 6gb-8gb of free disk space. Although not strictly necessary, you should download documentation, linux environment and windows environment packages to make your life easier.

#### Common issues

##### Project has invalid dependancies com.unity.robotics.ros-tcp-connector

As of now, there is an issue related to asset dependancies, so unity will complain about "error occured while resolving packages" more specifically with ROS related problems, **to fix it go to Asset Manager and add the ROS-TCP-Connector package to it from Unity Technologies.**

To save you a trip to this [url](https://github.com/Unity-Technologies/ROS-TCP-Connector), here's a small transcription:

1. Using Unity 2020.2 or later, open the Package Manager from `Window` -> `Package Manager`.
2. In the Package Manager window, find and click the + button in the upper lefthand corner of the window. Select `Add package from git URL....`
   [![image](https://user-images.githubusercontent.com/29758400/110989310-8ea36180-8326-11eb-8318-f67ee200a23d.png)](https://user-images.githubusercontent.com/29758400/110989310-8ea36180-8326-11eb-8318-f67ee200a23d.png)
3. Enter the git URL for the desired package. Note: you can append a version tag to the end of the git url, like `#v0.4.0` or `#v0.5.0`, to declare a specific package version, or exclude the tag to get the latest from the package's `main` branch.
   1. For the ROS-TCP-Connector, enter `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector`.
4. Click `Add`.

##### Ghostly left mouse click on linux environments

If your left mouse click seems to not be able to interact) with the linux UI(most of the time), theres a known issue relating to pipewire/pulseaudio/alsa that weirdly makes UnityEditor to not respond often to mouse clicks, especially when it comes with the ui

To fix it just go to `Edit` -> `Project Settings`, then a popup window should appear, select `Audio` on the left tabs, and search for Disable Unity Audio and make shure that the checkbox has a check on it

## Contributing

To contribute you should either follow the recruitment program or talk with the current department leader of the Autonomous Driving department check our website for more details

For team members, Contributing details and procedures are detailed on the teams scrum board
