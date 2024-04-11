<<<<<<< Updated upstream
## Task 1:
### -Reduce the batches:
- creating an atlas image using Unity Atlas Packer v2. To achieve this,I used the AtlasImage-1.0.0(https://github.com/mob-sakai/AtlasImage) package, which allows us to select sprites from the atlas in the Unity editor. Additionally, I implement a SpriteManager class to handle the retrieval of sprites for mission and reward icons.

- also I use Enable GPU Instancing feature in Materials .

[![Video Title](http://i3.ytimg.com/vi/OZY6aPJodYk/hqdefault.jpg)](https://www.youtube.com/watch?v=OZY6aPJodYk)

#### - Pooling System:

I Implement a generic pooling system for my Unity project to boost performance and streamline development. By reusing objects instead of constant instantiation and destruction, it minimizes overhead, enhancing runtime efficiency. Its versatility allows pooling for various MonoBehaviour types, offering a clean and maintainable solution. The centralized management and features like cross-scene persistence and dynamic pool size adjustments bring scalability, making game development more enjoyable and efficient.

##### PoolingSystemManager class:

This class is a singleton that manages object pools. It provides methods to get or create a pool for a given prototype, get a pooled object, and return an object to the pool.

```csharp
// Singleton that manages object pools
public class PoolingSystemManager : MonoBehaviour
{
    // Singleton instance
    static PoolingSystemManager instance;
    
    // Dictionary to store object pools
    private Dictionary<Type, object> _pools = new Dictionary<Type, object>();
    
    // Get or create a pool for a given prototype
    public ObjectPool<T> GetOrCreatePool<T>(T prototype, int initialSize = 100) where T : MonoBehaviour
    {
        // ...
    }
    
    // Get a pooled object
    public T GetPooledObject<T>(T prototype, Vector3 position, Quaternion rotation, Transform parent = null) where T : MonoBehaviour
    {
        // ...
    }
}
```

            
##### ObjectPool<T> class :
This class represents an object pool. It stores a list of pooled objects and provides methods to ensure a certain quantity of objects, get a pooled object, and return an object to the pool.

```csharp   
// Represents an object pool
public class ObjectPool<T> where T : MonoBehaviour
{
        // List of pooled objects
        private List<T> pool = new List<T>();
        
        // Ensure a certain quantity of objects
        public void EnsureQuantity(T prototype, int count, Transform parent = null)
        {
            // ...
        }
        
        // Get a pooled object
        public T GetPooled(T prototype, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            // ...
        }
        
        // Return an object to the pool
        public void ReturnToPool(T obj)
        {
            // ...
        }
        
        // Return all objects to the pool
        public void ReturnAllToPool()
        {
            // ...
        }
}
```
## Task 2:
### -Mission System:

## Mission System with Reward System - Design Overview

### Introduction

The Mission System with Reward System is a comprehensive architecture designed for Unity, incorporating an observer pattern, MVC (Model-View-Controller) architecture, and Singleton pattern. The system provides a flexible and extensible framework for managing missions, rewards, and user interfaces.


### Design Principles

#### Observer Pattern with EventManager

The system employs an observer pattern facilitated by the `EventManager`. This pattern promotes loose coupling, allowing different components to communicate without direct dependencies. Events, such as mission completion and reward reception, trigger actions across the system.

#### MVC Architecture

The design adheres to the MVC pattern, distributing responsibilities among the `MissionManager` (controller), various mission classes (models), and `MissionUI` (view). This separation of concerns enhances code organization and readability.

#### Singleton Pattern

Critical components, including `MissionManager`, `RewardManager`, and `MissionUI`, follow the Singleton pattern. This ensures there is only one instance of each class, facilitating global state management and preventing unnecessary instantiations.

#### Unity Editor Integration

Unity-specific features, such as `[SerializeField]` attributes, enable seamless integration with the Unity Editor. Mission and reward data can be easily created and modified within the editor, enhancing the development workflow.

### Key Components

#### MissionManager

The central controller manages active missions, handles mission completion, and dynamically loads mission data. It utilizes a factory method for mission creation, making it extensible to new mission types.

#### RewardManager

Responsible for managing rewards, the `RewardManager` initializes and tracks a list of available rewards. It responds to mission completion events and grants rewards accordingly.

#### EventManager

A robust event system provides a decoupled communication mechanism. Subscriptions, broadcasts, and cleanup operations are handled, promoting modularity and ease of extension.

#### MissionUI

The view component displays active missions through UI elements. It subscribes to events, updates mission data, and ensures a responsive user interface.

### Code Quality and Best Practices

The code exhibits high quality and adherence to Unity best practices:

- **Readability:** The code is well-structured, with clear comments and consistent naming conventions, ensuring readability.
- **Error Handling:** Robust error handling is implemented, addressing potential issues, such as null or empty mission data.
- **Unity Best Practices:** The implementation follows Unity-specific best practices, leveraging features like `[SerializeField]` and Unity events.

### Conclusion

The Mission System with Reward System stands out for its flexibility, maintainability, and scalability. By embracing observer patterns, MVC architecture, and Unity-specific features, it provides a solid foundation for diverse projects. The code quality and design choices contribute to a system that is not only efficient but also easy to extend and maintain over time.

### Extension: Dynamically Generating Reward Classes and Enum Values

To enhance the Mission System with Reward System, a dynamic class and enum generation feature has been implemented. This allows for easy adaptation to changes in the `RewardTypeEnum` by auto-generating new reward classes and updating the `RewardManager` through Unity Editor code.

#### Dynamic Class Generation

A `RewardClassGenerator` has been introduced to automatically create new reward classes based on changes in the `RewardTypeEnum`. This class, when invoked, generates a new reward logic class extending the specified base class. The generated class contains abstract methods that need to be implemented for each reward type.

```csharp
public static class RewardClassGenerator
{
    // ... (previously provided methods)

    public static void GenerateClass(string className, Type baseClassType)
    {
        // ... (previously provided code)

        // Get abstract method names from the base class
        List<MethodInfo> methodNames = GetAbstractMethodNames(baseClassType);

        // Generate abstract methods
        for (int i = 0; i < methodNames.Count; i++)
        {
            code += $"      public override {methodNames[i].ReturnType.Name.ToLower()} {methodNames[i].Name}()" +
                    "\n      {\n         throw new NotImplementedException();\n         }";
        }

        code += "\n   }\n}\n";

        return code;
    }

    // ... (previously provided methods)
}
```

#### Dynamic Enum Generation

An `EnumBuilder` class has been implemented to dynamically create and manage enums based on changes to the `RewardTypeEnum`. This class includes methods to build an enum, generate its code block, and find the enum block within script contents.

```csharp
public static class EnumBuilder
{
    // ... (previously provided methods)

    public static string GenerateEnumBlock(string enumName, string[] enumValues)
    {
        // ... (previously provided code)
    }

    public static string FindEnumBlock(string scriptContents, string enumName)
    {
        // ... (previously provided code)
    }
}
```

#### Enum Management

The `RewardEnumManager` scriptable object has been introduced to store the `RewardTypeEnum`. This object allows for easy modification within the Unity Editor, providing a central location to manage the reward enum.

```csharp
[CreateAssetMenu(fileName = "RewardList", menuName = "MissionsData/Reward List", order = 1)]
public class RewardEnumManager : ScriptableObject
{
    public Enum RewardTypeEnum;
}
```

#### Unity Editor Integration

The generated classes and enums can be managed within the Unity Editor. By changing the `RewardTypeEnum` in the `RewardEnumManager` asset, new classes and enums are auto-generated. Additionally, the `RewardManager` has been updated through editor code to include logic for the newly generated reward classes.

### Conclusion

The dynamic generation of reward classes and enums adds a powerful extension to the Mission System with Reward System. This feature allows developers to easily adapt to changes in the `RewardTypeEnum` by automatically generating the required logic. The Unity Editor integration streamlines the process of managing enums and ensures that the `RewardManager` stays up-to-date with the evolving system. This design choice enhances flexibility, maintainability, and scalability, making the system robust and adaptable to future changes.

### Videos

*Add New Mission

[![Video Title](http://i3.ytimg.com/vi/nHFQkMxQ0gk/hqdefault.jpg)](https://youtu.be/nHFQkMxQ0gk)

--------------------------------------------

*Add New Reward Type with Icon of it.

[![Video Title](http://i3.ytimg.com/vi/I82BTbFLUIM/hqdefault.jpg)](https://youtu.be/I82BTbFLUIM)

--------

***on mission load only select one of each difficulty and after done all of mission start from beginning.

[![Video Title](http://i3.ytimg.com/vi/I82BTbFLUIM/hqdefault.jpg)](https://youtu.be/-UsCjWDpKtk)

-------

*also I fixed the Build Tower in Editor

[![Video Title](http://i3.ytimg.com/vi/9uvsUwp42Sc/hqdefault.jpg)](https://youtu.be/9uvsUwp42Sc)
=======
## Assignment:
1. 3D Environment: Develop a simple 3D level with a basic floor, walls, and obstacles to test the AI. This can be created using Unity's
primitives or basic 3D models.
2. Patrolling: The guard AI should patrol along a fixed route when not alerted. This route should be defined using waypoints in the Unity
Editor. The guard should smoothly move between each waypoint and continue the patrol in a loop.
3. Player Detection: If the player comes into the guard's field of view, the guard should become alerted and start chasing the player. The
guard's field of view should be represented as a cone in 3D space and the guard should only detect the player if there are no obstacles
blocking the line of sight.
4. Chasing Player: When chasing the player, the guard should follow the player's last known location. If the guard reaches this location
without finding the player, it should return to patrolling. The guard should use Unity's NavMesh system for 3D pathfinding while chasing.
5. Sound Detection: If the player makes a loud noise (like running), the guard should investigate the noise. The sound detection should be
based on a radius around the player and shouldn't pass through obstacles.
6. Expandability: The AI system should be designed in such a way that new behaviors could be added in the future without having to
make major changes to the existing system. As such, behaviors should be modular and reusable.


### Overview
[![Video Title](http://i3.ytimg.com/vi/Bb9XLYVj2BY/hqdefault.jpg)](https://youtu.be/Bb9XLYVj2BY)
--------------------------------------------------------------------------------------------------------------
### Documentation: 

#### System Design and Structure:
State Machine Design: The code uses a state machine design pattern, which is a behavioral design pattern that provides a systematic and loosely coupled way to manage states and transitions between states. This is evident in the ChangeState method in the Character class, which changes the current state of the NPC.
Component-Based Design: The code follows a component-based design where different functionalities are encapsulated in separate classes. For example, the SoundDetectorSensor class is responsible for sound detection, and the FieldOfViewSensor class is responsible for field of view detection. This design allows for high cohesion and low coupling, making the code more maintainable and scalable.
Use of Interfaces: The code uses interfaces like IHasPathFinding and ICanHear to define contracts for classes. This ensures that the classes implementing these interfaces will have certain methods, providing a level of abstraction and making the code more flexible and extensible.

#### Expandability:
Modular NPC Behaviors: The NPC behaviors are designed to be modular and reusable. Each behavior is encapsulated in a separate class that extends NpcBehaviorBase<T>. This design allows new behaviors to be easily added in the future without having to make major changes to the existing system.
Expandable Sensor System: The sensor system is designed to be expandable. New types of sensors can be easily added by creating a new class that extends the SensorBase class and implements the required interfaces.
Flexible Pathfinding: The pathfinding system is designed to be flexible. Different pathfinding algorithms can be easily implemented and used by changing the PathfindingAlgorithmEnum.


### How to use and expand the classes:

**1. Character Class:**

This class serves as the base class for all characters in the game. It contains properties and methods that are common to all characters, such as `ChangeState()`, which changes the current state of the character, and `GetStateByName()`, which retrieves a state by its name.
To use this class, you would typically create a new class that inherits from `Character` and override the necessary methods to implement the specific behavior for that character.To expand this class, you could add new properties or methods that are common to all characters. For example, you could add a `Health` property if all characters have health, or a `TakeDamage()` method if all characters can take damage.

**2. NPC Class:**

This class inherits from `Character` and represents a non-player character (NPC) in the game. It contains additional properties and methods that are specific to NPCs, such as `CurrentTarget`, which represents the current target position that the NPC is moving towards, and `SetCurrentTarget()`, which sets the current target position.

To use this class, you would typically create a new class that inherits from `NPC` and override the necessary methods to implement the specific behavior for that NPC.

To expand this class, you could add new properties or methods that are specific to NPCs. For example, you could add a `PatrolRoute` property if all NPCs have a patrol route, or a `FollowPlayer()` method if all NPCs can follow the player.


**3. SensorController Class:**

This class is responsible for managing the sensors of an NPC. It contains a list of `SensorBase` objects, which represent the sensors attached to the NPC, and methods for initializing and updating these sensors.

To use this class, you would typically create a new instance of `SensorController` and add the necessary sensors to it. Then, you could call the `InitializeSensor()` method to initialize a sensor with a set of detection events, and the `UpdateSensor()` method to update the sensors.
To expand this class, you could add new methods for managing the sensors. For example, you could add a `RemoveSensor()` method if you need to remove sensors from the NPC, or a `GetSensorByName()` method if you need to retrieve a sensor by its name.

**4. SensorBase Class:**

This class represents a sensor that can detect certain events. It contains properties for the sensor's name and detection events, and a `Detect()` method that detects the events.
To use this class, you would typically create a new class that inherits from `SensorBase` and override the `Detect()` method to implement the specific detection logic for that sensor.
To expand this class, you could add new properties or methods that are common to all sensors. For example, you could add a `Range` property if all sensors have a range, or a `SetRange()` method if you need to set the range of a sensor.

**5. StateMachine Class:**

This class represents a state machine that can transition between different states. It contains a list of `IBaseState` objects, which represent the states of the state machine, and methods for changing the current state and updating the state machine.
To use this class, you would typically create a new instance of `StateMachine` and add the necessary states to it. Then, you could call the `ChangeState()` method to change the current state, and the `UpdateStateMachine()` method to update the state machine.
To expand this class, you could add new methods for managing the states. For example, you could add a `RemoveState()` method if you need to remove states from the state machine, or a `GetStateByName()` method if you need to retrieve a state by its name.


### Create a WayPoints for NPC

Added GuardNPC1 prefab to the scene, based on the name of NPC prefab in the below game objects been created in Scene, To add a new Waypoint click on the “Add a new Waypoint” button as the below images. 

>>>>>>> Stashed changes
