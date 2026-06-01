# My Platformer: The Quest for the Crazy Diamond 💎

A dynamic, action-driven 2D platformer developed in Unity 6. The player embarks on a perilous journey through hazard-filled environments to track down the legendary "Crazy Diamond." 

This project was developed with a strong focus on responsive controls, custom frame-by-frame animations, and a clean, scalable component-based C# architecture.

## 🎮 Gameplay Features
* Precision Platforming: Fluid movement mechanics including running, jumping (with Coyote Time), and a high-speed dash.
* Resource Management: Players must balance Health and Mana. Mana is used to cast magical shards to defeat enemies or destroy obstacles.
* Dynamic Camera System: A robust camera manager utilizing Unity Cinemachine that dynamically adapts the viewport to different rooms (LevelFit) or player actions.
* Custom Pixel Art: All character animations and sprites were custom-drawn frame-by-frame using Aseprite.
* Environmental Hazards: Moving platforms, damageable enemies, and spike traps.

## 🛠 Tech Stack & Architecture
* Engine: Unity 6000.3.15f1 LTS
* Language: C#
* Art Software: Aseprite
* Version Control: Git / GitHub

### Code Architecture Highlights
We prioritized software engineering best practices to ensure the codebase remains scalable and maintainable:
* Interface Segregation: Extensive use of C# interfaces (IDamageable, IMoveable, IDashable, IResourceMutable) to decouple logic. For example, both the Player and Enemies implement IDamageable, allowing the ShardController to interact with them uniformly.
* State-Driven Animation: The Animator state machine is directly driven by real-time physics data (e.g., yVelocity, IsGrounded), completely isolating visual updates from physics calculations.
* Optimized Physics: Transitioned from expensive BoxCast methods to precise Physics2D.OverlapCircle and LayerMask bitwise operations for pixel-perfect ground detection.
* Singleton Managers: Global systems like the CameraManager utilize the Singleton pattern for efficient event triggering across the scene.

## ⌨️ Controls
| Action | Key |
| :--- | :--- |
| Move | A / D |
| Jump | Space |
| Shoot Mana Shard | F |

## 🚀 How to Run the Project
1. Clone the repository:
   ```bash
   git clone [https://github.com/CrANeZ2430/MyPlatformer.git](https://github.com/CrANeZ2430/MyPlatformer.git)