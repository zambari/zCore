# zCore

_Edit: this repo aged badly. There are still useful snippets there - and I sometimes go back to pick a method or an extension, but overall its not something I would bring into a project in 2025 and onwards._

A collection of useful snippets that I keep around for most projects. You may find it a bit bloated (at first), it doesn't do a single function, it's just a collection of non-coupled scripts that I often use. 

With this release a lot of good code has been added (from the dev branch, but also quite a few bad or not really useful code has been removed. Well, not really removed, you can still find it in LessNeeded.zip, but at least it doesn't consume name space as much.


Speaking of namespace, I know for some people having so many exetnsions will be annoying, I've enclosed some more exotic ones in namespaces, but a lot of this stuff could be done with few lines at most on the fly - the purpose of this set is to save those precious seconds, and in some cases having to add an using clause would mitigate the benefit. 

I guess adding some menu items is a bit more controversial. Don't use them if you don't like htem.

Remote URL:
https://github.com/zambari/zCore.git

This subrepo is meant to go into Assets/plugins (to avoid recompilations when your scripts change)

# Editor Tools

## TildeObjectEnabler

This is the one tool I can't live without. It maps the tilde key (~ - under espace) to object activation / deactivation. I know there's "Alt + Shift + A" but who has the fingers when you can just press tilde. is also capable of traversing the tree up the hierarchy to make sure all the objects up in the chain are enabled too.

## DistributeTool

Distributes selected objects evenly on a line connecting the first one and the last




# Extension Methods

There is quite a few extensions, way too many probably, its a bit overkill, but I so much hate to look for extension methods that this collection tends to grow  a bit






### zExtensions

This is mostly stuff that didn't fit into any of the more specialised extensions, my most used is probably zExt.RandomText(int textLen), but theres constructors for AnimationCurves (zExt.SweepCurve() etc, for when you don't want to start with empty animationCurve), theres DumpKeys extension for Animactino Curves and Gradients - which prints code to console, that you can then copy and paste into actual code to make sure the default values for curve or gradient are what you wantthem to be. 

### zExtensionsVector

A collection of extensions operating on vectors, or using vectors to operate on floats. My favourite would propbably be tatic float MapInversed(this float f, Vector2 minMax), Vector3 Interpolate(this IList<Vector3> positions, float lerpAmt), but theres a few useful ones to operate on individual vector components (i.e. only Y), with a nice touch that you can get VectorComponent from a dropdown and experiment

### zExtensionsColors

Randomize color, tweak hue and saturation etc

### zExtensionsComponents

Some GetComponents tricks, my favourite being AddOrGetComponent

### zExtensionsDatatypes

This is mostly datatype mangling for embedded projects, so lots of byte arrays in and out


### zExtensionsJson

ToJson and FromJson save you the hassle of reaing the file manually


### zExtensionsLayout

This is mostly rubbish, but some of my old code depends on it ;(

### zExtensionsString

Mainly frormattnig methods for string that add richtext tags displayable int he console. For example Use "test".MakeRed().Large() to really pop the console


### zExtensionsRandom

Random value from vector range is probably the most useful here

### zExtensionsRect

A lot of methods for working with RectTransforms

### zExtensionsTextures

Texture tools, CheckDimensions() is a useful family, theres dumpign from and to base64, filliing, conversins between texture2d and retndertexture, methods for creating new texture with identical parameters to a provided one, and some  bsic texture drawing tools


### zExtensionsTransform

Based on two extra enums that can be placed in dropdowns, TransformType and TransformAxis, you can parametrize most strange and quicker make sense from TransformDirection vs Transform Point vs InverseTransformPoint vs InverseTransformVector etc


### zExtensionsUI

Thosre are mostly, null checked calls for example text.SetText("somtething") that checks if text isn't null and just ignores it if it is.


### zListExtensions

Few quick tricks for operating on Lists and Arrays, my most used being .RandomItem();

### zGizmos

Quite a lot of utilites for drawing gizmos, for example DrawCross(Vector3 position, float size = 0.05f), DrawLineDashed(Vector3 position, Vector3 position2, float size = 0.1f), DrawPath(Vector3[] positions), 
    

### zPath

Shortcuts throught file and folder management tasks


### zBench

Most of the methods in zBench are wrappers over System.Stopwatch, plus some dictionaries to keep it a little bit more organized. Can be used to set and query timers to measure exact tax on running a given section of the code. Saves you manually going new System.Diagnosticks.Stopwatch. Don't use for really quick tasks as dictionaries might affect the result

### ClickableEnumttribute

makes yuour dropdown appear as clickable - this is a workaround to UnityEditor 'feature' that freezes any rendering if any dropdown is open. With clickable enuum you can select enum value without stopping the render


# Serializable classes

Serializable classes are visible in editor when you use them as public or serializable fields in your class, basicalyl a poor man's drawer (not requiriing to wrtite or use editor code)

### ModSection

ModSection is a combine tool for processing a float value in a few ways, starting from simple range mapping, to curve mapping, to delays and dampening

### Damper

Damper is basically a wrapper over smoothdamp, you set the target value, and querty UpdatedValue in update, and the value automagically follows

### EventsExtenedWithParameters

Just UnityEvents taking parameters of given type: StringEvent , IntEvent :,FloatEvent, BoolEvent ,ByteArrayEvent ,CharEvent ,Vector3Event,Vector2Event and finally VoidEvent which is just one to one UnityEvent but I wanted to keep naming consistend

# Utils

### ChildrenHide

Provides easy way to hide scene complexity in editor by means of hiding children

### DebugShowUnderCursor

When you are not sure what exactly blocks your raycast, and what objects are occlluded by other objects - this tool can help

### DestroyInBuild

Add to stuff you only want in editor

### GenericSelector

This one sounds overthought but I actually use it a lot. By extending it with an interface type, you can create editor fields that only accept objects that implement some interface.

### TextNameHelper

Automatically renames text objexts on the scene (triggered on selection change) to match their text's contents

# Helpers

Varius scripts not quite fitting to abve categories
### EventToggleGate

Add it to toggle to enable additional events - inverted toggle and seperate toggled on, toggled off events

### AttachRect

Follow a wordlspace object on 2D canvas via camera cast

### BuildVersion

Auto intement build version, display it later. 

### ColorPulse

Slowly pulsing image animation

### DestoryOnTrigger

When you span stuff and want it dissappear when the fall offscreen

### FadeOnShow

A sample implementation that uses canvasGroup or Image to smoothly fade up/down when they are ShowHidden - uses IShowHide

### Flasher

Useful for UI accent animations. Creates a nice smooth fadeot ramp after an initial rapid comeon

### FPSDisplay

Just a plain old FPS display

### ListPopulator / ListItem

I use those quite a lot actually, listitem is a template, that provides basic labeling, coloring, ui object references, and ListPopulator handles spawning and destroying items, simplifies quickly populating a vertical layout with buttons greatly.



### RectGizmo

Adds editor gizmos for selected RectTransforms (Oragne outlines). Usefulf for complex UI work


### ScreenConsole

Grabs Debug.Logs and adds the content to a text object.

### SliderValueDisplay

Add to child text of a Slider to enable displaying of current slider value

### SmoothFollower

Make one transform follow another using both simple delay and damped motion

### SoloActivate / SoloGroup

Whenever one SoloAcrivate gets enabled, it disables all others within the same SoloGroup



### Ruler

Rudimentary tool to measure stuff in 3d Space - a tape maasure basically


### MeshMirror

Provides a geometry mirror functinoality to enable Symmetrty style editing. Works with ProBuilder

### MoveRel

Use MoveRel to move a RectTransform relative to its parent using normalized values. Good for playheads etc, saves remembering strange RectTransform way of doing that

### RectAnchorHelper

This should be an editor tool, I often use it to position panels in UI - uses just anchoring so no absolute coorindates (scales well out of the box)

### MaterialSetBasic

Finds all children meshrenderers and replaces the material 

### MaterialSetParameter

Interfaces with provided material parameter, enabling a bit quicker tweaks via code


### NameUtils

Reversable adding visible tags (with non-visible seperaator, so they can be removed later), plus some unicode bracketing

### ISHowHide

IShowHide is an interface, that requires you to have two methods. Show and Hide. Good thing is they are implemented for all gameobjects by extension methods, so unless you object has and IShowHide component available to handle the transition, the object will be simply set actie or not


# Third party code included

There's bit of code by third parties (in Dependencies folder - I do not claim ownership of those)

Three main examples are

### ExposeMethodInEditor

Add way to trigger methods directly from the inspector, without writing any editor code, i LOVE this

### BrownianMotion

This one is by Keijiro, with slight tweaks. I use it a lot when I just need some random, but natural looking movement

### Readonly

a classic.
