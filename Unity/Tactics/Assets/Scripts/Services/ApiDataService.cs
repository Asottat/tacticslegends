using Assets.Scripts.Entities;
using Assets.Scripts.Services.LocalData;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ApiDataService : MonoBehaviour
{
    static ApiDataService _instance;
    public static ApiDataService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ApiDataService>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(ApiDataService).Name;
                    _instance = go.AddComponent<ApiDataService>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

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

    #region Characters

    public IEnumerator GetCharactersByPlayer(int playerId, System.Action<List<Character>> callBack)
    {
        //TODO: Implementar o consumo da API
        callBack(FAKE_GetCharactersByPlayer(playerId));
        yield return null;
    }

    public IEnumerator GetCharactersCompleteByPlayer(int playerId, System.Action<List<CharacterGameplay>> callBack)
    {
        //TODO: Implementar o consumo da API
        callBack(FAKE_GetCharactersByPlayer(playerId).Select(s => CompleteCharacter(s)).ToList());
        yield return null;
    }

    private CharacterGameplay CompleteCharacter(Character character)
    {
        var cc = new CharacterGameplay()
        {
            BaseInfo = character,
            HealthMax = character.BaseHealth,
            HealthCurrent = character.BaseHealth,
            Vitality = character.BaseVitality,
            Strength = character.BaseStrength,
            Dextery = character.BaseDextery,
            Intelligence = character.BaseIntelligence,
            Evasion = character.BaseEvasion,
            Movement = character.BaseMovement,
            Jump = character.BaseMovement,
            Speed = character.BaseSpeed
        };

        cc.MainHandWeaponSkill = 1f; //TODO: recuperar da arma x dados do personagem (detalhes na planilha excel)
        cc.OffHandWeaponSkill = 1f; //TODO: recuperar

        cc.Power = 1000; //TODO: Implementar lógica de cálculo de Power

        foreach (var e in cc.BaseInfo.EquipSlots)
        {
            if (e.BaseModelId.HasValue)
                e.BaseModel = BaseEquipService.GetById(e.BaseModelId.Value);
        }

        if (cc.BaseInfo.MainHand != null && cc.BaseInfo.MainHand.BaseModelId.HasValue)
            cc.BaseInfo.MainHand.BaseModel = BaseWeaponService.GetById(cc.BaseInfo.MainHand.BaseModelId.Value);

        if (cc.BaseInfo.OffHand != null && cc.BaseInfo.OffHand.BaseModelId.HasValue)
            cc.BaseInfo.OffHand.BaseModel = BaseWeaponService.GetById(cc.BaseInfo.OffHand.BaseModelId.Value);

        cc.BaseAttackRange = AppFunctions.GetWeaponRangeByType(cc.BaseInfo.MainHand?.BaseModel?.WeaponType, cc.BaseInfo.OffHand?.BaseModel?.WeaponType);

        return cc;
    }

    #endregion


    //TODO: Remover o fake
    private List<Character> FAKE_GetCharactersByPlayer(int playerId)
    {
        List<Character> characters = new List<Character>();

        if (playerId == 0)
        {
            characters.Add(new Character()
            {
                Id = 3,
                PlayerId = 0,
                Name = "Cara Ruim",
                Level = 10,
                Gender = 0,
                Eyebrows = 8,
                FacialHair = 10,
                Head = 12,
                Hair = 12,
                BaseHealth = 400,
                BaseVitality = 100,
                BaseStrength = 100,
                BaseDextery = 100,
                BaseIntelligence = 100,
                BaseEvasion = 100,
                BaseMovement = 3,
                BaseJump = 2,
                BaseSpeed = 55,
                EquipSlots = new List<Equipment>()
                    {
                        new Equipment() { BaseModelId = 33 },
                        new Equipment() { BaseModelId = 85 }
                    }
            });
        }

        if (playerId == 1)
        {
            characters.Add(new Character()
            {
                Id = 1,
                PlayerId = 1,
                Name = "Hasdrubhal",
                Level = 10,
                Gender = 0,
                Eyebrows = 3,
                FacialHair = 4,
                Head = 5,
                Hair = 4,
                BaseHealth = 400,
                BaseVitality = 100,
                BaseStrength = 100,
                BaseDextery = 100,
                BaseIntelligence = 100,
                BaseEvasion = 100,
                BaseMovement = 2,
                BaseJump = 1,
                BaseSpeed = 40,
                EquipSlots = new List<Equipment>()
                {
                    //new Equipment() { BaseModelId = 187 },
                }
            });
            characters.Add(new Character()
            {
                Id = 2,
                PlayerId = 1,
                Name = "Brunette",
                Level = 10,
                Gender = 1,
                Eyebrows = 5,
                FacialHair = 0,
                Head = 8,
                Hair = 9,
                BaseHealth = 400,
                BaseVitality = 100,
                BaseStrength = 100,
                BaseDextery = 30,
                BaseIntelligence = 100,
                BaseEvasion = 75,
                BaseMovement = 3,
                BaseJump = 3,
                BaseSpeed = 65,
                EquipSlots = new List<Equipment>()
                {
                    //new Equipment() { BaseModelId = 216 },
                    //new Equipment() { BaseModelId = 19 },
                    //new Equipment() { BaseModelId = 76 },
                    //new Equipment() { BaseModelId = 74 },
                    //new Equipment() { BaseModelId = 121 },
                    //new Equipment() { BaseModelId = 131 },
                    //new Equipment() { BaseModelId = 229 }
                },
                MainHand = new Weapon() { BaseModelId = 47 },
                //OffHand = new Weapon() { BaseModelId = 52 }
            });
        }

        return characters;
    }

}
