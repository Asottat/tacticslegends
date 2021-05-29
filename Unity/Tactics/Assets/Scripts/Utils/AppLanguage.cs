namespace Assets.Scripts.Utils
{
    public static class AppLanguage
    {
        private static string[] _texts = new string[14];
        private static bool _translationsReady = false; //TODO: REMOVER A VERIFICAÇÃO E PASSAR A CHAMAR O SetLanguageData no início da aplicação (preload)

        public static string GetText(int id)
        {
            if (!_translationsReady) //TODO: REMOVER
                SetLanguageData(0);

            return _texts[id];
        }

        public static string GetText(int id, params object[] args)
        {
            return string.Format(GetText(id), args);
        }

        public static void SetLanguageData(int languageId)
        {
            if (languageId == 0)
            {
                _texts[0] = "Choose your champions";
                _texts[1] = "All available heroes have already been positioned";
                _texts[2] = "Remove Champion";
                _texts[3] = "Do you want to\nremove <color=#FFFE84>{0}</color>?";
                _texts[4] = "<color=#FFFE84>{0}</color> has been removed";
                _texts[5] = "Ready!";
                _texts[6] = "Are you ready\nto start?";
                _texts[7] = "Waiting your opponent";
                _texts[8] = "Combat started";
                _texts[9] = "Accuracy";
                _texts[10] = "Damage";
                _texts[11] = "Type";
                _texts[12] = "Normal attack";
                _texts[13] = "From behind";
            }
            else if (languageId == 1)
            {
                _texts[0] = "Escolha seus campeões";
                _texts[1] = "Todos campeões disponíveis já foram posicionados";
                _texts[2] = "Remover Campeão";
                _texts[3] = "Confirma remoção\nde <color=#FFFE84>{0}</color>?";
                _texts[4] = "<color=#FFFE84>{0}</color> foi removido(a)";
                _texts[5] = "Pronto!";
                _texts[6] = "Você está pronto\npara começar?";
                _texts[7] = "Aguardando seu oponente";
                _texts[8] = "Combate iniciado";
                _texts[9] = "Precisão";
                _texts[10] = "Dano";
                _texts[11] = "Tipo";
                _texts[12] = "Ataque normal";
                _texts[13] = "Pelas costas";
            }
        }
    }
}
