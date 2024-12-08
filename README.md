# **The Buried Light**

An **asteroids-like**, narrative-driven arcade WebGL game.

---

## **Tech Stack**

- **Unity**: Version 2022.3.22f1  
- **Programming Language**: C#  
- **Dependency Injection**: Zenject  
- **Reactive Programming**: UniRx  

---

## **About The Game**

**The Buried Light** is an extension of the **transmedia narrative project** [Tome of Arçuray](https://tome-of-arcuray.rasitgr.com/). This game expands on the story of the **tree folk**'s rebellion against the oppressive reign of **Erlik**. Set in a visually rich universe, it blends classic arcade gameplay with a modern, narrative-driven twist.

---

## **Gameplay Mechanics**

- **Two-Axis Movement**: Navigate your spaceship freely while avoiding or engaging enemies.  
- **Shooting**: Defend yourself against enemy waves using precise projectile mechanics.  
- **Perk Progression**: Enhance your abilities through scalable upgrades as you progress.

---

## **Design Goals**

- **Scalable Design**: Ensuring mechanics and systems are extensible for future updates or new gameplay features.  
- **LiveOps Integration**: Architected to support dynamic content updates and live events.

---

## **Development Practices**

- **Modular Architecture**: Built using Zenject for dependency injection, promoting clean code and scalability.  
- **Reactive Programming**: UniRx is used for handling events and system communications, allowing for robust and maintainable code.  
- **Physics-based Movement**: Player movement uses Unity’s physics system for realistic momentum and drag.  

---

## **Folder Structure**

The project is organized for maintainability and scalability:  
- **Scripts**  
  - **Data**: Scriptable Objects and Backend logic.  
  - **Gameplay**: Player, enemies, and mechanics logic.  
  - **Managers**: WaveManager, GameManager, and related managers.  
  - **Systems**: Event System, Sound System, Input System, and other standalone systems.
  - **Utilities**: Reusable generic scripts. 
- **Prefabs**  
- **Assets**  

---

## **Getting Started**

### Prerequisites:
- Install **Unity 2022.3.22f1** or later.  
- Ensure you have Git installed for version control.  

### Setup:
1. Clone this repository:  
   ```bash
   git clone https://github.com/pabron7/the-buried-light.git
   cd the-buried-light
2. Open the project in Unity.
3. Build and run the game for WebGL or any preferred platform.

### Contributions:
Contributions are welcome! Follow these steps:

* Fork the repository.
* Create a new branch: feature/YourFeatureName.
* Submit a pull request.

### LICENSE
This project is licensed under the MIT License.

### CONTACT
For questions or suggestions, reach out at: krtalp@gmail.com
