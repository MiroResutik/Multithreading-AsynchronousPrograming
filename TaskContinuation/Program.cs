
// Continuation example using async/await to fetch data from the PokeAPI and display the first Pokemon's name, weight, and height.
/*
using System.Text.Json;
using var client = new HttpClient();
var pokemonListJson = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

// Get the first Pokemon's url
var doc = JsonDocument.Parse(pokemonListJson);
JsonElement root = doc.RootElement;
JsonElement results = root.GetProperty("results");
JsonElement firstPokemon = results[0];
var url = firstPokemon.GetProperty("url").ToString();

// Get the first Pokemon's detailed info#
// await is used to asynchronously wait for the HTTP request to complete
// and get the response as a string.
var firstPokemonJson = await client.GetStringAsync(url);

// Get the weight and height
doc = JsonDocument.Parse(firstPokemonJson);
root = doc.RootElement;
Console.WriteLine($"Name: {root.GetProperty("name").ToString()}");
Console.WriteLine($"Weight: {root.GetProperty("weight").ToString()}");
Console.WriteLine($"Height: {root.GetProperty("height").ToString()}");
Console.WriteLine("This is the end of the program.");
Console.ReadLine();
*/

// Extended version of the above code that fetches and displays the first 5 Pokemon's names, weights, and heights.

using System.Text.Json;

using var client = new HttpClient();

Console.WriteLine("\nDownloading Pokémon list...\n");


// Download the list of Pokémon


string pokemonListJson =
    await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

// Parse the JSON document
using JsonDocument doc = JsonDocument.Parse(pokemonListJson);

JsonElement root = doc.RootElement;

// Read some top-level properties
Console.WriteLine($"Total Pokémon : {root.GetProperty("count")}");
Console.WriteLine($"Next Page     : {root.GetProperty("next")}");
Console.WriteLine();

// Read the results array
JsonElement results = root.GetProperty("results");

Console.WriteLine("First 5 Pokémon");
Console.WriteLine("----------------");

foreach (JsonElement pokemon in results.EnumerateArray().Take(5))
{
    Console.WriteLine(
        $"{pokemon.GetProperty("name"),-15} {pokemon.GetProperty("url")}");
}

Console.WriteLine();

// Get the first Pokémon
JsonElement firstPokemon = results[0];

string pokemonName =
    firstPokemon.GetProperty("name").GetString()!;

string pokemonUrl =
    firstPokemon.GetProperty("url").GetString()!;

Console.WriteLine($"Downloading details for {pokemonName}...\n");



// Download the Pokémon details

string pokemonJson =
    await client.GetStringAsync(pokemonUrl);

using JsonDocument pokemonDoc =
    JsonDocument.Parse(pokemonJson);

JsonElement pokemonRoot =
    pokemonDoc.RootElement;



// Basic Information

Console.WriteLine("Basic Information");
Console.WriteLine("-----------------");

Console.WriteLine($"Name            : {pokemonRoot.GetProperty("name")}");
Console.WriteLine($"ID              : {pokemonRoot.GetProperty("id")}");
Console.WriteLine($"Height          : {pokemonRoot.GetProperty("height")}");
Console.WriteLine($"Weight          : {pokemonRoot.GetProperty("weight")}");
Console.WriteLine($"Base Experience : {pokemonRoot.GetProperty("base_experience")}");



// Types

Console.WriteLine("\nTypes");
Console.WriteLine("-----");

foreach (JsonElement type in pokemonRoot.GetProperty("types").EnumerateArray())
{
    Console.WriteLine(
        type.GetProperty("type")
            .GetProperty("name"));
}



// Abilities

Console.WriteLine("\nAbilities");
Console.WriteLine("---------");

foreach (JsonElement ability in pokemonRoot.GetProperty("abilities").EnumerateArray())
{
    Console.WriteLine(
        ability.GetProperty("ability")
               .GetProperty("name"));
}



// Base Stats

Console.WriteLine("\nBase Stats");
Console.WriteLine("----------");

foreach (JsonElement stat in pokemonRoot.GetProperty("stats").EnumerateArray())
{
    string statName =
        stat.GetProperty("stat")
            .GetProperty("name")
            .GetString()!;

    int statValue =
        stat.GetProperty("base_stat")
            .GetInt32();

    Console.WriteLine($"{statName,-15} {statValue}");
}



// First 10 Moves

Console.WriteLine("\nFirst 10 Moves");
Console.WriteLine("--------------");

foreach (JsonElement move in pokemonRoot.GetProperty("moves")
                                        .EnumerateArray()
                                        .Take(10))
{
    Console.WriteLine(
        move.GetProperty("move")
            .GetProperty("name"));
}


// Sprites

Console.WriteLine("\nSprites");
Console.WriteLine("-------");

Console.WriteLine($"Front Default : {pokemonRoot.GetProperty("sprites").GetProperty("front_default")}");
Console.WriteLine($"Back Default  : {pokemonRoot.GetProperty("sprites").GetProperty("back_default")}");


// Game Indices

Console.WriteLine("\nGame Versions");
Console.WriteLine("-------------");

foreach (JsonElement game in pokemonRoot.GetProperty("game_indices").EnumerateArray())
{
    Console.WriteLine(
        game.GetProperty("version")
            .GetProperty("name"));
}


// Cries

Console.WriteLine("\nCries");
Console.WriteLine("-----");

Console.WriteLine($"Latest : {pokemonRoot.GetProperty("cries").GetProperty("latest")}");
Console.WriteLine($"Legacy : {pokemonRoot.GetProperty("cries").GetProperty("legacy")}");


Console.WriteLine("\nFinished!");

Console.ReadLine();