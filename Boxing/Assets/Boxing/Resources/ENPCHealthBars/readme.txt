Enemy + NPC Health Bars

ENPCHealthBars was developed to be a quick and easy solution to add health bars to enemies, NPCs, or any object at all.

How to use ENPCHealthBars in your own project
-------------------------------------

Simply add the ENPCHealthBar component to any object in the Unity Editor and configure it in the inspector as required. By default, the health bar will always face the main camera. However, you can change the camera via the 'ENPCHealthBar.FaceCamera' property in the inspector or at run time.

To reduce or add to the health bar value, first add the following namespace directive to the top of your C# script:
  using SnazzlebotTools.ENPCHealthBars;

Then simply obtain a reference to the gameobject's ENPCHealthBar component 
  e.g. gameObject.GetComponent<ENPCHealthBar>()
and then add or subtract from its Value property
  e.g. healthBar.Value -= 10;

Please see the demo scene for a quick example of health bars losing and gaining value.

For more information and demonstration video, visit http://snazzlebot.com/tools/

Questions, requests and support:

  web: http://snazzlebot.com/tools/
  email: support@snazzlebot.com
  twitter: https://twitter.com/SnazzlebotGames