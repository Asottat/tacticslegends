using Assets.Scripts.Services.LocalData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WSCustumeTest : MonoBehaviour
{
    public Text ButtonTextGender;
    public Text[] ButtonTexts;
    public Text ButtonTextHelmet;
    public Text[] ButtonHeadAttTexts;
    public Text[] ButtonAttchmentTexts;

    int currentGender = 0;
    int[] GenderParts = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int GenderHelmet = 0;
    int[] HeadAttachment = { 0, 0, 0 };
    int[] Attachments = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    int[] GenderPartsCap = { 22, 10, 18, 28, 20, 18, 17, 28, 19 };
    int GenderHelmetCap = 13;
    int[] HeadAttachmentCap = { 11, 4, 13 };
    int[] AttachmentsCap = { 0, 38, 13, 0, 15, 21, 6, 12, 11, 3 };
    //TODO: Colocar o cap por gênero

    public Transform ModularCharacter;
    public Transform ModularCharacterBody;

    public static bool FreezeRotation = false;

    void Start()
    {
        var x = new BaseEquipService();
        var y = x.GetBaseEquipBySlot(Assets.Scripts.Enums.ItemSlotType.Chest);

        RemakeCharacter();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
            FreezeRotation = !FreezeRotation;
    }

    public void ChangeGender()
    {
        currentGender = currentGender == 0 ? 1 : 0;
        ButtonTextGender.text = "Gender: " + (currentGender == 0 ? "Male" : "Female");

        RemakeCharacter();
    }

    public void ChangePart(int num)
    {
        GenderParts[num]++;
        if (GenderParts[num] > GenderPartsCap[num])
            GenderParts[num] = 0;

        ButtonTexts[num].text = ButtonTexts[num].name + ": " + GenderParts[num];

        RemakeCharacter();
    }
    public void ClearPart(int num)
    {
        GenderParts[num] = GenderPartsCap[num];
        ChangePart(num);
    }

    public void ChangeHelmet()
    {
        GenderHelmet++;
        if (GenderHelmet > GenderHelmetCap)
        {
            GenderHelmet = 0;

            Attachments[2] = 0;
            ButtonAttchmentTexts[2].text = ButtonAttchmentTexts[2].name + ": 0";
        }

        ButtonTextHelmet.text = "Closed Helmet: " + GenderHelmet;

        HeadAttachment[0] = 0;
        ButtonHeadAttTexts[0].text = ButtonHeadAttTexts[0].name + ": 0";
        HeadAttachment[1] = 0;
        ButtonHeadAttTexts[1].text = ButtonHeadAttTexts[1].name + ": 0";
        HeadAttachment[2] = 0;
        ButtonHeadAttTexts[2].text = ButtonHeadAttTexts[2].name + ": 0";

        RemakeCharacter();
    }
    public void ClearHelmet()
    {
        GenderHelmet = GenderHelmetCap;
        ChangeHelmet();
    }

    public void ChangeHeadAttachment(int num)
    {
        HeadAttachment[num]++;
        if (HeadAttachment[num] > HeadAttachmentCap[num])
            HeadAttachment[num] = 0;

        ButtonHeadAttTexts[num].text = ButtonHeadAttTexts[num].name + ": " + HeadAttachment[num];

        GenderHelmet = 0;
        ButtonTextHelmet.text = "Closed Helmet: 0";

        Attachments[2] = 0;
        ButtonAttchmentTexts[2].text = ButtonAttchmentTexts[2].name + ": 0";

        if (num != 0)
        {
            HeadAttachment[0] = 0;
            ButtonHeadAttTexts[0].text = ButtonHeadAttTexts[0].name + ": 0";
        }

        if (num != 1)
        {
            HeadAttachment[1] = 0;
            ButtonHeadAttTexts[1].text = ButtonHeadAttTexts[1].name + ": 0";
        }

        if (num != 2)
        {
            HeadAttachment[2] = 0;
            ButtonHeadAttTexts[2].text = ButtonHeadAttTexts[2].name + ": 0";
        }

        RemakeCharacter();
    }
    public void ClearHeadAttachment(int num)
    {
        HeadAttachment[num] = HeadAttachmentCap[num];
        ChangeHeadAttachment(num);
    }

    public void ChangeAttachment(int num)
    {
        if (num == 2 && GenderHelmet == 0)
            return;

        Attachments[num]++;
        if (Attachments[num] > AttachmentsCap[num])
            Attachments[num] = 0;

        ButtonAttchmentTexts[num].text = ButtonAttchmentTexts[num].name + ": " + Attachments[num];

        RemakeCharacter();
    }
    public void ClearAttachment(int num)
    {
        Attachments[num] = AttachmentsCap[num];
        ChangeAttachment(num);
    }

    public void ClearAllSettings()
    {
        currentGender = 0;
        ButtonTextGender.text = "Gender: Male";
        for (var i = 0; i < GenderParts.Length; i++)
        {
            GenderParts[i] = 0;
            ButtonTexts[i].text = ButtonTexts[i].name + ": 0";
        }
        GenderHelmet = 0;
        ButtonTextHelmet.text = "Closed Helmet: 0";
        for (var i = 0; i < HeadAttachment.Length; i++)
        {
            HeadAttachment[i] = 0;
            ButtonHeadAttTexts[i].text = ButtonHeadAttTexts[i].name + ": 0";
        }
        for (var i = 0; i < Attachments.Length; i++)
        {
            Attachments[i] = 0;
            if (ButtonAttchmentTexts[i] != null)
                ButtonAttchmentTexts[i].text = ButtonAttchmentTexts[i].name + ": 0";
        }

        RemakeCharacter();
    }

    public void Rotate(float direction)
    {
        ModularCharacter.Rotate(Vector3.up, (30f * direction));
    }

    public void ExtractEquipDataForFile()
    {
        List<string> data = new List<string>();

        for (var i = 3; i < GenderParts.Length; i++)
        {
            var gp = GenderParts[i];
            if (gp > 0)
            {
                data.Add(string.Concat("0:", i, ":", gp));
            }
        }
        if (GenderHelmet > 0)
        {
            data.Add(string.Concat("0:0:1:" + GenderHelmet));
        }
        for (var i = 0; i < HeadAttachment.Length; i++)
        {
            var gp = HeadAttachment[i];
            if (gp > 0)
            {
                data.Add(string.Concat("2:0:", i, ":", gp));
            }
        }
        for (var i = 2; i < Attachments.Length; i++)
        {
            var gp = Attachments[i];
            if (gp > 0)
            {
                if (i == 5)
                    data.Add(string.Concat("2:", i, ":", gp));
                else
                    data.Add(string.Concat("2:", i, ":", gp));
            }
        }

        if (data.Count > 0)
        {
            var combs = string.Concat("\"", string.Join("\",\"", data), "\"");
            Debug.Log(string.Concat("_allEquip.Add(new BaseEquipModel(X,Y,Z,", combs, "));"));
            //_allEquip.Add(new BaseEquipModel(X,Y,Z,"0:3:11","0:4:17","0:5:8"));
        }
        else
        {
            Debug.LogError("No equip data to extract");
        }
    }
    //Pos 0: 0 = ByGender, 2 = Attachment
    //X- é categoria: 0 = leve, 1 = médio, 2 = pesado

    private void ResetCharacterBody()
    {
        SetActiveCrawl(ModularCharacterBody, false);
    }

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

    private void RemakeCharacter()
    {
        ResetCharacterBody();

        #region Gender Specific parts

        var baseBody = ModularCharacterBody.GetChild(currentGender);
        baseBody.gameObject.SetActive(true);

        for (var slot = 0; slot <= 8; slot++)
        {
            var slotValue = GenderParts[slot];

            //No Facial Hair or Eyebrows = Nothing to render
            if (slot == 1 || slot == 2)
            {
                if (slotValue == 0)
                    continue;

                //No Facial Hair for Female
                if (slot == 2 && currentGender == 1)
                    continue;

                slotValue--;
            }

            //If has helmet, don't render Facial Hair or Eyebrows
            if ((slot == 1 || slot == 2) && GenderHelmet > 0)
                continue;

            //I has Mask, don't Facial Hair
            if (slot == 2 && HeadAttachment[1] > 0)
                continue;

            var basePart = baseBody.GetChild(slot);
            basePart.gameObject.SetActive(true);

            if (slot == 0)
            {
                if (GenderHelmet == 0)
                {
                    var headArea = basePart.GetChild(0);
                    headArea.gameObject.SetActive(true);

                    var part = headArea.GetChild(slotValue);
                    SetActiveCrawl(part, true);
                }
                else
                {
                    var headArea = basePart.GetChild(1);
                    headArea.gameObject.SetActive(true);

                    var part = headArea.GetChild(GenderHelmet - 1);
                    SetActiveCrawl(part, true);
                }
            }
            else
            {
                var part = basePart.GetChild(slotValue);
                SetActiveCrawl(part, true);
            }
        }

        ModularCharacterBody.gameObject.SetActive(true);

        #endregion

        #region No Gender Attachments

        var baseAttach = ModularCharacterBody.GetChild(2);
        var hasAttachs = false;

        for (var headAttType = 0; headAttType <= 2; headAttType++)
        {
            var slot = HeadAttachment[headAttType];

            if (slot == 0)
                continue;

            hasAttachs = true;
            var baseHeadAttach = baseAttach.GetChild(0);
            baseHeadAttach.gameObject.SetActive(true);
            var headAttachPart = baseHeadAttach.GetChild(headAttType);
            headAttachPart.gameObject.SetActive(true);

            var part = headAttachPart.GetChild(slot - 1);
            SetActiveCrawl(part, true);
        }

        for (var attType = 1; attType <= 9; attType++)
        {
            //If has a helmet, don't render Hair
            if (attType == 1 && (HeadAttachment[2] > 0 || GenderHelmet > 0))
                continue;

            var slot = Attachments[attType];

            if (slot == 0)
                continue;

            hasAttachs = true;
            var baseAttachSlot = baseAttach.GetChild(attType);
            baseAttachSlot.gameObject.SetActive(true);

            var part = baseAttachSlot.GetChild(slot - 1);
            SetActiveCrawl(part, true);

        }
        //IF HAIR CONTINUE IF HeadAttachment[2] > 0

        if (hasAttachs)
        {
            baseAttach.gameObject.SetActive(true);
        }

        #endregion
    }
}
