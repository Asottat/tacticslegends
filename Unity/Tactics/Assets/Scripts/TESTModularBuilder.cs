using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTModularBuilder : MonoBehaviour
{
    public Transform BodyTransform;

    //TODO: Isso deve estar em uma classe
    public GenderType? genderType;
    public GenderPart? genderPart;
    public int? genderPartNum;
    public HeadType? headType;
    public int? headPartNum;
    public NoGenderPart? noGenderPart;
    public int? noGenderPartNum;
    public HeadCoveringType? headCoveringType;
    public int? headCoveringNum;

    void Start()
    {
        BodyBuilder();
    }

    void Update()
    {
        
    }

    //TODO: Criar um enum pra indicar as partes que são "gêmeas" (mãos, pés, ombreiras, joelheiras e cotoveleiras...)
    //TODO: Colocar braços todos juntos, superior e inferior em um diretório indicando a posição
    //TODO: Colocar as cotoveleiras como parte do braço (descartar o que não for usar)
    //TODO: Colocar as joelheiras como parte das pernas, em um diretório (descartar o que não for usar)

    void BodyBuilder()
    {
        if (genderType.HasValue)
        {
            switch (genderType.Value)
            {
                case GenderType.None:
                    break;
                default:
                    break;
            }
        }
    }
}
