using Assets.Scripts.Entities;
using Assets.Scripts.Entities.LocalData;
using Assets.Scripts.Entities.Utility;
using Assets.Scripts.Enums;
using Assets.Scripts.Services;
using Assets.Scripts.Services.LocalData;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleSceneController : MonoBehaviour
{
    [Tooltip("Area to render the hexagon map")]
    public Transform HexagonsArea;
    [Tooltip("Area to render the hexagon marks")]
    public Transform MarksArea;
    [Tooltip("Area to render the hexagon marks of player sides")]
    public Transform SideMarksArea;
    [Tooltip("Canvas transform")]
    public Transform UICanvas;

    public Camera MainCamera;

    [Header("Buttons references")]
    public UIButton ButtonPositionSet;
    public GameObject TurnButtonsGroup;
    public UIButton ButtonAttack;
    public UIButton ButtonMovement;
    public UIButton ButtonUndoMovement;
    public UIButton ButtonSkill;
    public UIButton ButtonItem;
    public UIButton ButtonEndTurn;
    public UIButton ButtonCameraLock;

    public UIAttackPreview _attackPreview;

    private long _currentPlayerId = 1; //TODO: PEGAR DO PLAYERPREFS

    //TODO: Jogar para o arquivo de constantes
    private const int MAX_MAP_MATRIX_X = 12;
    private const int MAX_MAP_MATRIX_Y = 12;
    private const float STEP_HEX_POS_X = 4.33f;
    private const float STEP_HEX_POS_Y = 5f;
    private const float ELEVATION_MDF = 1f;

    private HexData[,] _mapMatrix = new HexData[MAX_MAP_MATRIX_X, MAX_MAP_MATRIX_Y];
    private HexMark[,] _markMatrix = new HexMark[MAX_MAP_MATRIX_X, MAX_MAP_MATRIX_Y];
    private int _mapMatrixHalfX;
    private int _mapMatrixHalfY;

    private Quaternion _defaultRotation = new Quaternion(0f, 0f, 0f, 0f);

    private IntVector2[] _directionMatrix = new IntVector2[6];

    private List<CharacterGameplay> _allHeroes = new List<CharacterGameplay>();
    private List<CharacterGameplay> _gameHeroes = new List<CharacterGameplay>();
    private List<HeroController> _heroesControllers = new List<HeroController>();

    private float _nextToastAllowed = -999;
    private UIToast _staticToast;

    #region Popups

    private PopupCharSelection _popupCharSelection;
    private UIConfirm _confirmBox;

    #endregion

    #region Resources

    private Object _heroPrefab;
    private Object _popupCharSelectionPrefab;
    private Object _confirmBoxPrefab;
    private Object _toastPrefab;

    private Object _hexMarkSetPositionPrefab;
    private Object _hexDirectionPrefab;
    private Object _hexMoveOptionPrefab;
    private Object _hexPlayerTurnPrefab;
    private Object _hexAllyPrefab;
    private Object _hexEnemyPrefab;
    private Object _hexAttackOptionPrefab;

    #endregion

    #region Path Finder

    private int _pathFinder_maxSteps = 0;
    private int _pathFinder_jump = 0;
    private int _pathFinder_bestFound;
    private int _pathFinder_goalX;
    private int _pathFinder_goalY;
    private List<PathRef> _pathFinder_goalPath;
    private List<PathRef> _pathFinder_viableSpots;
    private List<IntVector2> _pathFinder_enemySurrounds;
    private bool _pathFinder_isViableSearch;
    private bool _pathFinder_isAttackRange;

    #endregion

    #region Turn Controls

    private TurnPhase _turnPhase = TurnPhase.Positioning;
    private TurnSubPhase _turnSubPhase = TurnSubPhase.NotSet;
    private int _player1;
    private int _player2;
    private long? _playerTurn;
    private long? _characterTurn;
    private bool _movementDone;
    private bool _attackDone;
    private IntVector2 _originMoveHex;

    private bool _player1_positioningDone = false;
    private bool _player2_positioningDone = false;

    private IntVector2? _auxTargetSpot;
    private List<TurnGauge> _turnGauge = new List<TurnGauge>();
    private CharacterKey[] _turnOrder = new CharacterKey[8];

    #endregion

    #region Combat

    int _attackDamage = 0;
    float _attackAccuracy = 0f;
    bool _attackFromBehind = false;

    #endregion

    #region Camera

    private Vector3 _cameraTarget = new Vector3(0f, 0f, 0f);
    private Vector3 _lockedCameraTarget = new Vector3(0f, 0f, 0f);
    private Vector3? _cameraTranslateTarget = null;
    private float _cameraAngle = 0;
    private Vector3 _cameraRotationAngle = new Vector3(0f, 0f, 1f);
    private Vector3 _cameraYOffset = new Vector3(0f, 14f, 0f);
    private Transform _cameraMobileTarget = null;
    private bool _cameraIsLocked = true;
    private Vector3 _viewportCenter = new Vector3(0.5f, 0.5f, 0f);

    private Vector3? _dragStartPosition;
    private Vector3? _dragCurrentPosition;
    private Vector3? _dragMobileAnchor;

    #endregion

    #region Local Services

    BaseEquipService _svc_baseEquip;
    BaseEquipService BaseEquipService
    {
        get
        {
            if (_svc_baseEquip == null)
                _svc_baseEquip = new BaseEquipService();

            return _svc_baseEquip;
        }
    }

    BaseWeaponService _svc_baseWeapon;
    BaseWeaponService BaseWeaponService
    {
        get
        {
            if (_svc_baseWeapon == null)
                _svc_baseWeapon = new BaseWeaponService();

            return _svc_baseWeapon;
        }
    }

    #endregion

    #region Start

    private int _numPlayersLoaded = 0;

    #endregion

    private bool _isPopupActive = false;

    void Start()
    {
        _mapMatrixHalfX = System.Convert.ToInt32(System.Math.Floor((float)MAX_MAP_MATRIX_X / 2f));
        _mapMatrixHalfY = System.Convert.ToInt32(System.Math.Floor((float)MAX_MAP_MATRIX_Y / 2f));

        //LoadData();
        LoadResources();
        InstantiatePopups();
        ConfigureReferences();

        //TODO: COLOCAR NO GAME CONSTS
        _directionMatrix[0] = new IntVector2(0, -1); //Top
        _directionMatrix[1] = new IntVector2(1, 0); //Top Right
        _directionMatrix[2] = new IntVector2(1, 1); //Bottom Right
        _directionMatrix[3] = new IntVector2(0, 1); //Bottom
        _directionMatrix[4] = new IntVector2(-1, 0); //Bottom Left
        _directionMatrix[5] = new IntVector2(-1, -1); //Top Left

        //TODO: Mudar para consulta do banco e controlar se está pronto por variável
        _mapMatrix = new HexData[MAX_MAP_MATRIX_X, MAX_MAP_MATRIX_Y];

        _mapMatrix[8, 4] = new HexData(8, 4, 0, 2, 1);
        _mapMatrix[8, 5] = new HexData(8, 5, 0, 2, 1);
        _mapMatrix[8, 6] = new HexData(8, 6, 1, 2, 1);
        _mapMatrix[8, 7] = new HexData(8, 7, 1, 2, null, null, 1, 0);
        _mapMatrix[8, 8] = new HexData(8, 8, 2, 2);
        _mapMatrix[8, 9] = new HexData(8, 9, 3, 2, null, null, 1, 0);
        _mapMatrix[8, 10] = new HexData(8, 10, 4, 2, null, null, 1, 0);
        _mapMatrix[7, 3] = new HexData(7, 3, 0, 2, 1);
        _mapMatrix[7, 4] = new HexData(7, 4, 0, 2, 1);
        _mapMatrix[7, 5] = new HexData(7, 5, 0, 2, 1);
        _mapMatrix[7, 6] = new HexData(7, 6, 1, 2);
        _mapMatrix[7, 7] = new HexData(7, 7, 2, 2);
        _mapMatrix[7, 8] = new HexData(7, 8, 3, 2);
        _mapMatrix[7, 9] = new HexData(7, 9, 4, 2);
        _mapMatrix[7, 10] = new HexData(7, 10, 5, 2, null, null, 1, 0);
        _mapMatrix[6, 3] = new HexData(6, 3, 0, 2, null, null, 1, 0);
        _mapMatrix[6, 4] = new HexData(6, 4, 0, 2);
        _mapMatrix[6, 5] = new HexData(6, 5, 0, 2);
        _mapMatrix[6, 6] = new HexData(6, 6, 0, 2);
        _mapMatrix[6, 7] = new HexData(6, 7, 0, 2, null, null, 1, 0);
        _mapMatrix[6, 8] = new HexData(6, 8, 4, 2);
        _mapMatrix[6, 9] = new HexData(6, 9, 5, 2);
        _mapMatrix[5, 2] = new HexData(5, 2, 0, 2);
        _mapMatrix[5, 3] = new HexData(5, 3, 0, 2, null, 7, 2, 0);
        _mapMatrix[5, 4] = new HexData(5, 4, 0, 2);
        _mapMatrix[5, 5] = new HexData(5, 5, 0, 3, null, 0);
        _mapMatrix[5, 6] = new HexData(5, 6, 0, 3, null, 0);
        _mapMatrix[5, 7] = new HexData(5, 7, 0, 3, null, 0);
        _mapMatrix[5, 8] = new HexData(5, 8, 5, 2);
        _mapMatrix[5, 9] = new HexData(5, 9, 5, 2);
        _mapMatrix[4, 2] = new HexData(4, 2, 0, 2);
        _mapMatrix[4, 3] = new HexData(4, 3, 0, 2);
        _mapMatrix[4, 4] = new HexData(4, 4, 0, 3, null, 0);
        _mapMatrix[4, 5] = new HexData(4, 5, 0, 3, null, 0);
        _mapMatrix[4, 6] = new HexData(4, 6, 0, 3, null, 0);
        _mapMatrix[4, 7] = new HexData(4, 7, 5, 2);
        _mapMatrix[4, 8] = new HexData(4, 8, 5, 2);
        _mapMatrix[3, 1] = new HexData(3, 1, 0, 2);
        _mapMatrix[3, 2] = new HexData(3, 2, 0, 2, null, null, 1, 0);
        _mapMatrix[3, 3] = new HexData(3, 3, 0, 3, null, 0);
        _mapMatrix[3, 4] = new HexData(3, 4, 0, 3, null, 0);
        _mapMatrix[3, 5] = new HexData(3, 5, 0, 3, null, 0);
        _mapMatrix[3, 6] = new HexData(3, 6, 0, 3, null, 0);
        _mapMatrix[3, 7] = new HexData(3, 7, 5, 2);
        _mapMatrix[3, 8] = new HexData(3, 8, 5, 2);
        _mapMatrix[2, 1] = new HexData(2, 1, 0, 3, null, 0);
        _mapMatrix[2, 2] = new HexData(2, 2, 0, 3, null, 0);
        _mapMatrix[2, 3] = new HexData(2, 3, 0, 3, null, 0);
        _mapMatrix[2, 4] = new HexData(2, 4, 2, 2, 2);
        _mapMatrix[2, 5] = new HexData(2, 5, 3, 2);
        _mapMatrix[2, 6] = new HexData(2, 6, 4, 2);
        _mapMatrix[2, 7] = new HexData(2, 7, 5, 2);
        _mapMatrix[1, 0] = new HexData(1, 0, 0, 3, null, 0);
        _mapMatrix[1, 1] = new HexData(1, 1, 0, 3, null, 0);
        _mapMatrix[1, 2] = new HexData(1, 2, 0, 3, null, 0);
        _mapMatrix[1, 3] = new HexData(1, 3, 1, 2, 2);
        _mapMatrix[1, 4] = new HexData(1, 4, 2, 2, 2);
        _mapMatrix[1, 5] = new HexData(1, 5, 3, 2);
        _mapMatrix[1, 6] = new HexData(1, 6, 4, 2);
        _mapMatrix[1, 7] = new HexData(1, 7, 5, 2);
        _mapMatrix[0, 0] = new HexData(0, 0, 0, 3, null, 0);
        _mapMatrix[0, 1] = new HexData(0, 1, 0, 3, null, 0);
        _mapMatrix[0, 2] = new HexData(0, 2, 1, 2, 2);
        _mapMatrix[0, 3] = new HexData(0, 3, 2, 2, 2);
        _mapMatrix[0, 4] = new HexData(0, 4, 3, 2, 2);
        _mapMatrix[0, 5] = new HexData(0, 5, 4, 2);
        _mapMatrix[0, 6] = new HexData(0, 6, 4, 2);

        //TODO: Mudar para consulta do banco e controlar se está pronto por variável
        _player1 = 1;
        _player2 = 0;

        RenderizeMap();

        SetCameraRotation(0f);
        LoadData(); //TODO: Apagar daqui e mudar para o do início
    }

    void Update()
    {
        #region FAKES

        if (_fake_opponentPositionTime.HasValue && _fake_opponentPositionTime.Value <= Time.time)
        {
            _fake_opponentPositionTime = null;
            FAKE_OpponentPositioning();
        }

        #endregion

        CameraControl();

        if (_cameraMobileTarget != null)
        {
            SetCameraPosition(_cameraMobileTarget.position);
        }
    }

    #region Getting game data

    private void LoadData()
    {
        //TODO: Mudar para um método que já retorne o jogador e os dados do personagem
        StartCoroutine(ApiDataService.Instance.GetCharactersCompleteByPlayer(1, CharactersLoaded));
        StartCoroutine(ApiDataService.Instance.GetCharactersCompleteByPlayer(0, CharactersLoaded));
    }

    private void CharactersLoaded(List<CharacterGameplay> characters)
    {
        _allHeroes.AddRange(characters);
        _numPlayersLoaded++;
        CheckForStart();
    }

    private void CheckForStart()
    {
        if (_numPlayersLoaded == 2)
        {
            NextTurn();
        }
    }

    #endregion

    private void LoadResources()
    {
        _heroPrefab = Resources.Load(AppConts.PrefabPath.MODULAR_HERO);
        _popupCharSelectionPrefab = Resources.Load(AppConts.PopupPath.CHAR_SELECTION);
        _confirmBoxPrefab = Resources.Load(AppConts.UIComponentsPath.CONFIRM);
        _toastPrefab = Resources.Load(AppConts.UIComponentsPath.TOAST);

        _hexMarkSetPositionPrefab = Resources.Load(AppConts.HexMarkPath.SET_POSITION);
        _hexDirectionPrefab = Resources.Load(AppConts.HexMarkPath.SET_DIRECTION);
        _hexMoveOptionPrefab = Resources.Load(AppConts.HexMarkPath.MOVE_OPTION);
        _hexPlayerTurnPrefab = Resources.Load(AppConts.HexMarkPath.PLAYER_TURN);
        _hexAllyPrefab = Resources.Load(AppConts.HexMarkPath.ALLY);
        _hexEnemyPrefab = Resources.Load(AppConts.HexMarkPath.ENEMY);
        _hexAttackOptionPrefab = Resources.Load(AppConts.HexMarkPath.ATTACK_OPTION);
    }

    private void InstantiatePopups()
    {
        var popupCharSelection = Instantiate(_popupCharSelectionPrefab, UICanvas) as GameObject;
        _popupCharSelection = popupCharSelection.GetComponent<PopupCharSelection>();
        _popupCharSelection.OnSelected += CharacterSelectedOnMenu;
        _popupCharSelection.OnUserClose += (s, e) => CharacterPopupMenuClosed();
    }

    private void ConfigureReferences()
    {
        ButtonPositionSet.OnClicked += (s, e) => ConfirmPositioningDone();
        ButtonAttack.OnClicked += (s, e) => AttackButtonClicked();
        ButtonMovement.OnClicked += (s, e) => MovementButtonClicked();
        ButtonUndoMovement.OnClicked += (s, e) => UndoMovementButtonClicked();
        ButtonEndTurn.OnClicked += (s, e) => EndTurnButtonClicked();
        ButtonCameraLock.OnClicked += (s, e) => LockCamera();
        ButtonCameraLock.SetEnabled(false);
    }

    private void RenderizeMap()
    {
        var validHexs = _mapMatrix.OfType<HexData>();

        foreach (var hex in validHexs)
        {
            //GameObject hexObj;
            Vector3 position = GetHexPosition(hex.CoordX, hex.CoordY, hex.Height);

            var hexPrefab = Resources.Load(string.Concat("Hexagons/Hex", hex.HexType.ToString("000")));
            var hexObj = Instantiate(hexPrefab, position, _defaultRotation, HexagonsArea) as GameObject;
            hexObj.name = string.Concat("Hex", hex.HexType.ToString("000"), "_", hex.CoordX, "_", hex.CoordY);
            var baseHexagon = hexObj.GetComponent<BaseHexagon>();
            baseHexagon.HexData = hex;
            baseHexagon.BattleSceneController = this;

            if (hex.Prop.HasValue)
            {
                var propPrefab = Resources.Load(string.Concat("Props/Env", hex.Prop.Value.ToString("000")));
                Instantiate(propPrefab, position, AppFunctions.GetRotation(hex.PropRotation ?? 0), HexagonsArea);
            }
        }
    }

    private void RenderizeHeroes()
    {
        foreach (var hero in _allHeroes)
        {
            //TODO: CRIAR LÓGICA PARA A POSIÇÃO ONDE SERÃO CRIADOS
            hero.PosX = 3;
            hero.PosY = 2;
            hero.FaceDirection = 0;

            var hex = _mapMatrix[hero.PosX, hero.PosY];
            hex.Character = hero;

            var obj = Instantiate(_heroPrefab, GetHexPosition(hex.CoordX, hex.CoordY, hex.Height), _defaultRotation) as GameObject;
            var ctrl = obj.GetComponent<HeroController>();
            ctrl.SetCharacterData(hero);
            _heroesControllers.Add(ctrl);
        }
    }

    public void HexagonClicked(HexData hex)
    {
        //TODO: Remover o problema de contar o click sobre o objeto quando arrasta a câmera
        //Debug.Log("HexagonClicked: " + hex.CoordX + "/" + hex.CoordY);
        
        if (_turnPhase == TurnPhase.Positioning && _playerTurn == _currentPlayerId)
        {
            if (_markMatrix[hex.CoordX, hex.CoordY] != null)
            {
                if (_turnSubPhase == TurnSubPhase.NotSet)
                    PositionSpotSelected(hex.CoordX, hex.CoordY);
                else if (_turnSubPhase == TurnSubPhase.Direction)
                    DirectionSpotSelected(hex.CoordX, hex.CoordY);
            }
        }
        if (_turnPhase == TurnPhase.Combat) // && _playerTurn == _currentPlayerId) //TODO: DESCOMENTAR
        {
            if (_markMatrix[hex.CoordX, hex.CoordY] != null)
            {
                if (_turnSubPhase == TurnSubPhase.Movement && _markMatrix[hex.CoordX, hex.CoordY].HexMarkType == HexMarkType.MoveOption)
                    MoveSpotSelected(hex.CoordX, hex.CoordY);

                if (_turnSubPhase == TurnSubPhase.Direction && _markMatrix[hex.CoordX, hex.CoordY].HexMarkType == HexMarkType.Direction)
                    DirectionSpotSelected(hex.CoordX, hex.CoordY);

                if (_turnSubPhase == TurnSubPhase.Attack && _markMatrix[hex.CoordX, hex.CoordY].HexMarkType == HexMarkType.Attack)
                    AttackSpotSelected(hex.CoordX, hex.CoordY);
            }
        }

    }

    #region TESTES - EXCLUIR

    private float? _fake_opponentPositionTime;
    private void FAKE_OpponentPositioning()
    {
        _auxTargetSpot = new IntVector2(0, 3);
        SummonHero(0, 3, 3);
        NextTurn();
    }

    #endregion
    
    #region PathFinder

    private List<PathRef> PathFinder_GetViableSpots(int steps, int jump, int startX, int startY, bool forAttack = false)
    {
        _pathFinder_isViableSearch = true;
        _pathFinder_isAttackRange = forAttack;
        PathFinder_Search(steps, jump, startX, startY);

        return _pathFinder_viableSpots;
    }

    private List<PathRef> PathFinder_GetPath(int steps, int jump, int startX, int startY, int endX, int endY)
    {
        _pathFinder_isViableSearch = false;
        _pathFinder_isAttackRange = false;
        PathFinder_Search(steps, jump, startX, startY, endX, endY);

        if (_pathFinder_goalPath != null)
        {
            var result = _pathFinder_goalPath.OrderBy(o => o.Steps).ToList();

            foreach (var path in result)
                path.Position = GetHexPosition(path.PosX, path.PosY, path.Height);
            return result;
        }
        else
        {
            Debug.LogError("Can't reach!");
            return null;
        }
    }

    private void PathFinder_Search(int steps, int jump, int startX, int startY, int endX = -1, int endY = -1)
    {
        _pathFinder_maxSteps = steps;
        _pathFinder_jump = jump;
        _pathFinder_goalX = endX;
        _pathFinder_goalY = endY;
        _pathFinder_goalPath = null;
        _pathFinder_bestFound = 999;
        _pathFinder_viableSpots = new List<PathRef>();
        _pathFinder_enemySurrounds = new List<IntVector2>();

        if (!_pathFinder_isAttackRange)
        {
            var enemies = _mapMatrix.OfType<HexData>().Where(w => w.Character != null && w.Character.BaseInfo.PlayerId != _playerTurn);
            if (enemies != null)
            {
                foreach (var e in enemies)
                {
                    foreach (var dir in _directionMatrix)
                    {
                        var posX = e.CoordX + dir.x;
                        var posY = e.CoordY + dir.y;

                        if (posX >= 0 && posX < MAX_MAP_MATRIX_X && posY >= 0 && posY < MAX_MAP_MATRIX_Y)
                        {
                            var hex = _mapMatrix[posX, posY];

                            if (hex != null && Mathf.Abs(e.Height - hex.Height) <= 2 && !_pathFinder_enemySurrounds.Any(a => a.x == posX && a.y == posY))
                            {
                                _pathFinder_enemySurrounds.Add(new IntVector2(posX, posY));
                            }
                        }
                    }
                }
            }
        }

        PathFinder_CheckSpots(startX, startY, new List<PathRef>());
    }

    private void PathFinder_CheckSpots(int posX, int posY, List<PathRef> path, int currentStep = -1)
    {
        //Checking bounds
        if (posX >= 0 && posX < MAX_MAP_MATRIX_X && posY >= 0 && posY < MAX_MAP_MATRIX_Y)
        {
            //Checking for repeated hexagons
            if (path.Any(a => a.PosX == posX && a.PosY == posY))
                return;

            var hex = _mapMatrix[posX, posY];

            //Checking for valid hexagon
            if (hex != null)
            {
                if (hex.Character != null && !_pathFinder_isAttackRange
                    && !(hex.Character.BaseInfo.Id == _characterTurn && hex.Character.BaseInfo.PlayerId == _playerTurn))
                    return;

                //TODO: Verificar a forma de movimento (andar, flutuar, voar ou teleportar) e comparar com o nível de bloqueio
                if (hex.BlockLevel.HasValue && !_pathFinder_isAttackRange)
                    return;

                currentStep++;
                var lastPath = path.LastOrDefault();
                if (lastPath != null)
                {
                    var hDiff = hex.Height - lastPath.Height;

                    //TODO: Verificar porque consegue atacar de uma altura por cima e não da mesma altura por baixo
                    if (hDiff > 0)
                    {
                        var jumpFactor = Mathf.Clamp(hDiff - _pathFinder_jump, 0, 2);
                        if (jumpFactor == 1) //Climbing 1 difference consumes one more step
                            currentStep++;
                        else if (jumpFactor == 2) //Can't climb on 2 of difference
                            return;
                    }
                    else if (hDiff < 0) //TODO: Estava 1, mudei pra 0, confirmar funcionamento correto
                    {
                        var jumpFactor = Mathf.Clamp((hDiff * -1) - _pathFinder_jump, 0, 2);
                        if (jumpFactor == 2) //Can't fall on 2 of difference
                            return;
                    }
                }

                //Checking for limit steps
                if (currentStep > _pathFinder_maxSteps)
                    return;

                //Cheking if a better parth has found
                if (currentStep >= _pathFinder_bestFound)
                    return;

                if (_pathFinder_isViableSearch && !_pathFinder_viableSpots.Any(a => a.PosX == posX && a.PosY == posY))
                    _pathFinder_viableSpots.Add(new PathRef(posX, posY, hex.Height));

                //Cheking goal
                if (posX == _pathFinder_goalX && posY == _pathFinder_goalY)
                {
                    //Can't stay on a blocked hex
                    if (hex.BlockLevel.HasValue)
                        return;

                    var newPath = PathFinder_CloneAndAdd(path, posX, posY, currentStep, hex.Height);
                    _pathFinder_goalPath = newPath;
                    _pathFinder_bestFound = currentStep;
                    return;
                }

                //Checking if it's a final path
                if (currentStep == _pathFinder_maxSteps)
                    return;

                //Checkinf if it's a adjacent enemy spot, with less than 2 height difference (can't move anymore)
                if (currentStep > 0 && _pathFinder_enemySurrounds.Any(a => a.x == posX && a.y == posY))
                    return;

                //Search on all other directions
                foreach (var dir in _directionMatrix)
                {
                    var newPath = PathFinder_CloneAndAdd(path, posX, posY, currentStep, hex.Height);
                    PathFinder_CheckSpots(posX + dir.x, posY + dir.y, newPath, currentStep);
                }
            }
        }
    }

    private List<PathRef> PathFinder_CloneAndAdd(List<PathRef> origin, int posX, int posY, int step, int height)
    {
        //Cloning to remove reference
        var newPath = new List<PathRef>();
        newPath.AddRange(origin);
        newPath.Add(new PathRef(posX, posY, step, height));
        return newPath;
    }

    private void PathFinder_SetCharacterValues(CharacterGameplay character)
    {
        _pathFinder_maxSteps = character.Movement;
        _pathFinder_jump = character.Jump;
    }

    #endregion

    #region Turn Management

    private void NextTurn()
    {
        if (_turnPhase == TurnPhase.Positioning)
        {
            if (_player1_positioningDone && _player2_positioningDone)
            {
                _turnPhase = TurnPhase.Combat;
                if (_staticToast != null)
                    _staticToast.ReleaseText();

                _turnGauge = _gameHeroes.Select(s => new TurnGauge()
                {
                    CharacterId = s.BaseInfo.Id,
                    PlayerId = s.BaseInfo.PlayerId,
                    Movement = s.Movement,
                    Speed = s.Speed,
                    Gauge = 0
                }).ToList();

                _characterTurn = null;
                _playerTurn = null;
                //ShowToast(AppLanguage.GetText(8)); //Combat started
            }
            else
            {
                //TODO: REMOVER ESSA LÓGICA, ISSO TEM QUE ACONTECER DE FORMA SIMULTÂNEA
                if (!_player1_positioningDone)
                {
                    _playerTurn = _player1;
                    _player1_positioningDone = true;
                }
                else if (!_player2_positioningDone)
                {
                    _playerTurn = _player2;
                    _player2_positioningDone = true;
                }

                if (_playerTurn == _currentPlayerId)
                {
                    ShowPositionSpots(true);
                    ShowToast(AppLanguage.GetText(0)); //Choose your champions
                    ButtonPositionSet.gameObject.SetActive(true);
                    ButtonPositionSet.SetEnabled(false);
                }
                else
                {
                    //Mudar a for de exibir a mensagem de aguardando (talvez com um load)
                    var go = Instantiate(_toastPrefab, UICanvas) as GameObject;
                    _staticToast = go.GetComponent<UIToast>();
                    _staticToast.SetText(AppLanguage.GetText(7), true); //Waiting your opponent
                    ClearAllMarks();
                    _fake_opponentPositionTime = Time.time + 1f;
                    //TODO: Lógica de aguardando a vez do outro jogador
                }
            }
        }

        if (_turnPhase == TurnPhase.Combat)
        {
            RecalculateGauge();
            _characterTurn = _turnOrder[0].CharacterId;
            _playerTurn = _turnOrder[0].PlayerId;
            _turnSubPhase = TurnSubPhase.Movement;
            _movementDone = false;
            _attackDone = false;
            ShowMoveSpots();

            TurnButtonsGroup.SetActive(true);
            ButtonCameraLock.gameObject.SetActive(true);
            //TurnButtonsGroup.SetActive(_playerTurn == _currentPlayerId); //TODO: DESCOMENTAR

            //TODO: Se o turno for do jogador atual, exibir a barra no canto esquerdo, se for do oponente, exibir no direito

            //TODO: Verificar neste momento se o movimento está bloqueado e/ou ataque e ajustar a variáveis
        }
    }

    private void RecalculateGauge()
    {
        //Consuming the last played character gauge
        if (_characterTurn.HasValue && _turnOrder[0].CharacterId == _characterTurn.Value && _turnOrder[0].PlayerId == _playerTurn.Value)
        {
            var last = _turnGauge.First(f => f.CharacterId == _characterTurn.Value && f.PlayerId == _playerTurn.Value);
            last.Gauge -= 100f;
        }

        //Update the gauge list
        _turnGauge = UpdateGaugeList(_turnGauge);

        //Get turn order to show
        var turnGaugeToOrder = _turnGauge.Select(s => s.Clone()).ToList(); //Removing reference
        for (var i = 0; i < 8; i++)
        {
            var first = turnGaugeToOrder.OrderByDescending(o => o.Gauge)
                                        .ThenByDescending(t => t.Speed)
                                        .ThenByDescending(t => t.Movement)
                                        .ThenByDescending(t => t.CharacterId)
                                        .First();
            first.Gauge -= 100f;
            _turnOrder[i] = new CharacterKey(first.CharacterId, first.PlayerId);

            turnGaugeToOrder = UpdateGaugeList(turnGaugeToOrder);
        }
    }

    private List<TurnGauge> UpdateGaugeList(List<TurnGauge> gaugeList)
    {
        var newGaugeList = gaugeList;

        if (!newGaugeList.Any(a => a.Gauge >= 100f))
        {
            foreach (var gauge in newGaugeList)
                gauge.Gauge += gauge.Speed;
        }

        if (newGaugeList.Any(a => a.Gauge >= 100f))
            return newGaugeList;
        else
            return UpdateGaugeList(newGaugeList);
    }

    private void ClearAllMarks()
    {
        var allMarks = _markMatrix.OfType<HexMark>().ToList();

        foreach (var mark in allMarks)
        {
            _markMatrix[mark.CoordX, mark.CoordY] = null;
            Destroy(mark.gameObject);
        }

        if (SideMarksArea.childCount > 0)
        {
            for (var i = 0; i < SideMarksArea.childCount; i++)
            {
                Destroy(SideMarksArea.GetChild(i).gameObject);
            }
        }
    }

    #endregion

    #region Character Positioning

    private void ShowPositionSpots(bool moveCamera = false)
    {
        ClearAllMarks();

        var availableHexs = _mapMatrix.OfType<HexData>().Where(w => w.StartTeam.HasValue && w.StartTeam.Value == _playerTurn);
        if (availableHexs != null && availableHexs.Any())
        {
            if (moveCamera)
            {
                var first = availableHexs.FirstOrDefault();
                if (first != null)
                {
                    SetCameraPosition(GetHexPosition(first.CoordX, first.CoordY, first.Height));
                }
            }

            foreach (var hex in availableHexs)
            {
                var go = Instantiate(_hexMarkSetPositionPrefab, GetHexPosition(hex.CoordX, hex.CoordY, hex.Height), _defaultRotation, MarksArea) as GameObject;
                var hm = go.GetComponent<HexMark>();
                hm.CoordX = hex.CoordX;
                hm.CoordY = hex.CoordY;
                _markMatrix[hex.CoordX, hex.CoordY] = hm;
            }
        }

        ButtonPositionSet.SetEnabled(_gameHeroes.Any(a => a.BaseInfo.PlayerId == _playerTurn));
    }

    private void PositionSpotSelected(int posX, int posY)
    {
        var hex = _mapMatrix[posX, posY];

        if (hex.Character == null)
        {
            var available = _allHeroes.Where(w => w.BaseInfo.PlayerId == _currentPlayerId && !_gameHeroes.Any(a => a.BaseInfo.Id == w.BaseInfo.Id)).ToList();

            if (available.Count > 0)
            {
                SetCameraPosition(GetHexPosition(hex.CoordX, hex.CoordY, hex.Height));
                _markMatrix[posX, posY].SetSelected(true);
                _popupCharSelection.SetCharacterData(available);
                _popupCharSelection.SetActive(true);
                _isPopupActive = true;
                _auxTargetSpot = new IntVector2(posX, posY);
            }
            else
            {
                //TODO: Play a soft denied sound
                ShowToast(AppLanguage.GetText(1)); //All available heroes have already been positioned
            }
        }
        else
        {
            SetCameraPosition(GetHexPosition(hex.CoordX, hex.CoordY, hex.Height));
            _markMatrix[posX, posY].SetSelected(true);

            _confirmBox = GetNewConfirmBox();
            _confirmBox.SetData(AppLanguage.GetText(2), AppLanguage.GetText(3, hex.Character.BaseInfo.Name)); //Remove Champion? | Do you want to\nremove {0}?
            _confirmBox.OnCofirm += (s, e) => RemoveHero(posX, posY);
            _confirmBox.OnCancel += (s, e) => CloseHeroRemoveBox(posX, posY);
        }
    }

    private void CharacterSelectedOnMenu(object sender, System.EventArgs e)
    {
        var charId = (long)sender;
        ButtonPositionSet.SetEnabled(false);
        SummonHero(_playerTurn.Value, charId);
    }

    private void CharacterPopupMenuClosed()
    {
        _markMatrix[_auxTargetSpot.Value.x, _auxTargetSpot.Value.y].SetSelected(false);
        _isPopupActive = false;
    }

    private void SummonHero(long playerId, long heroId, int defaultRotation = 0)
    {
        var hero = _allHeroes.First(f => f.BaseInfo.PlayerId == playerId && f.BaseInfo.Id == heroId);
        _gameHeroes.Add(hero);

        hero.PosX = _auxTargetSpot.Value.x;
        hero.PosY = _auxTargetSpot.Value.y;
        hero.FaceDirection = defaultRotation;

        var hex = _mapMatrix[hero.PosX, hero.PosY];
        hex.Character = hero;

        var obj = Instantiate(_heroPrefab, GetHexPosition(hex.CoordX, hex.CoordY, hex.Height), _defaultRotation) as GameObject;
        obj.name = string.Format("Champion_{0}_{1}", heroId, playerId);
        var ctrl = obj.GetComponent<HeroController>();
        ctrl.SetCharacterData(hero);
        ctrl.MovementDone += (s, e) => MovementDone();
        ctrl.AttackDone += (s, e) => AttackDone();
        _heroesControllers.Add(ctrl);

        if (defaultRotation > 0)
            ctrl.FaceDirection(defaultRotation);

        if (_turnPhase == TurnPhase.Positioning && _playerTurn == _currentPlayerId)
        {
            _markMatrix[_auxTargetSpot.Value.x, _auxTargetSpot.Value.y].SetSelected(false);
            _popupCharSelection.SetActive(false);
            _isPopupActive = false;

            _turnSubPhase = TurnSubPhase.Direction;
            _characterTurn = hero.BaseInfo.Id;
            ShowDirectionSpots();
        }
    }

    private void RemoveHero(int posX, int posY)
    {
        var hex = _mapMatrix[posX, posY];
        var name = hex.Character.BaseInfo.Name;
        var heroId = hex.Character.BaseInfo.Id;
        _markMatrix[posX, posY].SetSelected(false);

        var heroCtrl = _heroesControllers.First(f => f.Character.BaseInfo.Id == heroId);
        _heroesControllers.Remove(heroCtrl);
        _gameHeroes.RemoveAll(r => r.BaseInfo.Id == heroId);
        Destroy(heroCtrl.gameObject);
        hex.Character = null;

        CloseHeroRemoveBox(posX, posY);
        ShowToast(AppLanguage.GetText(4, name)); //<color=#FFFE84>{0}</color> has been removed

        ButtonPositionSet.SetEnabled(_gameHeroes.Any(a => a.BaseInfo.PlayerId == _playerTurn));
    }

    private void CloseHeroRemoveBox(int posX, int posY)
    {
        _markMatrix[posX, posY].SetSelected(false);
        CloseConfirmBox();
    }

    private void ConfirmPositioningDone()
    {
        _confirmBox = GetNewConfirmBox();
        _confirmBox.SetData(AppLanguage.GetText(5), AppLanguage.GetText(6)); //Are you ready\nto start?
        _confirmBox.OnCofirm += (s, e) => EndPlayerPositioning();
        _confirmBox.OnCancel += (s, e) => CloseConfirmBox();
    }

    private void EndPlayerPositioning()
    {
        CloseConfirmBox();
        NextTurn();
        ButtonPositionSet.gameObject.SetActive(false);
    }

    #endregion

    #region Combat Phase

    private void UpdateSideSpots(bool showEnemies = true)
    {
        foreach (var character in _gameHeroes)
        {
            var hex = _mapMatrix[character.PosX, character.PosY];

            Object prefab = null;

            if (character.BaseInfo.PlayerId == _playerTurn)
            {
                if (character.BaseInfo.Id == _characterTurn)
                    prefab = _hexPlayerTurnPrefab;
                else
                    prefab = _hexAllyPrefab;
            }
            else
            {
                if (!showEnemies)
                    continue;

                prefab = _hexEnemyPrefab;
            }

            var go = Instantiate(prefab, GetHexPosition(hex.CoordX, hex.CoordY, hex.Height), _defaultRotation, SideMarksArea) as GameObject;
            var hm = go.GetComponent<HexMark>();
            hm.CoordX = hex.CoordX;
            hm.CoordY = hex.CoordY;
        }
    }

    private void EndTurnButtonClicked()
    {
        _attackDone = true;
        _movementDone = true;
        _turnSubPhase = TurnSubPhase.Direction;
        ShowDirectionSpots();
        SetUndoMovementOption(false);
    }

    #region Movement

    private void MovementButtonClicked()
    {
        if (!_movementDone)
        {
            _turnSubPhase = TurnSubPhase.Movement;
            ShowMoveSpots();
        }
    }

    private void ShowMoveSpots()
    {
        UpdateTurnButtonsState();
        ClearAllMarks();
        _auxTargetSpot = null;

        var character = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);
        PathFinder_SetCharacterValues(character);

        var availablePath = PathFinder_GetViableSpots(character.Movement, character.Jump, character.PosX, character.PosY);

        var hex = _mapMatrix[character.PosX, character.PosY];
        SetCameraPositionAndLock(GetHexPosition(hex.CoordX, hex.CoordY, hex.Height));

        if (availablePath != null && availablePath.Any())
        {
            foreach (var path in availablePath)
            {
                if (path.PosX == character.PosX && path.PosY == character.PosY)
                    continue;

                var go = Instantiate(_hexMoveOptionPrefab, GetHexPosition(path.PosX, path.PosY, path.Height), _defaultRotation, MarksArea) as GameObject;
                var hm = go.GetComponent<HexMark>();
                hm.CoordX = path.PosX;
                hm.CoordY = path.PosY;
                _markMatrix[path.PosX, path.PosY] = hm;
            }
        }
        else
        {
            //TODO: Finalizar movimento e exibir mensagem dizendo que não há opções de movimentação
        }

        UpdateSideSpots();
        _attackPreview.Hide();
    }

    private void MoveSpotSelected(int posX, int posY)
    {
        if (_auxTargetSpot == null)
        {
            _auxTargetSpot = new IntVector2(posX, posY);
            _markMatrix[posX, posY].SetSelected(true);
        }
        else
        {
            if (_auxTargetSpot.Value.x == posX && _auxTargetSpot.Value.y == posY)
            {
                ClearAllMarks();

                var character = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);
                var charCtrl = _heroesControllers.First(f => f.Character.BaseInfo.PlayerId == _playerTurn && f.Character.BaseInfo.Id == _characterTurn);

                _originMoveHex = new IntVector2(character.PosX, character.PosY);

                var path = PathFinder_GetPath(character.Movement, character.Jump, character.PosX, character.PosY, _auxTargetSpot.Value.x, _auxTargetSpot.Value.y);
                charCtrl.MoveOnPath(path);
                _cameraMobileTarget = charCtrl.transform;
                _movementDone = true;
                UpdateTurnButtonsState();
            }
            else
            {
                _markMatrix[_auxTargetSpot.Value.x, _auxTargetSpot.Value.y].SetSelected(false);
                _markMatrix[posX, posY].SetSelected(true);

                _auxTargetSpot = new IntVector2(posX, posY);
            }
        }
    }

    private void MovementDone()
    {
        var character = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);
        var charCtrl = _heroesControllers.First(f => f.Character.BaseInfo.PlayerId == _playerTurn && f.Character.BaseInfo.Id == _characterTurn);

        _mapMatrix[_originMoveHex.x, _originMoveHex.y].Character = null; //Removing character reference from the previous hex
        _mapMatrix[character.PosX, character.PosY].Character = character; //Adding character reference to the new hex

        _movementDone = true;
        _cameraMobileTarget = null;

        if (!_attackDone)
        {
            _turnSubPhase = TurnSubPhase.Attack;
            ShowAttackSpots();
        }
        else
        {
            _turnSubPhase = TurnSubPhase.Direction;
            ShowDirectionSpots();
        }

        SetUndoMovementOption(true);
    }

    private void UndoMovementButtonClicked()
    {
        var character = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);
        var charCtrl = _heroesControllers.First(f => f.Character.BaseInfo.PlayerId == _playerTurn && f.Character.BaseInfo.Id == _characterTurn);

        _mapMatrix[character.PosX, character.PosY].Character = null; //Removing character reference from the current hex
        character.PosX = _originMoveHex.x;
        character.PosY = _originMoveHex.y;
        _mapMatrix[_originMoveHex.x, _originMoveHex.y].Character = character; //Adding character reference to the old hex
        var hexPos = GetHexPosition(_originMoveHex.x, _originMoveHex.y, _mapMatrix[_originMoveHex.x, _originMoveHex.y].Height);
        charCtrl.transform.position = hexPos;
        charCtrl.FixPosition(hexPos);

        _movementDone = false;

        _turnSubPhase = TurnSubPhase.Movement;
        ShowMoveSpots();
        SetUndoMovementOption(false);

        SetCameraPositionAndLock(hexPos);
    }

    private void SetUndoMovementOption(bool enable)
    {
        ButtonUndoMovement.gameObject.SetActive(enable);
        ButtonMovement.gameObject.SetActive(!enable);
    }

    #endregion

    #region Attack

    private void AttackButtonClicked()
    {
        if (!_attackDone)
        {
            _turnSubPhase = TurnSubPhase.Attack;
            ShowAttackSpots();
        }
    }

    //For base weapon attack
    private void ShowAttackSpots()
    {
        UpdateTurnButtonsState();
        ClearAllMarks();
        _auxTargetSpot = null;

        var character = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);

        var hex = _mapMatrix[character.PosX, character.PosY];
        SetCameraPosition(GetHexPosition(hex.CoordX, hex.CoordY, hex.Height));

        var rangePath = PathFinder_GetViableSpots(character.BaseAttackRange, 2, character.PosX, character.PosY, true);
        if (rangePath != null && rangePath.Any())
        {
            foreach (var path in rangePath)
            {
                if (path.PosX == character.PosX && path.PosY == character.PosY)
                    continue;

                var go = Instantiate(_hexAttackOptionPrefab, GetHexPosition(path.PosX, path.PosY, path.Height), _defaultRotation, MarksArea) as GameObject;
                var hm = go.GetComponent<HexMark>();
                hm.CoordX = path.PosX;
                hm.CoordY = path.PosY;
                _markMatrix[path.PosX, path.PosY] = hm;
            }
        }
        else
        {
            //TODO: Exibir mensagem dizendo que não há opções de ataque básico
        }

        UpdateSideSpots(false);
        _attackPreview.Hide();
    }

    //For base weapon attack
    private void AttackSpotSelected(int posX, int posY)
    {
        //TODO: Quando existir diferença de altura, calcular o raycast a partir da área do peito e identificar se há objetos no caminho para identificar chance de acerto
        //(posicionar dois colliders já carregados específicos para isso, fazer os cálculos e removelos da posição)
        var newHexSelected = false;

        if (_auxTargetSpot == null)
        {
            newHexSelected = true;
        }
        else
        {
            if (_auxTargetSpot.Value.x == posX && _auxTargetSpot.Value.y == posY)
            {
                ClearAllMarks();

                var turnCharacter = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);
                var turnCharCtrl = _heroesControllers.First(f => f.Character.BaseInfo.PlayerId == _playerTurn && f.Character.BaseInfo.Id == _characterTurn);
                var enemyCharacter = _gameHeroes.First(f => f.PosX == posX && f.PosY == posY);
                var enemyCharCtrl = _heroesControllers.First(f => f.Character.PosX == posX && f.Character.PosY == posY);

                turnCharCtrl.AttackOnTarget(GetHexPosition(posX, posY) - GetHexPosition(turnCharacter.PosX, turnCharacter.PosY));

                if (Random.Range(0f, 100f) <= _attackAccuracy)
                {
                    enemyCharCtrl.TakeDamage(100, 0.3f, _attackFromBehind);
                }
                else
                {
                    enemyCharCtrl.Evade(0.3f);
                }

                _attackDone = true;
                UpdateTurnButtonsState();
                _attackPreview.Hide();

                if (_movementDone)
                    SetUndoMovementOption(false);
            }
            else
            {
                _markMatrix[_auxTargetSpot.Value.x, _auxTargetSpot.Value.y].SetSelected(false);
                newHexSelected = true;
            }
        }

        if (newHexSelected)
        {
            _auxTargetSpot = null;
            var hex = _mapMatrix[posX, posY];

            if (hex.Character != null && hex.Character.BaseInfo.PlayerId != _playerTurn)
            {
                _auxTargetSpot = new IntVector2(posX, posY);
                _markMatrix[posX, posY].SetSelected(true);

                var turnCharacter = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);
                var enemyCharacter = _gameHeroes.First(f => f.PosX == posX && f.PosY == posY);

                _attackFromBehind = IsBackAttack(turnCharacter, enemyCharacter);
                _attackDamage = 2456; //TODO: Implementar via GameMath
                _attackAccuracy = GameMath.GetBaseAttackAccuracy(turnCharacter, enemyCharacter, _attackFromBehind);
                //TODO: se tiver duas armas, chamar o accuracy de novo (passando parametro de offhand) e...
                //      chamar um método novo do _attackPreview.ShowValues que exibe os dois valores no formato: 80%/40%
                //TODO: Criar uma classe que faça o cálculo de uma vez de accuracy e dano e devolva os dados em uma entidade

                SetCameraPositionAndLock(GetHexPosition(hex.CoordX, hex.CoordY, hex.Height));
                _attackPreview.ShowValues(_attackAccuracy, _attackDamage, _attackFromBehind ? AttackType.FromBehind : AttackType.Normal);
                //TODO: Exibir barra do inimigo no canto superior direito
            }
            else
            {
                //TODO: play soft denied sound and/or show a toast
                _attackPreview.Hide();
            }
        }
    }

    //Rule 1: Back attack count only for base attack
    private bool IsBackAttack(CharacterGameplay attacker, CharacterGameplay target)
    {
        var targetLine = IsTargetInLine(attacker.PosX, attacker.PosY, target.PosX, target.PosY, attacker.BaseAttackRange);
        //Rule 2: Back attack count only if the target is in a line
        if (targetLine == null)
            return false;

        //Rule 3: Back attack count only if the attackar and target are facing the same direction
        if (AppFunctions.GetDirectionNum(targetLine.Value.x, targetLine.Value.y) != target.FaceDirection)
            return false;

        //Rule 4: Ranged attack don't have Back attack (unless it's a spear)
        //TODO: Implementar (colocar por primeiro)

        Debug.Log("IS BACK ATTACK!");
        return true;
    }

    private void AttackDone()
    {
        //var character = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);
        //var charCtrl = _heroesControllers.First(f => f.Character.BaseInfo.PlayerId == _playerTurn && f.Character.BaseInfo.Id == _characterTurn);

        //_mapMatrix[_originMoveHex.x, _originMoveHex.y].Character = null; //Removing character reference from the previous hex
        //_mapMatrix[character.PosX, character.PosY].Character = character; //Adding character reference to the new hex

        _attackDone = true;
        if (!_movementDone)
        {
            _turnSubPhase = TurnSubPhase.Movement;
            ShowMoveSpots();
        }
        else
        {
            _turnSubPhase = TurnSubPhase.Direction;
            ShowDirectionSpots();
        }
    }

    #endregion

    #region Direction

    private void ShowDirectionSpots()
    {
        UpdateTurnButtonsState();
        ClearAllMarks();

        if (_characterTurn.HasValue)
        {
            var character = _gameHeroes.First(f => f.BaseInfo.PlayerId == _playerTurn && f.BaseInfo.Id == _characterTurn);

            var curHex = _mapMatrix[character.PosX, character.PosY];
            SetCameraPosition(GetHexPosition(curHex.CoordX, curHex.CoordY, curHex.Height));

            foreach (var dir in _directionMatrix)
            {
                var posX = character.PosX + dir.x;
                var posY = character.PosY + dir.y;

                if (posX >= 0 && posX < MAX_MAP_MATRIX_X && posY >= 0 && posY < MAX_MAP_MATRIX_Y)
                {
                    var hex = _mapMatrix[posX, posY];

                    if (hex != null)
                    {
                        var go = Instantiate(_hexDirectionPrefab, GetHexPosition(hex.CoordX, hex.CoordY, hex.Height), _defaultRotation, MarksArea) as GameObject;
                        var hm = go.GetComponent<HexMark>();
                        hm.CoordX = hex.CoordX;
                        hm.CoordY = hex.CoordY;
                        _markMatrix[hex.CoordX, hex.CoordY] = hm;
                    }
                }
            }
        }

        UpdateSideSpots();
        _attackPreview.Hide();
    }

    private void DirectionSpotSelected(int posX, int posY)
    {
        var hero = _gameHeroes.FirstOrDefault(f => f.BaseInfo.Id == _characterTurn && f.BaseInfo.PlayerId == _playerTurn);
        var ctrl = _heroesControllers.FirstOrDefault(f => f.Character.BaseInfo.Id == _characterTurn && f.Character.BaseInfo.PlayerId == _playerTurn);
        ctrl.FaceDirection(posX, posY, true);

        if (_turnPhase == TurnPhase.Positioning)
        {
            _turnSubPhase = TurnSubPhase.NotSet;
            ShowPositionSpots();
        }
        else if (_turnPhase == TurnPhase.Combat)
        {
            ClearAllMarks();
            NextTurn();
            SetUndoMovementOption(false);
        }
    }

    #endregion

    #endregion

    #region Utils

    private Vector3 GetHexPosition(float coordX, float coordY, float? height = null)
    {
        var newCoordX = coordX - _mapMatrixHalfX;
        var newCoordY = coordY - _mapMatrixHalfY;
        var posX = STEP_HEX_POS_X * newCoordX * -1f;
        var posY = height.HasValue ? ELEVATION_MDF * height.Value : 0f;
        var posZ = (STEP_HEX_POS_Y * newCoordY) - (newCoordX * STEP_HEX_POS_Y / 2f);

        return new Vector3(posX, posY, posZ);
    }

    private void ShowToast(string text)
    {
        if (Time.time > _nextToastAllowed)
        {
            _nextToastAllowed = Time.time + 0.25f;
            var go = Instantiate(_toastPrefab, UICanvas) as GameObject;
            var toast = go.GetComponent<UIToast>();
            toast.SetText(text);
        }
    }

    private UIConfirm GetNewConfirmBox()
    {
        var go = Instantiate(_confirmBoxPrefab, UICanvas) as GameObject;
        return go.GetComponent<UIConfirm>();
    }

    private void CloseConfirmBox()
    {
        _confirmBox.SelfDestroy();
        _confirmBox = null;
    }

    private void UpdateTurnButtonsState()
    {
        ButtonAttack.SetEnabled(!_attackDone && _turnSubPhase != TurnSubPhase.Attack);
        ButtonMovement.SetEnabled(!_movementDone && _turnSubPhase != TurnSubPhase.Movement);
        ButtonEndTurn.SetEnabled(!_attackDone || !_movementDone);
        ButtonItem.SetEnabled(false); //TODO: REVALIDAR QUANDO HOUVER LÓGICA
        ButtonSkill.SetEnabled(false); //TODO: REVALIDAR QUANDO HOUVER LÓGICA
    }

    private IntVector2? IsTargetInLine(int originX, int originY, int targetX, int targetY, int range)
    {
        foreach (var dir in _directionMatrix)
        {
            var posX = originX;
            var posY = originY;

            for (var step = 1; step <= range; step++)
            {
                posX += dir.x;
                posY += dir.y;

                if (posX == targetX && posY == targetY)
                    return dir;
            }
        }

        return null;
    }

    #endregion

    #region Camera

    private void CameraControl()
    {
        #region Get Positions
        //For Mobile
        //if (Input.touches.Count() > 0 && Input.touches[0].fingerId == 0)
        //{
        //    if (_dragStartPosition == null)
        //        _dragStartPosition = Input.touches[0].position;
        //    _dragCurrentPosition = Input.touches[0].position;
        //}
        //else
        //{
        //    _dragStartPosition = null;
        //    _dragCurrentPosition = null;
        //    _dragOffSet = null;
        //}

        //For PC
        if (Input.GetMouseButton(0) && !_isPopupActive)
        {
            if (_dragStartPosition == null)
                _dragStartPosition = Input.mousePosition;
            _dragCurrentPosition = Input.mousePosition;
        }
        else if (_dragStartPosition.HasValue)
        {
            _dragStartPosition = null;
            _dragCurrentPosition = null;
            _dragMobileAnchor = null;
        }
        #endregion

        if (_dragStartPosition.HasValue && _dragCurrentPosition.HasValue)
        {
            Vector3? diff = null;

            if (_dragMobileAnchor.HasValue)
            {
                diff = (_dragMobileAnchor.Value - _dragCurrentPosition.Value).normalized;
                _dragMobileAnchor = _dragCurrentPosition;
            }
            else if (Vector3.Distance(_dragStartPosition.Value, _dragCurrentPosition.Value) >= 20f)
            {
                diff = (_dragStartPosition.Value - _dragCurrentPosition.Value).normalized;
                _dragMobileAnchor = _dragCurrentPosition;
            }

            if (diff.HasValue)
            {
                diff = new Vector3(diff.Value.x, 0f, diff.Value.y) * 30f * Time.deltaTime;
                TranslateCameraBy(diff.Value);
            }
        }

        if (_cameraTranslateTarget.HasValue)
        {
            SetFreeCameraPosition(Vector3.Lerp(_cameraTarget, _cameraTranslateTarget.Value, 10f * Time.deltaTime));

            if (Vector3.Distance(_cameraTarget, _cameraTranslateTarget.Value) < 0.001f)
                _cameraTranslateTarget = null;
        }
    }

    private void TranslateCameraBy(Vector3 step)
    {
        if (!_cameraTranslateTarget.HasValue)
            _cameraTranslateTarget = _cameraTarget;
        _cameraTranslateTarget += step;

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(_viewportCenter);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Hexagon")
            {
                _cameraTranslateTarget = new Vector3(_cameraTranslateTarget.Value.x,
                                                     hit.transform.position.y,
                                                     _cameraTranslateTarget.Value.z);
            }
        }
    }

    private void SetCameraPositionAndLock(Vector3 target)
    {
        _cameraIsLocked = true;
        ButtonCameraLock.SetEnabled(false);
        SetCameraPosition(target);
    }

    private void SetFreeCameraPosition(Vector3 target)
    {
        if (_cameraIsLocked)
        {
            _cameraIsLocked = false;
            ButtonCameraLock.SetEnabled(true);
            _attackPreview.Hide();
        }

        _cameraTarget = target;
        SetCameraPosition();
    }

    private void SetCameraPosition(Vector3 target)
    {
        _lockedCameraTarget = target;

        if (!_cameraIsLocked)
            return;

        _cameraTarget = target;
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        MainCamera.transform.position = _cameraTarget + (_cameraRotationAngle * -9f) + _cameraYOffset;
    }

    private void SetCameraRotation(float angle)
    {
        _cameraAngle = angle;
        var newRotation = Quaternion.AngleAxis(_cameraAngle, Vector3.up);
        _cameraRotationAngle = newRotation * Vector3.forward;

        MainCamera.transform.rotation = newRotation;
        MainCamera.transform.Rotate(Vector3.right, 60f);

        SetCameraPosition();
    }

    private void LockCamera()
    {
        _cameraIsLocked = true;
        ButtonCameraLock.SetEnabled(false);
        _cameraTarget = _lockedCameraTarget;
        _cameraTranslateTarget = null;
        SetCameraPosition();
    }

    #endregion
}
