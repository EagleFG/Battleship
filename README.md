# Battleship
The classic game of Battleship as a single-player game

## Classes
### Board
Takes width and height as integers
Tiles are parented to the Board Offset when generated
Board Offset moves to ensure the tiles are centered
Tile prefab is used to generate each tile

Listens for when a tile is selected

### Tile
Keeps an array of materials to switch between
Materials should be assigned in the editor in the order of Default, Hovered, Confirmed, Rejected

Static functions to convert a tile's name to its position in the grid and vice-versa

Broadcasts when it's been selected