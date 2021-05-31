using Assets.Scripts.Entities;
using Assets.Scripts.Entities.LocalData;
using Assets.Scripts.Entities.Utility;
using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [Header("Controllers")]
    public Animator Animator;

    [Header("Visual")]
    public Transform BodySkin;
    public Transform BackAttachment;

    [Header("Weapons")]
    public Transform MainHandSlot;

    public CharacterGameplay Character { get; private set; }

    #region Animation Hash

    #region Triggers
    private int _animationAttack; //DoAttack
    private int _animationSpecialAttack; //DoSpecialAttack
    private int _animationFrontHit; //TakeFrontHit
    private int _animationBackHit; //TakeBackHit
    private int _animationBlock; //DoBlock
    private int _animationDodge; //DoDodge
    private int _animationGetBuff; //GetBuff
    private int _animationThrow; //Throw
    private int _animationJump; //DoJump
    private int _animationLand; //DoLand
    private int _animationVictoryMove; //DoVictoryMove
    private int _animationDefeatMove; //DoDefeatMove
    private int _animationLookAround; //DoLookAround
    #endregion

    #region Bools
    private int _animationCastingSingleSelf; //CastingSingleSelf
    private int _animationCastingSingleTarget; //CastingSingleTarget
    private int _animationCastingAoeSelf; //CastingAoeSelf
    private int _animationCastingAoeTarget; //CastingAoeTarget
    private int _animationRunning; //Running
    private int _animationFloating; //Floating
    private int _animationStunned; //IsStunned
    private int _animationParalyzed; //IsParalyzed
    private int _animationDead; //IsDead
    #endregion

    #endregion

    #region Movement

    private bool _isWalking = false;
    private List<PathRef> _targetPath;
    private PathRef _currentSpot;
    private PathRef _targetSpot;
    private Vector3 _targetPosition;
    private int _targetHeigthDiff;
    private bool _jumping = false;
    private bool _delayedJump = false;
    private float _lastDistance;
    private float? _keepPosAdjustmentTimer;
    private Quaternion _rotationToKeep;

    private float? _baseAttackDoneTimer;
    private float? _getHitTimer;
    private float? _evadeTimer;
    private bool _isBackAttack = false;

    #endregion

    #region Events

    public event System.EventHandler MovementDone;
    public event System.EventHandler AttackDone;

    #endregion

    void Start()
    {
    }

    private void Update()
    {
        if (_isWalking)
        {
            var newPosition = Vector3.MoveTowards(transform.position, _targetPosition, 6f * Time.deltaTime);

            if (_jumping)
            {
                var jumpFactor = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(newPosition.y)) * (float)_targetHeigthDiff;
                newPosition = newPosition + new Vector3(0f, jumpFactor, 0f);

                if ((_targetHeigthDiff > 0 && newPosition.y > _targetPosition.y) || (_targetHeigthDiff < 0 && newPosition.y < _targetPosition.y))
                    newPosition.y = _targetPosition.y;
            }

            //Keep going for a short distance
            if (_delayedJump)
            {
                var direction = transform.forward;
                newPosition = transform.position + (direction * 6f * Time.deltaTime);
            }

            var distance = Vector2.Distance(new Vector2(newPosition.x, newPosition.z), new Vector2(_targetPosition.x, _targetPosition.z));
            
            if (_delayedJump && distance <= 2.5f)
            {
                _jumping = true;
                _delayedJump = false;
                Animator.SetTrigger(_animationJump);
            }

            if (_jumping && distance <= 2f)
            {
                _jumping = false;
                if (!_targetPath.Any())
                {
                    Animator.SetBool(_animationRunning, false);
                }
                Animator.SetTrigger(_animationLand);
            }

            if (distance >= _lastDistance)
            {
                //Last spot, persist adjustment
                if (!_targetPath.Any())
                {
                    _keepPosAdjustmentTimer = Time.time + 1f;
                    _rotationToKeep = AppFunctions.GetRotation(Character.FaceDirection);
                }

                GoToNextSpot();
            }
            else
            {
                transform.position = newPosition;
                _lastDistance = distance;
            }
        }

        //Persist for animation motion adjustment
        if (_keepPosAdjustmentTimer.HasValue)
        {
            if (Time.time <= _keepPosAdjustmentTimer.Value)
            {
                transform.position = _targetPosition;
                transform.rotation = _rotationToKeep;
            }
            else
                _keepPosAdjustmentTimer = null;
        }

        if (_baseAttackDoneTimer.HasValue && _baseAttackDoneTimer.Value <= Time.time)
        {
            _baseAttackDoneTimer = null;
            AttackDone?.Invoke(null, System.EventArgs.Empty);
        }

        if (_getHitTimer.HasValue && _getHitTimer.Value <= Time.time)
        {
            _getHitTimer = null;

            if (_isBackAttack)
                Animator.SetTrigger(_animationBackHit);
            else
                Animator.SetTrigger(_animationFrontHit);
        }

        if (_evadeTimer.HasValue && _evadeTimer.Value <= Time.time)
        {
            _evadeTimer = null;
            Animator.SetTrigger(_animationDodge);
        }
    }

    public void SetCharacterData(CharacterGameplay character)
    {
        Character = character;

        SetAppearance();
        SetAnimationData();
    }

    #region Animation definition

    private void SetAnimationData()
    {
        var animInstance = AppConts.AnimatorControllerPath.UNARMED;

        var mainHand = Character.BaseInfo.MainHand;
        if (mainHand != null)
        {
            if (mainHand.BaseModel.WeaponType == (int)WeaponType.TwoHandAxe)
                animInstance = AppConts.AnimatorControllerPath.TWO_HANDED_SWORD;
        }

        Animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animInstance);

        _animationAttack = Animator.StringToHash("DoAttack");
        _animationSpecialAttack = Animator.StringToHash("DoSpecialAttack");
        _animationFrontHit = Animator.StringToHash("TakeFrontHit");
        _animationBackHit = Animator.StringToHash("TakeBackHit");
        _animationBlock = Animator.StringToHash("DoBlock");
        _animationDodge = Animator.StringToHash("DoDodge");
        _animationGetBuff = Animator.StringToHash("GetBuff");
        _animationThrow = Animator.StringToHash("Throw");
        _animationJump = Animator.StringToHash("DoJump");
        _animationLand = Animator.StringToHash("DoLand");
        _animationVictoryMove = Animator.StringToHash("DoVictoryMove");
        _animationDefeatMove = Animator.StringToHash("DoDefeatMove");
        _animationLookAround = Animator.StringToHash("DoLookAround");
        _animationCastingSingleSelf = Animator.StringToHash("CastingSingleSelf");
        _animationCastingSingleTarget = Animator.StringToHash("CastingSingleTarget");
        _animationCastingAoeSelf = Animator.StringToHash("CastingAoeSelf");
        _animationCastingAoeTarget = Animator.StringToHash("CastingAoeTarget");
        _animationRunning = Animator.StringToHash("Running");
        _animationFloating = Animator.StringToHash("Floating");
        _animationStunned = Animator.StringToHash("IsStunned");
        _animationParalyzed = Animator.StringToHash("IsParalyzed");
        _animationDead = Animator.StringToHash("IsDead");
    }

    #endregion

    #region Appearance definition

    //TODO: Implementar skins e os materials
    private void SetAppearance()
    {
        #region Get configuration

        //TODO: criar variáveis equivalentes para armazenamento da skin (material) - Equipamento e pele
        var genderParts = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        var attachParts = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        var headCoverings = new int[3] { 0, 0, 0 };

        if (Character.BaseInfo.EquipSlots != null)
        {
            //Order to define priority to render
            foreach (var e in Character.BaseInfo.EquipSlots.Where(w => w.BaseModel != null && w.BaseModel.Slot < 8).OrderBy(o => o.BaseModel.Slot))
            {
                foreach (var cmp in e.BaseModel.Composition)
                {
                    //Gender part
                    var partGroup = cmp[1];
                    if (cmp[0] == 0)
                    {
                        //If the part is not set (priority order)
                        if (genderParts[partGroup] == 0)
                        {
                            //Full helm
                            if (partGroup == 0)
                            {
                                genderParts[partGroup] = cmp[3];
                            }
                            else
                            {
                                genderParts[partGroup] = cmp[2];
                            }
                        }
                    }
                    else //Attachment
                    {
                        //If the attachment is not set (priority order)
                        if (attachParts[partGroup] == 0)
                        {
                            //Head Coverings
                            if (partGroup == 0)
                            {
                                //Just setting as set
                                attachParts[partGroup] = cmp[2];
                                headCoverings[cmp[2]] = cmp[3];
                            }
                            else
                            {
                                attachParts[partGroup] = cmp[2];
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Enable objects

        #region Gender Parts

        var baseBody = BodySkin.GetChild(Character.BaseInfo.Gender);
        baseBody.gameObject.SetActive(true);

        for (var i = 0; i < 9; i++)
        {
            if (i == 1 && (Character.BaseInfo.Eyebrows == 0 || genderParts[0] > 0))
                continue;
            if (i == 2 && (Character.BaseInfo.Gender == 1 || Character.BaseInfo.FacialHair == 0 || genderParts[0] > 0 || headCoverings[1] > 0))
                continue;

            var basePart = baseBody.GetChild(i);
            basePart.gameObject.SetActive(true);

            if (i == 0)
            {
                //Has full helm, don't hender Head
                if (genderParts[i] > 0)
                {
                    var helmPart = basePart.GetChild(1);
                    helmPart.gameObject.SetActive(true);

                    var part = helmPart.GetChild(genderParts[i]);
                    SetActiveCrawl(part, true);
                }
                else
                {
                    var headPart = basePart.GetChild(0);
                    headPart.gameObject.SetActive(true);

                    var part = headPart.GetChild(Character.BaseInfo.Head);
                    SetActiveCrawl(part, true);
                }
            }
            else if (i == 1)
            {
                var part = basePart.GetChild(Character.BaseInfo.Eyebrows);
                SetActiveCrawl(part, true);
            }
            else if (i == 2) //No facial hair for females
            {
                var part = basePart.GetChild(Character.BaseInfo.FacialHair);
                SetActiveCrawl(part, true);
            }
            else
            {
                var part = basePart.GetChild(genderParts[i]);
                SetActiveCrawl(part, true);
            }
        }

        #endregion

        #region Attachments

        var baseAttach = BodySkin.GetChild(2);
        baseAttach.gameObject.SetActive(true);

        for (var i = 0; i < 10; i++)
        {
            if (attachParts[i] == 0 && i != 1)
                continue;

            if (i == 1 && (Character.BaseInfo.Hair == 0 || genderParts[0] > 0 || headCoverings[2] > 0))
                continue;

            var basePart = baseAttach.GetChild(i);
            basePart.gameObject.SetActive(true);

            if (i == 0)
            {
                var headAttach = baseAttach.GetChild(attachParts[0]);
                headAttach.gameObject.SetActive(true);

                var part = basePart.GetChild(headCoverings[attachParts[0]]);
                SetActiveCrawl(part, true);
            }
            else if (i == 1)
            {
                var part = basePart.GetChild(Character.BaseInfo.Hair);
                SetActiveCrawl(part, true);
            }
            else
            {
                var part = basePart.GetChild(attachParts[i]);
                SetActiveCrawl(part, true);
            }
        }

        #endregion

        #endregion
    }

    //TODO: Passar a skin (material) também - Equipamento e pele
    private void SetActiveCrawl(Transform target, bool active)
    {
        target.gameObject.SetActive(active);

        if (target.childCount > 0)
        {
            for (var i = 0; i < target.childCount; i++)
            {
                SetActiveCrawl(target.GetChild(i), active);
            }
        }
    }

    #endregion

    #region Movement

    public void MoveOnPath(List<PathRef> path)
    {
        _targetPath = path;
        _targetSpot = _targetPath[0];
        _targetPath.RemoveAt(0);

        _isWalking = true;
        Animator.SetBool(_animationRunning, true);
        GoToNextSpot();
    }

    private void GoToNextSpot()
    {
        _currentSpot = _targetSpot;
        Character.PosX = _currentSpot.PosX;
        Character.PosY = _currentSpot.PosY;

        if (_targetPath.Any())
        {
            _targetSpot = _targetPath[0];
            _targetPosition = _targetSpot.Position;

            _targetHeigthDiff = _targetSpot.Height - _currentSpot.Height;
            if (_targetHeigthDiff > 1)
            {
                _jumping = true;
                Animator.SetTrigger(_animationJump);
            }
            else if (_targetHeigthDiff < -1)
            {
                _delayedJump = true;
            }

            _lastDistance = 999;
            _targetPath.RemoveAt(0);

            FaceDirection(_targetSpot.PosX, _targetSpot.PosY);
        }
        else
        {
            _currentSpot = null;
            _targetSpot = null;
            _isWalking = false;
            Animator.SetBool(_animationRunning, false);
            MovementDone?.Invoke(null, System.EventArgs.Empty);
        }
    }

    public void FaceDirection(int posX, int posY, bool forceRotationFix = false)
    {
        var targetX = posX - Character.PosX;
        var targetY = posY - Character.PosY;
        Character.FaceDirection = AppFunctions.GetDirectionNum(targetX, targetY);
        FaceDirection(Character.FaceDirection, forceRotationFix);
    }

    public void FaceDirection(int faceDirection, bool forceRotationFix = false)
    {
        var newRotation = AppFunctions.GetRotation(faceDirection);
        transform.rotation = newRotation;
        if (forceRotationFix)
            _rotationToKeep = transform.rotation;
    }

    public void FixPosition(Vector3 position)
    {
        if (_keepPosAdjustmentTimer.HasValue)
            _targetPosition = position;
    }

    #endregion

    #region Combat

    public void AttackOnTarget(Vector3 positionDifference)
    {
        transform.rotation = Quaternion.LookRotation(positionDifference);
        _rotationToKeep = transform.rotation;
        Animator.SetTrigger(_animationAttack);
        _baseAttackDoneTimer = Time.time + 1f;
    }

    public void TakeDamage(int damage, float animationDelay, bool isBackAttack = false)
    {
        //TODO: Processar o dano e verificar se é animação de morte
        _isBackAttack = isBackAttack;
        _getHitTimer = Time.time + animationDelay;
    }

    public void Evade(float animationDelay)
    {
        _evadeTimer = Time.time + animationDelay;
    }

    #endregion
}
