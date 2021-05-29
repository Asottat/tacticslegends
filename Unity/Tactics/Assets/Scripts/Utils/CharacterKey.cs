public struct CharacterKey
{
    public long CharacterId;
    public long PlayerId;

    public CharacterKey(long characterId, long playerId)
    {
        CharacterId = characterId;
        PlayerId = playerId;
    }
}