# Subway Surfers Clone

Welcome to the Subway Surfers Clone repository! This project is a technical test approved in Module 5 (M5P2) and is designed to replicate the core mechanics of the popular mobile game Subway Surfers.

## Game Mechanics

### Start and End of Game

The game starts with a countdown and the player gains control after the countdown finishes. The end of the game is triggered when the player collides with an obstacle that results in a game over condition.

### Collision and Death

The player's journey is fraught with hazards. Collisions with certain obstacles will result in an immediate game over, while others may simply alter the player's path. It is crucial to adjust the collision boxes to trigger the player's death in case of a collision with the following assets:

- `blocker_jump` - Player dies upon collision
- `blocker_roll`
- `blocker_standard` - Player dies upon collision
- `train_cargo`, `train_cargo_3`, `train_cargo_5` - Player dies upon collision
- `train_ramp`
- `train_standard`, `train_standard_3` - Player dies upon collision
- `train_sub`, `train_sub_3` - Player dies upon collision

### Asset Distribution

Assets are distributed throughout the level to create a challenging experience for the player. Each asset type should have at least five instances placed throughout the level. The naming convention for the assets and their prefabs should follow the original nomenclature, and duplicates should be numbered sequentially (e.g., `tunnel_m(1)`).

## Specifications

- The game should not generate any failures in the `tunnel_` assets that modify or obstruct the player's trajectory.
- Ensure there are no empty spaces at the beginning and end of the level that could interfere with gameplay.
- All assets must follow the original naming convention when creating Prefabs, and copies of these prefabs in the hierarchy should follow the numbering given by the duplicate operation.

## Contributing

Feel free to fork this repository and submit pull requests to contribute to the development of the Subway Surfers Clone. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)

Thank you for checking out the Subway Surfers Clone project!

