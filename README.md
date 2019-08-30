# Unity-SpriteSheetMaker
A Unity editor tool to make sprite sheets from individual sprites

## Usage
- Drag and drop into an Editor folder in your Unity project.
- Set the necessary settings for individual sprites (can be set automatically with an asset processor, or manually by mass-selecting and setting the right options).  Make sure they're all in the same folder, and that they're all the same size.
  - Texture Type: Sprite (2D and UI)
  - Read/Write Enabled: true
- Open the editor window via ifelse>Sheet Maker in the menu bar (maybe even dock it if you're doing lots of sheets).
- Drag and drop your sprites into the array field at the top.  It'll look like they're all out of order, but if they're named properly, it'll sort the sprites for you.
- Set the max horizontal sprite count.  You can calculate this by dividing the max sprite size you want (let's say 4096px for example) by the sprite width (400px, again for example) and rounding to the nearest integer (result of 10).  The tool will loop the sprites every *n*th sprite.
- Give the file-to-be a name.  I suggest including the word sheet somewhere in there.
- Click on the "Create Sheet" button.
- The tool will spit out a sprite sheet (.png) inside the same folder as the sprites.
- Set settings for the sheet.  My suggestions are:
  - Texture Type: Sprite (2D and UI)
  - Read/Write Enabled: false
  = Sprite Mode: Multiple
  - Generate Mip Maps: false
  - Max Size: whatever your desired size is.  Try to ue the same size as you used when setting the sprite count
- Slice as normal
