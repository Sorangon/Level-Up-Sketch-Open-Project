# Sorangon Level Up Sketch Open Project

<p>Here's the open project of my entry for the Real Time VFX monthly sketch challenge #52.
With the theme "Level Up" I've tried to make a character absorbing new data to update himself.
I used some shader tricks to make the character look like a volume of data blocks.</p>

Feel free to explore the project, I've imported the following personal tools to achieve this.
- Gradient and curve Textures generator
- Gradient mapping shaders
- Height Fog custom render pass

Those ones are available in the directory <b>"Assets/SorangonToolset"</b> feel free to use those one your personal or commercial projects.

##Samples maps

You can find 3 sample maps into <b>Assets/Scenes</b>.

### SCN_LevelUp

This scene contains the entire effect submitted for the challenge, 
the whole effect is contained into the Game Object <b>PF_LevelUp</b>,
feel free to explore this prefab content, everything is orchestrated by a Timeline.

!["Complete Level Up Effect"](\~Documentation\LevelUp_Final_Wide.gif)

### SCN_Samples

This scene contains 3 of the mains shader or technical tricks I've used to achieve
this effect.

- Mesh Voxelization
- View based glitch effect
- Generated data pattern

There's a shader for each effect. You can found those into <b>"Assets/Sketch/Samples/Shaders"</b>

Also the data pattern generation is managed by the script <b>"Assets/Global/Scripts/DataPatternGenerator.cs"</b>
the script has no external dependency and could be used as it is on an external project, so feel free to import it on other projects.

!["Sample Scene"](\~Documentation\Sample_Scene.gif)

### SCN_OldTests

This scene contains some of the researches I've done for this sketch. I've tried to make
a glitchy Text Mesh Pro shader but I ran out of time and it seemed to not be important for the 
final effect. 

The shader is still there and you can explore its source files
- Shader <b>"Assets/Global/Shaders/UI/TMP_GlitchySDF"</b>
- Shader Editor GUI <b>"Assets/Global/Editor/Shaders/TMP_GlitchySDFShaderGUI"</b>

!["Tests"](\~Documentation\Old_Tests.gif)


##Additional Informations

The projects runs on <b>Unity 2021.3.4f1 (LTS)</b> with <b>Universal Render Pipeline</b>.

You can follow me on [Artstation](https://www.artstation.com/juliendelaunay) and [Twitter](https://twitter.com/JulienDelauna19) .

##Credits

Fonts
- [Orbitron](https://fonts.google.com/specimen/Orbitron) 

Character
- [YBot Mixamo](https://www.mixamo.com/#/?page=1&query=YBot&type=Character)

Animations
- [Standing 2H Cast Spell 01 Mixamo](https://www.mixamo.com/#/?page=1&query=Standing+2H+Cast+Spell+01&type=Motion%2CMotionPack)
- [Sword And Shield Idle Mixamo](https://www.mixamo.com/#/?page=1&query=Sword+And+Shield+Idle&type=Motion%2CMotionPack)
- [Sword And Shield Power Up Mixamo](https://www.mixamo.com/#/?page=1&query=Sword+And+Shield+Power+Up&type=Motion%2CMotionPack)
