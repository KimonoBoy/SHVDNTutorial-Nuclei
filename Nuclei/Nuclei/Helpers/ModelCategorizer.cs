using System.Collections.Generic;
using GTA;
using Nuclei.Helpers.ExtensionMethods;

namespace Nuclei.Helpers;

public class ModelCategorizer
{
    public ModelCategorizer()
    {
        Categories = new Dictionary<string, List<PedHash>>();
        InitializeCategories();
    }

    public Dictionary<string, List<PedHash>> Categories { get; }

    private void InitializeCategories()
    {
        Categories.Add("Animals", new List<PedHash>());
        Categories.Add("Humans", new List<PedHash>());
        Categories.Add("Females", new List<PedHash>());
        Categories.Add("Males", new List<PedHash>());
        Categories.Add("Gangs", new List<PedHash>());
        Categories.Add("Cops, Agents and Military", new List<PedHash>());
        Categories.Add("Worker", new List<PedHash>());
        Categories.Add("Cults", new List<PedHash>());
        Categories.Add("Beach", new List<PedHash>());
        Categories.Add("Air Personnel", new List<PedHash>());
        Categories.Add("Hooker", new List<PedHash>());
        Categories.Add("Creepy", new List<PedHash>());
        Categories.Add("Cutscene", new List<PedHash>());
    }

    public void Categorize(PedHash pedHash, Model model)
    {
        if (model.IsAnimalPed)
            Categories["Animals"].Add(pedHash);
        if (model.IsHumanPed)
            Categories["Humans"].Add(pedHash);
        if (model.IsFemalePed)
            Categories["Females"].Add(pedHash);
        if (model.IsMalePed)
            Categories["Males"].Add(pedHash);
        if (model.IsGangPed)
            Categories["Gangs"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("cop") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("sheriff") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("police") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("fib") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("agent") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("vincent") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("armoured") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("cia") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("casey") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("sec") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("swat") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("blackops") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("army") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("mili") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("navy"))
            Categories["Cops, Agents and Military"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("worker"))
            Categories["Worker"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("cult"))
            Categories["Cults"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("beach") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("baywatch"))
            Categories["Beach"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("air"))
            Categories["Air Personnel"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("hooker"))
            Categories["Hooker"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("drown") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("dead") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("corpse") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("halloween") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("clown") ||
            pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("murder"))
            Categories["Creepy"].Add(pedHash);
        if (pedHash.GetLocalizedDisplayNameFromHash().ToLower().Contains("cutscene"))
            Categories["Cutscene"].Add(pedHash);
    }
}