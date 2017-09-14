# MyUnityScripts
C# scripts I've created for my own projects 
#WARNING They might have dependencies of third party scripts etc as they are taken
from my projects-SrrY

# Platform Mover
![Alt text](http://i.imgur.com/8xQVUPA.png)

Uses RigidBody MovePosition and MoveRotation to move a rigidbody object. I use this to move platforms and elevators for my
rigidbody character. Elevator mode will move back and forth between the first two points. Use the "active" bool to switch
it on and off. It can rotate to face the next point but always keeps the object level.

# Flight Path
![Alt text](http://i.imgur.com/bDDvLGg.png)

Example usage
![Alt text](https://fat.gfycat.com/UnknownBitterGuineapig.gif)

Does basic 3d pathfinding with a rigid body- on the rb, turn gravity off and interpolate on.
Can fly to/around a target or stop/lookAt when it reaches the target/distance.
