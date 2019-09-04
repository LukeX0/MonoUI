Instructions for using the MonoUI library with any MonoGame project.
----------------------------------------------------------------------------
1. Add a reference to the MonoUI library. (.dll and .xml)
2. Use "using MonoUI" to get access to the namespace.
3. Import all textures through the MonoGame Pipeline Tool.
4. Load the textures with the method Content.Load<Texture2D>();
5. Create the control element(s) by calling the constructor.
6. Call GUI.DrawAll() inside the Draw() method.
----------------------------------------------------------------------------
Useable classes are:

-Control elements-
Button
Check box
Drop-down list
Image
Label
Progress bar
Radio button
Slider
Text box
Tooltip

-Other-
Input
Tool