using System;
using System.Collections.Generic;

[Serializable]
public class PokemonListResponse
{
    public int count;
    public string next;
    public string previous;
    public List<PokemonEntry> results;
}

[Serializable]
public class PokemonEntry
{
    public string name;
    public string url;
}

[Serializable]
public class PokemonDetail
{
    public int id;
    public string name;
    public int height;
    public int weight;
    public Sprites sprites;
    public List<TypeEntry> types;
    public List<StatEntry> stats;
}

[Serializable]
public class Sprites
{
    public string front_default;
    public string front_shiny;
    public Other other;
}

[Serializable]
public class Other
{
    public OfficialArtwork official_artwork;
}

[Serializable]
public class OfficialArtwork
{
    public string front_default;
}

[Serializable]
public class TypeEntry
{
    public int slot;
    public TypeInfo type;
}

[Serializable]
public class TypeInfo
{
    public string name;
    public string url;
}

[Serializable]
public class StatEntry
{
    public int base_stat;
    public int effort;
    public StatInfo stat;
}

[Serializable]
public class StatInfo
{
    public string name;
    public string url;
}

[Serializable]
public class PokemonSpecies
{
    public int id;
    public string name;
    public List<FlavorTextEntry> flavor_text_entries;
    public EvolutionChain evolution_chain;
}

[Serializable]
public class FlavorTextEntry
{
    public string flavor_text;
    public Language language;
    public Version version;
}

[Serializable]
public class Language
{
    public string name;
    public string url;
}

[Serializable]
public class Version
{
    public string name;
    public string url;
}

[Serializable]
public class EvolutionChain
{
    public string url;
}
