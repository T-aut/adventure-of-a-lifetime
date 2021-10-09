# Adventure of a Lifetime
A unity 2d top-down game project
**Unity version**: 2020.3.18f1 LTS

## Folder structure

- Assets
	- Animations (folder for each animation group)
	- Scripts (folder for scripts relating to groups)
	- Asset Packs (folder for 3rd party asset packs)
	- Scenes (includes scenes for different levels)

### rule of thumb
If your assets can be grouped - put them in a folder :cowboy_hat_face:

## Scene hierarchy structure
Please put loose props/prefabs in a parent empty game object (for e. g. named "Props") to easily group large amounts of assets.

When creating a new empty game object, make sure **it has the position of 0 on all axes**
