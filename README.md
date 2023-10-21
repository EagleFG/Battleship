# Battleship
The classic game of Battleship as a single-player game

## How to Play
Press the start button

Drag your pieces onto the board
You can right-click while dragging pieces to switch between horizontal and vertical orientation

When all your pieces are placed, you can begin selecting tiles on the opponent's board to start attacking
After each attack, the opponent will attack a tile on your board

Upon victory or defeat, you can press the restart button to play again

At any time, you can press the quit button to close the game

The debug button shows which tiles on each board are occupied

## Classes
### Board
Generates tiles and keeps track of them

**Variables**
- int _boardWidth - determines number of generated tiles along x-axis (restricted to values between 1 and 99)
- int _boardHeight - determines number of generated tiles along y-axis (restricted to values between 1 and 99)

- Transform _boardOffsetPosition - starting point for generating tiles, moves based on board width and height to ensure board stays centered
- Transform _boardStartingPosition - after generating tiles, board moves to match this transform's position and rotation

- GameObject _tilePrefab - tile prefab that the board generates

- Dictionary<Vector2Int, Tile> _boardTiles - a dictionary keeping track of every generated tile by its position

- bool _isBoardInteractable - states whether the board's tiles can be interacted with by the mouse

- bool _isInDebugView - states whether the board is currently showing debug view

**Functions**
- void Start() - when the board is created, sets board offset, calls for the board to be generated, and moves the board to starting position
- void SpawnTiles() - generates tiles and stores them in dictionary

- public Tile GetTile(Vector2Int) - returns Tile based on its location

- public int GetBoardWidth() - returns board width
- public int GetBoardHeight() - returns board height

- public bool GetIsBoardInteractable() - returns whether board is interactable
- public void SetIsBoardInteractable() - sets if board is interactable, propogates new setting to each tile

- public void SwitchDebugViews() - called by UI button, switches between debug mode on and off
- void DebugViewOn() - sets tile materials to show whether they're occupied
- void DebugViewOff() - sets tile materials back to normal

- void OnDrawGizmos() - shows in the editor where the generated board will be placed

### CameraManager
Manages virtual cameras to switch between

**Variables**
- CinemachineVirtualCamera[] _cameras - array of virtual cameras to be switched between

**Functions**
- public void SwitchCameras(int) - switches to selected virtual camera from array

### GameManager
Dictates game state and keeps track of player scores

**Variables**
- Board _playerBoard - the human player's board
- Board _opponentBoard - the AI player's board

- Piece[] _pieces - the player's pieces

- List<Tile> _tilesAICanAttack - list of tiles on the player's board that have not yet been attacked by the AI player

- OpponentAIBase _opponentAI - class that determines the AI's decision-making process

- GameObject _hitMarker - prefab generated when an attacked tile is a hit
- GameObject _missMarker - prefab generated when an attacked tile is a miss

- int _playerHitCount - how many hits the human player has
- int _opponentHitCount - how many hits the AI player has

- UIManager _uiManager - manager of UI elements
- CameraManager _cameraManager - manager of virtual cameras

**Functions**
- void Awake() - when game manager is created, instantiate list of tiles the AI player can attack
- void OnEnable() - when game manager is enabled, listen for pieces being placed and tiles being selected
- void OnDisable() - when game manager is disabled, stop listening for pieces being placed and tiles being selected

- public void StartPiecePlacementPhase() - sets the player's pieces to be interactable, places AI player's pieces, calls for UI change

- void PlaceOpponentPieces() - calls for each of the AI player's pieces to be placed

- void SetPieceInteractability(bool) - determines the interactability of the player's pieces

- void CheckIfAllPiecesArePlaced() - when a piece is placed, starts turn taking phase if all pieces are placed

- void StartTurnTakingPhase() - sets the player's pieces be uninteractable, calls for camera and UI changes, fills list of tiles the AI can attack, starts human player's turn

- void StartPlayerTurn() - sets the AI player's board to be interactable

- void AttackOpponentTile(Tile) - when an unattacked tile is selected, determines if the attack is a miss or hit; generates marker, updates score, and checks if game is over; triggers victory or starts AI player's turn

- void StartOpponentTurn() - sets the AI player's board to be uninteractable, calls AI to choose a tile to attack, determines if the attack is a miss or hit; generates marker, updates score, and checks if game is over; triggers victory or starts human player's turn

- void ApplyHitMarkerToOpponentTile(Tile) - generates hit marker on tile on AI player's board
- void ApplyHitMarkerToPlayerTile(Tile) - generates hit marker on tile on human player's board
- void ApplyMissMarkerToTile(Tile) - generates miss marker on tile

- void TriggerVictory() - sets the AI player's board to be uninteractable, calls UI change
- void TriggerDefeat() - sets the AI player's board to be uninteractable, calls UI change

### InvisiblePiecePlacer
Abstract class that places a piece onto a board by setting tiles as occupied

**Functions**
- public static void PlacePiece(Board, int) - on a given board, changes a given number of tiles in a line to be occupied after verifying that piece placement is valid
- static Tile[] TryPlacePiece(Board, int) - returns a line of tiles on a board that a piece of a given length could be placed on
- static bool VerifyPlacementValidity(Tile[]) - verifies that the given tiles are all unoccupied

### OpponentAIBase
Base AI class that selects a tile to attack for the AI player

**Functions**
- public virtual Tile ChooseTileToAttack(List<Tile>) - chooses and then removes the first item from a list of tiles

### OpponentAIRandom
Inherits from OpponentAIBase, selects a tile to attack for the AI player

**Functions**
- public override Tile ChooseTileToAttack(List<Tile>) - chooses and then removes a random item from a list of tiles

### Piece
Can be dragged onto the board with the mouse to determine which of the human player's tiles will be occupied

**Variables**
- Segment[] _segments - array of segments belonging to the piece

- Transform _unplacedPosition - position/rotation the piece should return to when dropped in an invalid position

- public bool isDraggable - determines whether the piece can be dragged with the mouse
- bool _isBeingDragged - states whether the piece is being dragged

- Vector3 _mouseOffset - stores the difference in position between the mouse and the piece when it's picked up
- Vector3 _mousePosition - stores the mouse's location when the piece is being dragged

- public static event Action PieceHasBeenPlaced - event to be called when the piece is placed

- bool _isPlaced - states whether the piece is placed on the board

**Functions**
- void Start() - when created, resets the position to the unplaced position

- void OnMouseDown() - when mouse button is pressed on the piece, picks up the piece if draggable
- void OnMouseDrag() - when mouse button is held down on the piece, drags the piece if being dragged
- void OnMouseUp() - when mouse button is released on the piece, drops the piece if being dragged

- void PickUpPiece() - sets the piece as being dragged and unplaced, calls segments to assess tile beneath, rotates piece to horizontal orientation unless already in vertical orientation, sets mouse offset
- void DragPiece() - moves piece with mouse, rotates piece between horizontal and vertical orientations if right mouse button is released
- void DropPiece() - checks if piece can be placed, returns piece to unplaced position or places piece, calls segments to stop assessing, sets the piece as not being dragged

- void UpdateMousePosition() - determines mouse position using a raycast to a plane in the scene

- public void ResetPosition() - moves the piece to the unplaced position

- void PlacePiece() - calls segments to set tiles beneath to occupied, calculates piece position to be centered between first and last tiles, sets piece as placed, broadcasts that piece has been placed

- public bool IsPiecePlaced() - returns whether piece is placed

### Segment
Tile-sized object that indicates whether the piece can be placed and communicates with the tile beneath it

**Variables**
- Tile _occupiedTile - which tile this segment occupies

- Material[] _materials - array of materials to be switched between

- MeshRenderer _renderer - renderer whose material will be switched

- enum IndicatorStatus { Confirmed, Rejected }
- IndicatorStatus _status - states if segment can occupy tile beneath

- bool _isAssessingTileBeneath - determines if segment is assessing the tile beneath it

- Tile _currentTileBeingAssessed - the current tile beneath the segment that is being assessed
- Tile _previousTileAssessed - the previous tile beneath the segment

**Functions**
- void Update() - constantly calls to assess tile beneath if the segment should be assessing

- public bool GetIsAssessingTileBeneath() - returns whether the tile is assessing the tile beneath
- public void SetIsAssessingTileBeneath(bool) - sets if the tile is assessing the tile beneath, sets renderer visibility and material of tile beneath

- void AssessTileBeneath() - determines whether the tile beneath exists and is occupied using a raycast, switches tile and indicator materials

- void SetIndicatorStatus(IndicatorStatus) - sets indicator status, switches indicator material

- void SetMaterial(int) - sets the indicator material from the material array

- void ConfirmAssessedTile(Tile) - calls to set indicator status, switches tile material
- void RejectAssessedTile(Tile) - calls to set indicator status, switches tile material
- void ResetTileMaterial(Tile) - switches tile material

- public bool IsPlaceable() - returns whether segment can be placed

- public Tile GetOccupiedTile() - returns the tile the segment is occupying
- public void SetOccupiedTile() - sets occupied tile to the tile the segment is currently assessing, sets the tile as occupied
- public void RemoveOccupiedTile() - sets the occupied tile as unoccupied, sets occupied tile to null

### Tile
Spaces that compose the game board, determines where pieces and markers can be placed

**Variables**
- Material[] _materials - array of materials to be switched between
- MeshRenderer _renderer - renderer whose material will be switched

- enum TileColorStatus { Normal, Hovered, Confirmed, Rejected }
- TileColorStatus _status - states what material the tile should be using

- public static event Action<Tile> TileHasBeenSelected - event to be called when the tile has been selected with the mouse

- public bool isInteractable - determines whether the tile can be selected with the mouse
- public bool isOccupied - states whether the tile is occupied
- public bool hasBeenAttacked - states whether the tile has been attacked already

**Functions**
- public void SetTileColorStatus(int) - sets tile color status and calls to set tile material
- void SetMaterial(int) - sets tile material from material array

- void OnMouseEnter() - when mouse enters the tile's collider, switch material to hovered if material is normal
- void OnMoustExit() - when mouse exits the tile's collider, switch material to normal if material is hovered
- void OnMouseUpAsButton() - when mouse button is pressed and released on the tile, broadcast that it has been selected if it is interactable

- public static string ConvertTilePositionToName(Vector2Int) - returns a tile's position on its board, given the tile's name
- public static Vector2Int ConvertTileNameToPosition(string) - returns a tile's name, given the tile's position on its board

### UIManager
Enables and disables UI elements, reloads the scene and quits the application

**Variables**
- Button _startGameButton - button that starts the piece placement phase

- GameObject _piecePlacementUI - UI that should show during the piece placement phase
- GameObject _victoryUI - UI that should show on victory
- GameObject _defeatUI - UI that should show on defeat

**Functions**
- public void StartPiecePlacementPhaseUI() - disables start game button, enables piece placement UI
- public void StartTurnTakingPhaseUI() - disables piece placement UI

- public void TriggerVitoryUI() - enables victory UI
- public void TriggerDefeatUI() - enables defeat UI

- public void ReloadScene() - reloads the scene
- public void QuitGame() - quits the application