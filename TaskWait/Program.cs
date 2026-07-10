// Target Framework: .NET 8
//
// Demonstrates:
//
// • Making asynchronous HTTP requests
// • Continuations using ContinueWith()
// • Parsing JSON with JsonDocument
// • Reading primitive values
// • Enumerating JSON arrays
// • Navigating nested JSON objects
// • Making a second request using a URL from the first response

using System.Text.Json;

using var client = new HttpClient();

Console.WriteLine("\nRequesting Pokémon list...\n");

// ======================================================================
// First Request
// ======================================================================
//
// GET https://pokeapi.co/api/v2/pokemon
//
// Returns JSON similar to:
//
// {
//     "count":1302,
//     "next":"...",
//     "previous":null,
//     "results":[
//          {
//              "name":"bulbasaur",
//              "url":"..."
//          }
//     ]
// ======================================================================

Task<string> task =
    client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

// Execute this code once the HTTP request completes.
task.ContinueWith(firstRequest =>
{
    Console.WriteLine("Pokémon list downloaded.\n");

    JsonDocument document =
        JsonDocument.Parse(firstRequest.Result);

    JsonElement root = document.RootElement;

    // ------------------------------------------------------------------
    // Read some top-level properties
    // ------------------------------------------------------------------

    Console.WriteLine($"Total Pokémon : {root.GetProperty("count")}");

    Console.WriteLine($"Next Page     : {root.GetProperty("next")}");

    Console.WriteLine();

    // ------------------------------------------------------------------
    // Read the results array
    // ------------------------------------------------------------------

    JsonElement pokemonArray = root.GetProperty("results");

    Console.WriteLine("First 10 Pokémon");
    Console.WriteLine("----------------");

    foreach (JsonElement pokemon in pokemonArray.EnumerateArray().Take(10))
    {
        Console.WriteLine(
            $"{pokemon.GetProperty("name"),-15} {pokemon.GetProperty("url")}");
    }

    Console.WriteLine();

    // ------------------------------------------------------------------
    // Read the first Pokémon
    // ------------------------------------------------------------------

    JsonElement firstPokemon = pokemonArray[0];

    string name = firstPokemon
        .GetProperty("name")
        .GetString()!;

    string url = firstPokemon
        .GetProperty("url")
        .GetString()!;

    Console.WriteLine($"First Pokémon : {name}");
    Console.WriteLine($"Details URL   : {url}");


    // Second Request

    // Download the details for the first Pokémon.


    Console.WriteLine("\nDownloading Pokémon details...\n");

    string detailsJson = client.GetStringAsync(url).Result;

    JsonDocument detailsDocument =
        JsonDocument.Parse(detailsJson);

    JsonElement pokemonRoot =
        detailsDocument.RootElement;

    // ------------------------------------------------------------------
    // Read primitive properties
    // ------------------------------------------------------------------

    Console.WriteLine($"ID         : {pokemonRoot.GetProperty("id")}");
    Console.WriteLine($"Height     : {pokemonRoot.GetProperty("height")}");
    Console.WriteLine($"Weight     : {pokemonRoot.GetProperty("weight")}");
    Console.WriteLine($"Base XP    : {pokemonRoot.GetProperty("base_experience")}");

    // ------------------------------------------------------------------
    // Read abilities
    // ------------------------------------------------------------------

    Console.WriteLine("\nAbilities");
    Console.WriteLine("---------");

    foreach (JsonElement ability in
             pokemonRoot.GetProperty("abilities").EnumerateArray())
    {
        Console.WriteLine(
            ability
                .GetProperty("ability")
                .GetProperty("name"));
    }

    // ------------------------------------------------------------------
    // Read Pokémon types
    // ------------------------------------------------------------------

    Console.WriteLine("\nTypes");
    Console.WriteLine("-----");

    foreach (JsonElement type in
             pokemonRoot.GetProperty("types").EnumerateArray())
    {
        Console.WriteLine(
            type
                .GetProperty("type")
                .GetProperty("name"));
    }

    // ------------------------------------------------------------------
    // Read stats
    // ------------------------------------------------------------------

    Console.WriteLine("\nStats");
    Console.WriteLine("-----");

    foreach (JsonElement stat in
             pokemonRoot.GetProperty("stats").EnumerateArray())
    {
        string statName =
            stat
                .GetProperty("stat")
                .GetProperty("name")
                .GetString()!;

        int value =
            stat
                .GetProperty("base_stat")
                .GetInt32();

        Console.WriteLine($"{statName,-15} {value}");
    }

});

Console.WriteLine("The main thread continues executing...");
Console.WriteLine("Press Enter to exit.");

Console.ReadLine();
