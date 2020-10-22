# Utily
A Utility library for Unity3D

As Unity3D Developers we all know that Unity3d does come with lots of features right out of the box, But we also know that we need to write some helper codes in addition to our Game code. Things like removing all child objects of a transform or Remaping a number to another range and stuff like this.
This library contains utilities for most things that a Game Developer needs to create faster than before.

I've created Utily to make my own life easier, I just submodule this reposetory whenever i start working on a new project so it contains essential stuff that is needed for almost any project.
I think other people can also find it useful, It's seperated into modules using Assembely definitions and comes with Tools, Helpers and Utilities for many areas of Game Developing.

Currently I'm working on making a documantation for Utily but for now it does not come with any documentation whatsoever, But it's written clean, seperated with helful namespacing and most important it is fast and optimized!

Here is a list of more important parts of Utily
* Collections
  * Generic
    * Dictionary Extentions
    * List Extentions
  * Array Helper
* IO
  * Binary Helper
  * Directory Helper
  * Path Helper
* Linq
  * Extentions
* Reflection
  * MemberInfo Extentions
  * PropertyInfo Extentions
  * Type Extentions
* Security
  * Cryptography
* SystemTypes
  * Enums
  * String extentions
* Threading
  * Threading Utility
* UTime
  * TimeSpan Extentions
* Unity (Actual Unity Utilities are in this Namespace other stuffs are .net Utilities)
  * Editor
    * AddressableAssets
      * Addressables Editor Utilities
    * Serialization
      * ReadOnlyDrawer
      * SerializePropertyDrawer
      * SerializedProperty Extentions
    * EditorGUILayour Helper
    * IO (Unity Editor specefic IO)
    * PlayerSettings Helper
    * Search For Components (Tool)
    * SerializedProperty Extentions
    * UtilyMenu (Utily Internal)
    * UtilySettingsWindow (Utily Internal)
  * Events
    * EventPropagator
      * EventPropagatorBase
      * BeginDragEventPropagator
      * DragEventPropagator
      * DragEventsPropagator
      * DropHandlerEventPropagator
      * PointerClickEventPropagator
      * PointerDownEventPropagator
      * PonterEnterEventPropagator
      * PointerExitEventPropagator
      * PointerUpEventPropagator
    * UnityEventBinder
  * Runtime
    * Components (Just attach these to game objects)
      * DontDestroy
      * FadeMe
      * RotateMe
      * SafeArea (Useful for handling display Notch of new smart phones)
      * SpriteAnimation (Easy Animation for playing sprite sequances)
    * IO (Unity Runtime specefic IO)
      * PlayerPrefs Helper
    * Networking
      * Request Helper
      * UnityWebRequest Awaiter (async)
      * UnityWebRequest Extentions
    * Profiling
      * Profiler Helper
    * Threading
      * Async Helper
      * Async MonoBehaviour
      * Async ScriptableObject
    * UI
      * RectTransform Helper
    * CoExecute (Manage Coroutine Execution and Execute without MonoBehaviour)
    * GameObject Extentions
    * Math (It was the main reason i've created Utily)
    * ObjectPool (Easy Object pooling by just extending this class)
    * Singleton (Extend this class to make it singleton)
    * Strings
    * Texture2D Extentions
    * Texture2D Utility
    * Transform Extentions
  * Serialization
    * ReadOnly Attribute (Use for displaying variable without being able to edit it from editor)
    * SerializeProperty Attribute (Serialize Properties instead of fields)
