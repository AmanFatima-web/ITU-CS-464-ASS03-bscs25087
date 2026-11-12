# CS464 Assignment 03 – Collect, Dodge, Survive

**Student Name:** Aman Fatima  
**Roll Number:** BSCS23001  
**Course:** CS464 – Game Development


Sir, I submitted my assignment late. I kindly request you to please consider compensating me for the late submission, as I had a remed exam immediately after the midterms, and I was also out of station.&#x20;


## Game Overview

Collect, Dodge, Survive is a 3D Unity game developed using GameObjects and C# scripts. The player collects coins, avoids hazards, deals with enemies, and uses power-ups to complete each level.

The game contains five progressively challenging levels. Each new level introduces additional gameplay mechanics while retaining challenges from previous levels.

## Gameplay Video
THE Video was long due to capture of all five levels
[https://drive.google.com/file/d/1O2ZO8PSjwxIMuNMbogAmJdXOE5eQ8GCU/view?usp=sharing]



## Controls

| Key | Action |
|---|---|
| W | Move Forward |
| S | Move Backward |
| A | Move Left |
| D | Move Right |

The player can also use the arrow keys for movement.

## How to Win

- Collect 8 coins to complete the level.
- Avoid hazards and enemies.
- Keep your health above zero.
- Complete the level before the timer runs out in levels where the timer is enabled.
- Use available power-ups to improve your chances of survival.

## Level Progression

| Level | New Gameplay Feature | Combined Mechanics |
|---|---|---|
| Level 1 | Basic coin collection | Collect 8 coins and reach the target score |
| Level 2 | Spikes | Level 1 mechanics + damaging spikes |
| Level 3 | Health pickups and chasing guards | Level 2 mechanics + 3 health pickups + 3 chasing guards |
| Level 4 | Moving crusher | Level 3 mechanics + moving crusher |
| Level 5 | Speed power-ups and patrolling enemies | Level 4 mechanics + 2 speed power-ups + 4 patrolling enemies |

### Level 1 – Basic Collection

The player collects 8 coins to complete the level. This level introduces the basic movement and coin collection mechanics.

**Screenshot:** <img width="1908" height="868" alt="level01" src="https://github.com/user-attachments/assets/a9b9a08b-1c37-4a29-a2b8-0e8beb653c88" />


### Level 2 – Spike Hazards

Spikes are introduced. Touching an active spike decreases the player's health and applies knockback.

**Screenshot:** <img width="1912" height="837" alt="level02" src="https://github.com/user-attachments/assets/640e7813-5024-4e4e-8d48-ac14c5d459d2" />


### Level 3 – Health and Chasing Guards

Three health pickups and three chasing guards are added. Health pickups restore one health point when the player's health is below the maximum. Guards chase the player when they enter their detection range.

**Screenshot:** <img width="1917" height="826" alt="level03" src="https://github.com/user-attachments/assets/09eeaf71-b6aa-4bcb-a24e-7f7411ffc30f" />


### Level 4 – Moving Crusher

A moving crusher is added. It continuously moves up and down, creating another obstacle for the player.

**Screenshot:** <img width="1917" height="836" alt="level04" src="https://github.com/user-attachments/assets/581c658d-489a-44d4-bee2-f542e57de233" />


### Level 5 – Speed Boost and Patrol Enemies

Two speed power-ups and four patrolling enemies are added. Collecting a speed power-up increases player movement speed for 10 seconds. Enemies patrol between two waypoints.

A 2-minute timer is enabled in this level.

**Screenshot:** <img width="1872" height="845" alt="level05" src="https://github.com/user-attachments/assets/fc72b5c7-41bb-463a-90fb-51ba02049bd3" />

## Gameplay Settings

| Feature | Setting |
|---|---:|
| Target score | 8 coins |
| Maximum health | 3 |
| Invulnerability time | 1 second |
| Spike damage | 1 health |
| Spike knockback | 6 |
| Knockback ForceMode | Impulse |
| Guard movement speed | 3 |
| Guard chase range | 7 units |
| Guard stop distance | 1.5 units |
| Guard damage | 1 health |
| Patrol speed | 2 units/second |
| Patrol turning speed | 360 degrees/second |
| Crusher movement height | 3 units |
| Crusher movement speed | 1.5 |
| Player movement force | 10 |
| Speed multiplier | 3× |
| Speed boost duration | 10 seconds |
| Level 5 timer | 120 seconds |

*Note: The actual values can also be changed through Unity Inspector settings.*

## Technical Questions

### 1. Why does a trigger need a Rigidbody on at least one of the two objects?

Unity requires at least one of the interacting objects to have a Rigidbody so that the physics system can detect trigger interactions and generate trigger events.

### 2. Which ForceMode is used for knockback and player movement? Why?

- **ForceMode.Impulse:** Used for knockback because it applies an instant push to the player.
- **ForceMode.Force:** Used for player movement because it continuously applies force while movement input is given.

### 3. What happens if the player collects a second power-up while one is already active?

The existing boost coroutine is stopped and restarted. The speed remains at 3×, and the 10-second duration starts again. The multiplier does not become 9×.

### 4. Why can `==` fail as an arrival check, and when does it work?

Using `==` with floating-point positions can fail because of small precision differences. A distance check such as `Vector3.Distance() < 0.05f` is more reliable. Exact equality works when both values match precisely.


**Aman Fatima**  
BS Computer Science  
Information Technology University (ITU)
